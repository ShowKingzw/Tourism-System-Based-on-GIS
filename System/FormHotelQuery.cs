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
    public partial class FormHotelQuery : Form
    {   



        private FormPointDetail formPointDetail;
        private PointDetail hotel;
        //string currentDirectory;
        public FormHotelQuery()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 给定一个文本，PictureBox，之后经行显示
        /// </summary>
        /// <param name="workPath">文本相对路径</param>
        /// <param name="pictureBox">PictureBox控件</param>
        public void ShowDetailFromPic(string workPath,PictureBox pictureBox)
        {
            FileControl.currentDirectory = Directory.GetCurrentDirectory();
            string h1 = FileControl.currentDirectory + workPath;
            hotel = FileControl.ReadFile(h1);
            formPointDetail = new FormPointDetail(hotel, pictureBox.Image);
            formPointDetail.Show();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\汉庭酒店(西双版纳告庄西双景店)\汉庭酒店(西双版纳告庄西双景店).txt";
            ShowDetailFromPic(path, pictureBox1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\湄公河景兰大酒店\湄公河景兰大酒店.txt";
            ShowDetailFromPic(path, pictureBox2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\维也纳国际酒店(滨江俊园店)\维也纳国际酒店(滨江俊园店).txt";
            ShowDetailFromPic(path, pictureBox3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\世纪金源大饭店\世纪金源大饭店.txt";
            ShowDetailFromPic(path, pictureBox4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\美度大酒店\美度大酒店.txt";
            ShowDetailFromPic(path, pictureBox5);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\汉庭酒店(西双版纳孔雀湖店)\汉庭酒店(西双版纳孔雀湖店).txt";
            ShowDetailFromPic(path, pictureBox6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\凯里亚德酒店(西双版纳景洪曼弄金湾嘎洒机场店).txt";
            ShowDetailFromPic(path, pictureBox7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\西双版纳柏栎子居曼岛秘境酒店.txt";
            ShowDetailFromPic(path, pictureBox8);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

            string path = @"\酒店\西双版纳景洪尚水田园·湖畔观景美宿.txt";
            ShowDetailFromPic(path, pictureBox9);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\西双版纳湄公河景兰大酒店(东塔店).txt";
            ShowDetailFromPic(path, pictureBox10);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            string path = @"\酒店\西双版纳栖林墅旅居度假酒店.txt";
            ShowDetailFromPic(path, pictureBox11);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

            string path = @"\酒店\西双版纳寨子里傣院.txt";
            ShowDetailFromPic(path, pictureBox12);
        }
    }
}
