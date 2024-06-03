
using System;
using System.Windows.Forms;

namespace Generate_Password
{
    public partial class PasswordManagerForm : Form
    {


        public LoginForm loginForm = null;
        private User currentUser;

        public PasswordManagerForm(User user)
        {
            InitializeComponent();
            currentUser = user;

        }


        private void PasswordManager_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add(ChangeLanguage.GetValueFromIniFile("Item_Name"), 100);
            listView1.Columns.Add(ChangeLanguage.GetValueFromIniFile("LB_UserName"), 100);
            listView1.Columns.Add(ChangeLanguage.GetValueFromIniFile("LB_Password"), 100);
            listView1.Columns.Add(ChangeLanguage.GetValueFromIniFile("Urls"), 100);
            listView1.Columns.Add(ChangeLanguage.GetValueFromIniFile("Description"), 100);
            listView1.Columns.Add(ChangeLanguage.GetValueFromIniFile("LB_SecureLevel"), 100);
            listView1.Columns.Add(ChangeLanguage.GetValueFromIniFile("Modification"), 100);


            listView1.ItemSelectionChanged += ListView1_ItemSelectionChanged;

            LoadData();

        }

        private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[2].Text = ChangeLanguage.GetValueFromIniFile("Message_select_to_show")
;
            }
            if (listView1.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    listView1.SelectedItems[i].SubItems[2].Text = currentUser.Passwords[i].Password;
                    listView1.SelectedItems[i].SubItems[2].ForeColor = PaswordUtil.ConvertEnumsToColor(PaswordUtil.EvaluatePassword(currentUser.Passwords[i].Password));
                }

            }
        }


        public void LoadData()
        {
            listView1.Items.Clear();
            foreach (var a in currentUser.Passwords)
            {
                ListViewItem item = new ListViewItem(
                    new string[]
                    {
                            a.ItemName,
                            a.UserName,
                            ChangeLanguage.GetValueFromIniFile("Message_select_to_show")
,
                            a.URL,
                            a.Note,
                            PaswordUtil.ConvertEnumsToSecureString(PaswordUtil.EvaluatePassword(a.Password)),
                            a.LastModified.ToString(),
                            a.Group

                    });
                item.SubItems[4].ForeColor = PaswordUtil.ConvertEnumsToColor(PaswordUtil.EvaluatePassword(a.Password));
                listView1.Items.Add(item);
            }

        }


        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                AddItem addItem = new AddItem(currentUser);
                if (addItem.ShowDialog() == DialogResult.OK)
                {
                    PasswordManager.Instance.SaveData();
                    LoadData();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            PasswordManager.Instance.SaveData();
            LoginForm.GetInstance().Show();
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        //currentUser.Passwords.RemoveAt(item.Index);
                        PasswordManager.Instance.DeletePassword(currentUser.UserName, item.Index);
                        MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Password_information_changed"), Application.ProductName + " - " + ChangeLanguage.GetValueFromIniFile("Delete_password_information"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    PasswordManager.Instance.SaveData();
                    LoadData();
                }
            }
        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ChangeItemForm addItem = new ChangeItemForm(currentUser);
                    if (addItem.ShowDialog() == DialogResult.OK)
                    {
                        PasswordManager.Instance.SaveData();
                        MessageBox.Show(ChangeLanguage.GetValueFromIniFile("Password_information_changed"), Application.ProductName + " - " + ChangeLanguage.GetValueFromIniFile("Change_password_information"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    LoadData();
                }
            }
        }

        private void linkLabel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form1 form1 = new Form1();
                form1.StartPosition = FormStartPosition.CenterScreen;
                form1.Show();
            }
        }
    }
}
