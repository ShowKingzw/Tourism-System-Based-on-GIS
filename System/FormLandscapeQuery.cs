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
    public partial class FormLandscapeQuery : Form
    {
        private FormPointDetail formPointDetail;
        private PointDetail hotel;



        /// <summary>
        /// 给定一个文本，PictureBox，之后经行显示
        /// </summary>
        /// <param name="workPath">文本相对路径</param>
        /// <param name="pictureBox">PictureBox控件</param>
        public void ShowDetailFromPic(string workPath, PictureBox pictureBox)
        {
            FileControl.currentDirectory = Directory.GetCurrentDirectory();
            string h1 = FileControl.currentDirectory + workPath;
            hotel = FileControl.ReadFile(h1);
            formPointDetail = new FormPointDetail(hotel, pictureBox.Image);
            formPointDetail.Show();
        }
        public FormLandscapeQuery()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string path = @"\景点\219界碑公园.txt";
            ShowDetailFromPic(path, pictureBox1);
        }

        private void FormLandscapeQuery_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            string path = @"\景点\告庄西双景.txt";
            ShowDetailFromPic(path, pictureBox5);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string path = @"\景点\茶马古道景区.txt";
            ShowDetailFromPic(path, pictureBox2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string path = @"\景点\打洛独树成林风景区.txt";
            ShowDetailFromPic(path, pictureBox3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string path = @"\景点\鳄鱼谷.txt";
            ShowDetailFromPic(path, pictureBox4);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            string path = @"\景点\光芒山.txt";
            ShowDetailFromPic(path, pictureBox6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string path = @"\景点\基诺山寨景区.txt";
            ShowDetailFromPic(path, pictureBox7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            string path = @"\景点\景洪市大金塔寺.txt";
            ShowDetailFromPic(path, pictureBox8);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            string path = @"\景点\景真八角亭.txt";
            ShowDetailFromPic(path, pictureBox9);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            string path = @"\景点\绿石林景区.txt";
            ShowDetailFromPic(path, pictureBox10);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            string path = @"\景点\曼点瀑布.txt";
            ShowDetailFromPic(path, pictureBox11);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            string path = @"\景点\曼飞龙白塔.txt";
            ShowDetailFromPic(path, pictureBox12);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            string path = @"\景点\曼听公园.txt";
            ShowDetailFromPic(path, pictureBox13);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            string path = @"\景点\湄公河印象.txt";
            ShowDetailFromPic(path, pictureBox14);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            string path = @"\景点\望天树.txt";
            ShowDetailFromPic(path, pictureBox15);
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            string path = @"\景点\西双版纳勐泐文化旅游区.txt";
            ShowDetailFromPic(path, pictureBox16);
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            string path = @"\景点\西双版纳民族博物馆.txt";
            ShowDetailFromPic(path, pictureBox17);
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            string path = @"\景点\西双版纳蘑菇屋.txt";
            ShowDetailFromPic(path, pictureBox18);
        }
    }
}
