using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                int length = Convert.ToInt32(numericUpDown1.Value);
                textBox1.Text = PasswordGenerator.GeneratePassword(length, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, 1);
            }    
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                Clipboard.SetText(textBox1.Text);
                label2.Text = $"Copied! ({textBox1.Text.Length}): "+ textBox1.Text;
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
