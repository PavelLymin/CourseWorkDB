using BusinessLogic;
using System;
using System.Windows.Forms;

namespace ViewPharmacy
{
    public partial class Pharmacy : Form
    {
        public static Store store { get; set; }

        Logic logic;

        public CartStore cartStore {  get; set; }

        public Pharmacy()
        {
            InitializeComponent();
            logic = Logic.getInstance();
            ShowPharmacies();
        }

        public void ShowPharmacies()
        {
            foreach (var pharmacy in logic.GetPharmacy())
            {
                ListViewItem newItems = new ListViewItem(pharmacy);
                listView1.Items.Add(newItems);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logic.MakeOrder(listView1.SelectedItems[0].Text);
            cartStore.listView1.Items.Clear();
            logic.ClearCart();

            store.listView1.Items.Clear();
            store.ShowMedicines();
        }
    }
}
