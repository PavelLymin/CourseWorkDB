using BusinessLogic;
using System;
using System.Windows.Forms;

namespace ViewPharmacy
{
    public partial class SignIn : Form
    {
        Logic logic;

        public SignIn()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
            logic = Logic.getInstance();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (logic.CheckHaveUser(textBox1.Text, textBox2.Text) && !string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                this.Hide();
                Store store = new Store();
                store.Closed += (s, args) => this.Close();
                store.Show();
            }
            else if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                MessageBox.Show("Заполните все поля");
            else
                MessageBox.Show("Неправильный логин или пароль");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.Closed += (s, args) => this.Close();
            signUp.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)    
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
