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
    public partial class MeasureResult : Form
    {
        //声明运行结果关闭事件
        public delegate void ClosedEvent();//使用委托将方法作为参数传递
        public event ClosedEvent fClosed = null;

        public MeasureResult()
        {
            InitializeComponent();
        }
        //窗口关闭时引发委托事件

        private void MeasureResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fClosed != null)
            {
                fClosed();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MeasureResult_Load(object sender, EventArgs e)
        {
            //Form1 f = Owner as Form1;
            //Location = new Point(f.Location.X, f.Location.Y - Size.Height);
            

        }
    }
}
