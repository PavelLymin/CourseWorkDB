using BusinessLogic;
using System.Windows.Forms;

namespace ViewPharmacy
{
    public partial class CartStore : Form
    {
        Logic logic;

        public CartStore()
        {
            InitializeComponent();
            logic = Logic.getInstance();
            ShowCart();
        }

        private void ShowCart()
        {
            listView1.Items.Clear();
            foreach (var medicine in logic.GetAllFromCart())
            {
                ListViewItem newItems = new ListViewItem(medicine);
                listView1.Items.Add(newItems);
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (logic.GetAllFromCart().Count != 0)
            {
                Pharmacy pharmacy = new Pharmacy();
                pharmacy.cartStore = this;
                pharmacy.Show();
            }
            else
            {
                MessageBox.Show("Добавьте в корзину товар, чтобы можно было перейти к его оформлению");
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                logic.DeleteFromCart(listView1.SelectedIndices[0]);
                listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
            }
            else
                MessageBox.Show("Выберите лекарство для удаления.");
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                logic.UpdateQuantityFromCart(listView1.SelectedIndices[0], (int)numericUpDown1.Value);
                ShowCart();
            }
            else
                MessageBox.Show("Корзина пуста");
        }
    }
}
