using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Class;

namespace System
{
    public partial class LogIn : Form
    {
        private DataBase dataBase;
        public static bool isLogIn = false;

        public LogIn()
        {
            InitializeComponent();
            dataBase = new DataBase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int userID;
            if (int.TryParse(textBox1.Text, out userID))
            {
                string password = textBox2.Text;

                // 执行数据库操作
                bool loginSuccessful = dataBase.PerformLogin(userID, password);

                if (loginSuccessful)
                {
                    //MessageBox.Show("登录成功", "提示");
                    isLogIn = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("账号密码错误。", "提示");
                }
            }
            else
            {
                MessageBox.Show("请输入有效的用户ID。", "提示");
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSignUp formSignUp = new FormSignUp();
            formSignUp.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            isLogIn = true;
            this.Close();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("找回密码请联系管理员。", "找回密码");
        }
    }
}
