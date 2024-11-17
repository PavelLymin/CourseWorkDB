using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace DataAccess
{
    public class OleDbAccess : IRepository
    {
        private OleDbConnection mc;

        public OleDbAccess(string _connectionString)
        {
            mc = new OleDbConnection(_connectionString);
            mc.Open();
        }

        public void AddClient(User user)
        {
            string query = "INSERT INTO Клиенты (ФИО, Пароль, Логин, Почта, Номер_телефона) VALUES (@Name,@Pasword, @Login, @Email, @NumberOfPhone)";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Pasword", user.Password);
            cmd.Parameters.AddWithValue("@Login", user.Login);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@NumberOfPhone", user.NumberOfPhone);
            cmd.ExecuteNonQuery();
        }

        public void DeleteClient(int id)
        {
            string query = $"Delete From Student Where Id = {id}";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            cmd.ExecuteNonQuery();
        }

        public bool SignIn(string login, string password, User user)
        {
            bool result = false;
            string query = "SELECT * FROM Клиенты WHERE Логин = '" + login + "' AND Пароль = '" + password + "'";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                user.Name = dr["ФИО"].ToString();
                user.Password = dr["Пароль"].ToString();
                user.Login = dr["Логин"].ToString();
                user.Email = dr["Почта"].ToString();
                user.NumberOfPhone = dr["Номер_телефона"].ToString();
                result = true;
            }
            dr.Close();
            return result;
        }

        public bool LoginSearch(string login)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Клиенты WHERE Логин='" + login + "'", mc);
            OleDbDataReader dr = cmd.ExecuteReader(); 
            return dr.Read();   
        }

        public void MakeOrder(Pharmacy pharmacy, List<CartLine> cart, User user)
        {
            foreach (CartLine cartLine in cart)
            {
                OleDbCommand cmd = new OleDbCommand($"INSERT INTO Транзакции (Дата, Время_заказа, Название_аптеки, Логин_сотрудника, Логин_клиента, Номер_лекарства, Количество)" +
                    $"VALUES (@date, @time, @name, @login, @id_client, @id_medicine, @quantity)", mc);
                cmd.Parameters.AddWithValue("date", DateTime.Now.ToString("dd.MM.yyyy"));
                cmd.Parameters.AddWithValue("time", DateTime.Now.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("name", pharmacy.Name);
                cmd.Parameters.AddWithValue("login", pharmacy.LoginOfOwner);
                cmd.Parameters.AddWithValue("id_client", user.Login);
                cmd.Parameters.AddWithValue("id_medicine", cartLine.MedicinalProduct.Id);
                cmd.Parameters.AddWithValue("quantity", cartLine.Quantity);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateQuantity(List<CartLine> lineCollection)
        {
            foreach (var medicine in lineCollection)
            {
                string query = $"UPDATE Лекарства SET Количество = Количество - {medicine.Quantity} WHERE Код = {medicine.MedicinalProduct.Id}";
                OleDbCommand cmd = new OleDbCommand(query, mc);
                cmd.ExecuteNonQuery();
            }
        }

        public MedicinalProduct ReadById(int id)
        {
            MedicinalProduct medicinalProduct = new MedicinalProduct(); ;
            string query = $"SELECT * FROM Лекарства WHERE Код = {id.ToString()}";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                medicinalProduct.Id = Convert.ToInt16(dr["Код"].ToString());
                medicinalProduct.Category = dr["Категория"].ToString();
                medicinalProduct.Name = dr["Название"].ToString();
                medicinalProduct.Brand = dr["Бренд"].ToString();
                medicinalProduct.Quantity = Convert.ToInt16(dr["Количество"].ToString());
                medicinalProduct.Price = Convert.ToInt16(dr["Цена"].ToString());
            }
            dr.Close();
            return medicinalProduct;
        }

        public Pharmacy ReadByName(string name)
        {
            Pharmacy pharmacy = new Pharmacy();
            OleDbCommand cmd = new OleDbCommand($"SELECT * FROM Аптеки WHERE Название='" + name + "'", mc);
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                pharmacy.Name = dr["Название"].ToString();
                pharmacy.Address = dr["Адрес"].ToString();
                pharmacy.LoginOfOwner = dr["Логин_сотрудника"].ToString();
            }
            dr.Close(); 
            return pharmacy;
        }

        public List<string[]> GetMedicine()
        {
            List<string[]> medicines = new List<string[]>();
            OleDbCommand cmd = new OleDbCommand($"SELECT * FROM Лекарства", mc);
            var z = cmd.ExecuteReader();
            while (z.Read())
            {
                if (Convert.ToInt16(z["Количество"]) > 0)
                    medicines.Add(new string[] { z["Код"].ToString(), z["Категория"].ToString(), z["Название"].ToString(), z["Бренд"].ToString(), "В наличии", z["Количество"].ToString(), z["Цена"].ToString() + " руб." });
            }
            z.Close();
            return medicines;
        }

        public List<string[]> GetPharmacy()
        {
            List<string[]> pharmacies = new List<string[]>();
            OleDbCommand cmd = new OleDbCommand($"SELECT * FROM Аптеки", mc);
            var z = cmd.ExecuteReader();
            while (z.Read())
            {
                pharmacies.Add(new string[] { z["Название"].ToString(), "ул. " + z["Адрес"].ToString(), z["Логин_сотрудника"].ToString() });
            }
            z.Close();
            return pharmacies;
        }

        //Создание табл
        public void AddNewTableForCart()
        {
            string query = $"CREATE TABLE Cart (ID INT, Category NVARCHAR(20)," +
                $" Name NVARCHAR(20), Brand NVARCHAR(20), Quantity INT, Price INT, QuantityOfMedicine INT)";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            cmd.ExecuteNonQuery();
        }

        //Добавление в корзину табл дб
        public void AddMedicineToCart(CartLine medicinal)
        {
            string query = "INSERT INTO Cart (ID, Category, Name, Brand, Quantity, Price, QuantityOfMedicine) VALUES" +
                " (@ID,@Category, @Name, @Brand, @Quantity, @Price, @QuantityOfMedicine)";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            cmd.Parameters.AddWithValue("@ID", medicinal.MedicinalProduct.Id);
            cmd.Parameters.AddWithValue("@Category", medicinal.MedicinalProduct.Category);
            cmd.Parameters.AddWithValue("@Name", medicinal.MedicinalProduct.Name);
            cmd.Parameters.AddWithValue("@Brand", medicinal.MedicinalProduct.Brand);
            cmd.Parameters.AddWithValue("@Quantity", medicinal.MedicinalProduct.Quantity);
            cmd.Parameters.AddWithValue("@Price", medicinal.MedicinalProduct.Price);
            cmd.Parameters.AddWithValue("@QuantityOfMedicine", medicinal.Quantity);
            cmd.ExecuteNonQuery();
        }

        //Есть ли товар уже в корзине бд
        public bool ReadByIdCart(int id)
        {
            string query = $"SELECT * FROM Cart WHERE ID = {id.ToString()}";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            OleDbDataReader dr = cmd.ExecuteReader();
            return dr.Read();
        }

        //Изменять корзину бд
        public void UpdateCart(int id, int quantity)
        {
            string query = $"UPDATE Cart INNER JOIN Лекарства ON Cart.ID = Лекарства.Код SET Cart.QuantityOfMedicine = Cart.QuantityOfMedicine + {quantity}, Cart.Price = Лекарства.Цена * (Cart.QuantityOfMedicine + {quantity}) WHERE Cart.ID = {id}";
            OleDbCommand cmd = new OleDbCommand(query, mc);
            cmd.ExecuteNonQuery();
        }

        public List<string[]> GetAllFromCart() 
        {
            List<string[]> linecollection = new List<string[]>();
            OleDbCommand cmd = new OleDbCommand($"SELECT * FROM Cart", mc);
            var z = cmd.ExecuteReader();
            while (z.Read())
            {
                linecollection.Add(new string[] { z["ID"].ToString(), z["Category"].ToString(), z["Name"].ToString(), z["Brand"].ToString(), z["Quantity"].ToString(), z["Price"].ToString(), z["QuantityOfMedicine"].ToString() });
            }
            z.Close();
            return linecollection;
        }

        public bool TableExists()
        {
            bool TableExists = false; 
            DataTable dt = mc.GetSchema("Tables");
            foreach (DataRow row in dt.Rows)
            {
                if (row.ItemArray[2].ToString() == "Cart")
                {
                    TableExists = true;
                    break;
                }
            }
            return TableExists;
        }
    }
}
