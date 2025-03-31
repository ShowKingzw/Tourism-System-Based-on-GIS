using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace System
{
    public partial class FormQuery : Form
    {
        //private AxMapControl mainMapControl;

        //private FlowLayoutPanel flowLayoutPanel;
        //private Timer timer;
        //private int scrollSpeed = 5; // 滚动速度
        //private int spacing = 10; // PictureBox 之间的间隔
        private AxMapControl mainMapControl;

        private VScrollBar vScrollBar;
        private HScrollBar hScrollBar;
        private Panel contentPanel;
        public FormQuery(AxMapControl axMapControl1)
        {
            InitializeComponent();
            mainMapControl = axMapControl1;
            //InitializeUI();

        }





        public FormQuery()
        {
        }

        public static void Query(AxMapControl axMapControl1,string pName)
        {
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
            HighLight.HighlightFeaturesByAttribute(axMapControl1.Map, "NAME", pName);
        }

        public void FormQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                 HighLight.ClearHighlights(mainMapControl.Map);
            }
            catch
            {

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void button1_Click_1(object sender, EventArgs e)
        {
            FormQuery.Query(mainMapControl, textBox1.Text);
        }

        private void FormQuery_Load(object sender, EventArgs e)
        {

        }
    }
}
