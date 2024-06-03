using Generate_Password.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Generate_Password
{
    public partial class AddItem : Form
    {
        User currentUser;
        public AddItem(User currentUser)
        {
            this.currentUser = currentUser;
            InitializeComponent();
            Name= Properties.Resources.Add_Item_Title;
            label1.Text = Properties.Resources.LB_UserName+":";
            label2.Text = Properties.Resources.LB_Password+":";
            label3.Text = Properties.Resources.Urls + ":";
            label4.Text = Properties.Resources.Description + ":";
            label5.Text = Properties.Resources.LB_SecureLevel + ":";
            label6.Text = Properties.Resources.Item_Name + ":";
            button1.Text = Properties.Resources.Btn_OK;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var a = PaswordUtil.EvaluatePassword(textBox2.Text);
            label5.Text = $"{Properties.Resources.LB_PassLength}: {textBox2.Text.Length}, {Properties.Resources.LB_SecureLevel}: {CorverEnumToString(a)}";

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
                    return Properties.Resources.Password_Level_Danger;
                case PasswordSecureLevel.Normal:
                    return Properties.Resources.Password_Level_Normal;
                case PasswordSecureLevel.Safe:
                    return Properties.Resources.Password_Level_Safe;
                default:
                    return Properties.Resources.Unknown;
            }
        }

        private void AddItem_Load(object sender, EventArgs e)
        {

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show(Properties.Resources.Please_Fill_AllFields);
                    return;
                }

                PasswordManager.Instance.GetUsers().ForEach(x =>
                {
                    if (x.UserName == currentUser.UserName)
                    {
                        x.Passwords.Add(new PasswordInfo
                        {
                            ItemName = textBox4.Text,
                            UserName = textBox1.Text,
                            Password = textBox2.Text,
                            URL = textBox3.Text,
                            Note = richTextBox1.Text,
                            LastModified = DateTime.Now,
                            PasswordLength = textBox2.Text.Length
                        });
                    }
                });

            }

            DialogResult = DialogResult.OK;

        }

        private void linkLabel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
        }
    }
}
