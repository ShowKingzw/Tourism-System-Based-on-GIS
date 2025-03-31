using System;
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
    public partial class bookmarks : Form
    {
        private string f_bookmarks;//书签名
        private int f_check;//是否创建书签
        public bookmarks()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
       

        private void 确定_Click(object sender, EventArgs e)
        {
            f_bookmarks = textBox1.Text;
            textBox1.Text = "";
            f_check = 1;
            this.Close();

        }

        private void 取消_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            f_check = 0;
            this.Close();
        }
        //设置书签名为只读
        public string Bookmark
        {
            get { return f_bookmarks; }
        }

        //是否创建书签变量为只读
        public int Check
        {
            get { return f_check; }
        }

        private void bookmarks_Load(object sender, EventArgs e)
        {
          // button1.Enabled = true;
        }
        private void TxtChange(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            { button1.Enabled = false; }
            else
            { button1.Enabled = true; }
        }
    }
}
