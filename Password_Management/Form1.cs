using System;
using System.Windows.Forms;

namespace Generate_Password
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            checkBox2.Text = Properties.Resources.UpperCase_Chars;
            checkBox3.Text = Properties.Resources.Numeric_Chars;
            checkBox4.Text = Properties.Resources.Special_Chars;

            groupBox1.Text = Properties.Resources.Type_Chars+":";
            groupBox2.Text = Properties.Resources.Options + ":";

            button1.Text = Properties.Resources.Generate_Btn;

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int length = Convert.ToInt32(numericUpDown1.Value);
                textBox1.Text = PasswordGenerator.GeneratePassword(length, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, 1);
            }
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Clipboard.SetText(textBox1.Text);
                label2.Text =  $"{Properties.Resources.Copied} ({textBox1.Text.Length}): " + textBox1.Text;
                label2.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int length = Convert.ToInt32(numericUpDown1.Value);
            textBox1.Text = PasswordGenerator.GeneratePassword(length, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, 1);
        }
    }
}
