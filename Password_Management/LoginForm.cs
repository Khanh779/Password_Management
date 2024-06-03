using System;
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
            LB_Welcome.Text = GetTimeDay() + $"\n{Properties.Resources.Welcome} " + textBox1.Text;
        }

        public string GetTimeDay()
        {
            // Lấy thời gian hiện tại để xác định buổi trong ngày
            DateTime time = DateTime.Now;
            if (time.Hour >= 0 && time.Hour < 12)
            {
                return Properties.Resources.Good_Morning;
            }
            else if (time.Hour >= 12 && time.Hour < 18)
            {
                return Properties.Resources.Good_Afternoon;
            }
            else
            {
                return Properties.Resources.Good_Evening;
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

                if(userName != "" && password!="")
                {
                    if (PasswordManager.Instance.CheckUserExist(userName))
                    {
                        User user = PasswordManager.Instance.AuthenticateUser(userName, password);

                        if (user != null)
                        {
                            MessageBox.Show(Properties.Resources.Message_Login_OK);
                            PasswordManagerForm passwordForm = new PasswordManagerForm(user);
                            passwordForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show(Properties.Resources.Message_Login_Fail);
                        }
                    }
                    else
                    {
                        // hiện thông báo yes no để tạo user mới
                        DialogResult dialogResult = MessageBox.Show(Properties.Resources.Message_User_Not_Exist, Properties.Resources.CreateNewUser, MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            var createUserExist = PasswordManager.Instance.AddUser(userName, password);
                            if (createUserExist == true)
                            {
                                MessageBox.Show(Properties.Resources.Add_User_OK);
                            }
                            else
                            {
                                MessageBox.Show(Properties.Resources.Message_User_Exist);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.Message_Fill_UserName_Password);
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

      

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            PasswordManager.Instance.SaveData();
        }
    }
}
