using BusinessLogic;
using System;
using System.Windows.Forms;

namespace ViewPharmacy
{
    public partial class Store : Form
    {
        Logic logic;

        public Store()
        {
            InitializeComponent();
            logic = Logic.getInstance();
            ShowMedicines();
        }

        public void ShowMedicines()
        {
            foreach (var medicine in logic.GetMedicines())
            {
                ListViewItem newItems = new ListViewItem(medicine);
                listView1.Items.Add(newItems);
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            CheckQuantity();
        }

        private void CheckQuantity()
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int id = Convert.ToInt32(listView1.SelectedItems[0].Text);
                int quantity = (int)numericUpDown1.Value;
                int quantityOfMedicine = Convert.ToInt16(listView1.SelectedItems[0].SubItems[5].Text);

                if (quantity <= quantityOfMedicine && quantity != 0)
                    logic.AddItemToCart(id, quantity);
                else if (quantity == 0)
                    MessageBox.Show($"Выберите количество товара");
                else
                    MessageBox.Show($"Извините, количество данного товара осталось {quantityOfMedicine}");
            }
            else 
            {
                MessageBox.Show("Выберите товар для добавления в корзину");
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            CartStore cartStore = new CartStore();
            Pharmacy.store = this;
            cartStore.Show();
        }
    }
}
