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
            LB_Welcome.Text = GetTimeDay() + $"\n{ChangeLanguage.GetValueFromIniFile("Welcome")} " + textBox1.Text;
        }

        public string GetTimeDay()
        {
            // Lấy thời gian hiện tại để xác định buổi trong ngày
            DateTime time = DateTime.Now;
            if (time.Hour >= 0 && time.Hour < 12)
            {
                return ChangeLanguage.GetValueFromIniFile("Good_Morning");
            }
            else if (time.Hour >= 12 && time.Hour < 18)
            {
                return ChangeLanguage.GetValueFromIniFile("Good_Afternoon");
            }
            else
            {
                return ChangeLanguage.GetValueFromIniFile("Good_Evening");
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

                if (userName != "" && password != "")
                {
                    if (PasswordManager.Instance.CheckUserExist(userName))
                    {
                        User user = PasswordManager.Instance.AuthenticateUser(userName, password);

                        if (user != null)
                        {
                            MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Message_Login_OK"));
                            PasswordManagerForm passwordForm = new PasswordManagerForm(user);
                            passwordForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Message_Login_Fail"));
                        }
                    }
                    else
                    {
                        // hiện thông báo yes no để tạo user mới
                        DialogResult dialogResult = MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Message_User_Not_Exist"), ChangeLanguage.GetValueFromIniFile("CreateNewUser"), MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            var createUserExist = PasswordManager.Instance.AddUser(userName, password);
                            if (createUserExist == true)
                            {
                                MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Add_User_OK"));
                            }
                            else
                            {
                                MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Message_User_Exist"));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Message_Fill_UserName_Password"));
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

            foreach (var file in System.IO.Directory.GetFiles(Application.StartupPath + "\\Lang"))
            {
                comboBox1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }

            comboBox1.SelectedItem="English";
            SetLangFile();

            label1.Text = ChangeLanguage.GetValueFromIniFile("UserName");
            label2.Text = ChangeLanguage.GetValueFromIniFile("Password");
            checkBox1.Text = ChangeLanguage.GetValueFromIniFile("Hide_Password");
            button1.Text = ChangeLanguage.GetValueFromIniFile("Login");
            button2.Text = ChangeLanguage.GetValueFromIniFile("Cancel");
            LB_Welcome.Text = GetTimeDay() + $"\n{ChangeLanguage.GetValueFromIniFile("Welcome")} " + textBox1.Text;
        }



        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            PasswordManager.Instance.SaveData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetLangFile();
        }

        void SetLangFile()
        {
            ChangeLanguage.ChangeLanguageFile(Application.StartupPath + "\\Lang\\" + comboBox1.Items[comboBox1.SelectedIndex] + ".lng");

            label1.Text = ChangeLanguage.GetValueFromIniFile("UserName");
            label2.Text = ChangeLanguage.GetValueFromIniFile("Password");
            checkBox1.Text = ChangeLanguage.GetValueFromIniFile("Hide_Password");
            button1.Text = ChangeLanguage.GetValueFromIniFile("Login");
            button2.Text = ChangeLanguage.GetValueFromIniFile("Cancel");
            LB_Welcome.Text = GetTimeDay() + $"\n{ChangeLanguage.GetValueFromIniFile("Welcome")} " + textBox1.Text;

            Refresh();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
