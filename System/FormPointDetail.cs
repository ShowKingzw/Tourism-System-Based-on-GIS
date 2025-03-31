using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System
{
    public partial class FormPointDetail : Form
    {
        private PointDetail point;
        public FormPointDetail(PointDetail pointDetail)
        {
            InitializeComponent();
            point = pointDetail;
        }
        public FormPointDetail(PointDetail pointDetail, Image pic)
        {
            InitializeComponent();
            point = pointDetail;
            pictureBox1.Image = pic;
        }

        private void FormPointDetail_Load(object sender, EventArgs e)
        {
            label1.Text = point.Name;
            richTextBox1.Text = point.Intro;
            label3.Text = point.Tel;
            label4.Text = point.Addr;
            label5.Text = point.Time;
            label6.Text = point.Price;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.baidu.com");
        }


    }
}
