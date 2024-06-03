using Generate_Password.Enums;
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Generate_Password
{
    public partial class ChangeItemForm : Form
    {
        User currentUser;
        public ChangeItemForm(User currentUser)
        {
            this.currentUser = currentUser;
            InitializeComponent();
            Name = ChangeLanguage.GetValueFromIniFile("Change_Item");
            label1.Text = ChangeLanguage.GetValueFromIniFile("LB_UserName") + ":";
            label2.Text = ChangeLanguage.GetValueFromIniFile("LB_Password") + ":";
            label3.Text = ChangeLanguage.GetValueFromIniFile("Urls") + ":";
            label4.Text = ChangeLanguage.GetValueFromIniFile("Description") + ":";
            label5.Text = ChangeLanguage.GetValueFromIniFile("LB_SecureLevel") + ":";
            button1.Text = ChangeLanguage.GetValueFromIniFile("Btn_OK");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var a = PaswordUtil.EvaluatePassword(textBox2.Text);
            label5.Text = $"{ChangeLanguage.GetValueFromIniFile("LB_PassLength")}: {textBox2.Text.Length}, {ChangeLanguage.GetValueFromIniFile("LB_SecureLevel")}: {CorverEnumToString(a)}";


            switch (a)
            {
                case PasswordSecureLevel.Danger:
                    label5.ForeColor = Color.Red;
                    break;
                case PasswordSecureLevel.Normal:
                    label5.ForeColor = Color.Orange;
                    break;
                case PasswordSecureLevel.Safe:
                    label5.ForeColor = Color.Green;
                    break;
                default:
                    break;
            }
        }

        string CorverEnumToString(PasswordSecureLevel passwordSecureLevel)
        {
            switch (passwordSecureLevel)
            {
                case PasswordSecureLevel.Danger:
                    return ChangeLanguage.GetValueFromIniFile("Password_Level_Danger");
                case PasswordSecureLevel.Normal:
                    return ChangeLanguage.GetValueFromIniFile("Password_Level_Normal");
                case PasswordSecureLevel.Safe:
                    return ChangeLanguage.GetValueFromIniFile("Password_Level_Safe");
                default:
                    return ChangeLanguage.GetValueFromIniFile("Unknown");
            }

        }

        private void AddItem_Load(object sender, EventArgs e)
        {

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Please_Fill_AllFields"));

                    return;
                }

                var getus = PasswordManager.Instance.GetUsers();
                getus.ForEach(x =>
                {
                    if (x.UserName == currentUser.UserName)
                    {
                        var a = new PasswordInfo
                        {
                            UserName = textBox1.Text,
                            Password = textBox2.Text,
                            URL = textBox3.Text,
                            Note = richTextBox1.Text,
                            LastModified = DateTime.Now,
                            PasswordLength = textBox2.Text.Length
                        };
                        PasswordManager.Instance.ChangePassword(currentUser.UserName, getus.IndexOf(x), a);
                    }
                });

            }

            DialogResult = DialogResult.OK;

        }
    }
}
