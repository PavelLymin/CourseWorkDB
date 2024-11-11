using BusinessLogic;
using System;
using System.Windows.Forms;

namespace ViewPharmacy
{
    public partial class SignUp : Form
    {
        public Logic logic;

        public SignUp()
        {
            InitializeComponent();
            logic = Logic.getInstance();
            Password.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (logic.CheckRegister(NameOfUser.Text, Email.Text, Number.Text, Login.Text, Password.Text))
                {
                    this.Hide();
                    var strore = new Store();
                    strore.Closed += (s, args) => this.Close();
                    strore.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Password.UseSystemPasswordChar = false;
            }
            else
            {
                Password.UseSystemPasswordChar = true;
            }
        }
    }
}
