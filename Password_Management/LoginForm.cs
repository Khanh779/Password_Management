using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generate_Password
{
    public partial class LoginForm : Form
    {
        static LoginForm loginForm = null;
        public static LoginForm GetInstance()
        {
            if (loginForm == null || loginForm.IsDisposed)
            {
                loginForm = new LoginForm();
            }
            loginForm.BringToFront();
            return loginForm;
        }

        public LoginForm()
        {
            InitializeComponent();
            loginForm = this;
            textBox1.LostFocus += TextBox1_LostFocus;
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            LB_Welcome.Text = GetTimeDay() + "\nWelcome " + textBox1.Text;
        }

        public string GetTimeDay()
        {
            // Lấy thời gian hiện tại để xác định buổi trong ngày
            DateTime time = DateTime.Now;
            if (time.Hour >= 0 && time.Hour < 12)
            {
                return "Good morning!";
            }
            else if (time.Hour >= 12 && time.Hour < 18)
            {
                return "Good afternoon!";
            }
            else
            {
                return "Good evening!";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = checkBox1.Checked;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string userName = textBox1.Text;
                string password = textBox2.Text;

                User user = PasswordManager.Instance.AuthenticateUser(userName, password);

                if (user != null)
                {
                    MessageBox.Show("Login success!");
                    // Mở form quản lý mật khẩu
                    PasswordManagerForm passwordForm = new PasswordManagerForm(user);
                    passwordForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("User name or password is not correct!");
                }
            }
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            foreach (var a in PasswordManager.Instance.GetUsers())
            {
                textBox1.AutoCompleteCustomSource.Add(a.UserName);
            }
        }

        private void linkLabel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if(textBox1.Text!="" && textBox2.Text != "")
                {
                    var createUserExist = PasswordManager.Instance.AddUser(textBox1.Text, textBox2.Text);
                    if (createUserExist == true)
                        MessageBox.Show("Add new user success!\nPlease click login");
                    else
                    {
                        MessageBox.Show("User name is exist!");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter user name and password!");
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            PasswordManager.Instance.SaveData();
        }
    }
}
