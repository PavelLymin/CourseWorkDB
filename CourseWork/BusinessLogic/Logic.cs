using DataAccess;
using Model;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class Logic
    {
        private static AuthService _authService;
        private static CartService _cartService;
        private static OrderService _orderService;

        User user = new User();

        List<CartLine> lineCollection = new List<CartLine>();

        static IRepository _repository;

        private static Logic instance;
        private Logic() { }

        public static Logic getInstance()
        {
            if (instance == null)
            {
                instance = new Logic();
                _repository = new OleDbAccess("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=CoursePaper.mdb");
                _authService = new AuthService(_repository);
                _cartService = new CartService(_repository);
                _orderService = new OrderService(_repository);
            }
            return instance;
        }

        public bool CheckRegister(string name, string mail, string phone, string login, string password)
        {
            return _authService.CheckRegister(name, mail, phone, login, password, user);
        }

        public bool CheckHaveUser(string login, string password)
        {
            return _authService.CheckHaveUser(login, password, user);
        }

        public List<string[]> GetMedicines()
        {
            return _repository.GetMedicine();
        }

        public List<string[]> GetPharmacy()
        {
            return _repository.GetPharmacy();
        }

        public void AddItemToCart(int id, int quantity)
        {
            _cartService.AddItemToCart(id, quantity, lineCollection);
        }

        public void ClearCart()
        {
            _cartService.ClearCart(lineCollection);
        }

        public void DeleteFromCart(int index)
        {
            _cartService.DeleteFromCart(index, lineCollection);
        }

        public void UpdateQuantityFromCart(int index, int quantity)
        {
            _cartService.UpdateQuantityFromCart(index, quantity, lineCollection);
        }

        public List<string[]> GetCartOfMedicines()
        {
            return _cartService.GetCartOfMedicines(lineCollection);
        }

        public void MakeOrder(string name)
        {
            _orderService.MakeOrder(name, lineCollection, user);
        }
    }
}
