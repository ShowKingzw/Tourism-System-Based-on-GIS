using System;
using System.Class;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System
{
    public partial class FormSignUp : Form
    {
        public FormSignUp()
        {
            InitializeComponent();
        }

        private bool pwdEnter(string pwd1, string pwd2)
        {
            if (pwd1 == pwd2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pwdEnter(textBox2.Text, textBox3.Text))
            {
                DataBase dataBase = new DataBase();

                //dataBase.RegisterUser(textBox1, string password)
                int userID;
                if (int.TryParse(textBox1.Text, out userID))
                {
                    string password = textBox2.Text;
                    if (dataBase.RegisterUser(userID, password))
                    {
                        MessageBox.Show("注册成功", "提示");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("请输入有效的用户ID。", "提示");
                }

            }
        }
    }
}
