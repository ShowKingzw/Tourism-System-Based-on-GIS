using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace System
{
    public partial class BookmarkManagement : Form
    {  private IMap _currentMap=null;
        Dictionary<string, ISpatialBookmark> pDictionary = new Dictionary<string, ISpatialBookmark>();
        IMapBookmarks mapBookmarks = null;
        

        public BookmarkManagement(IMap pMap)
        {
            InitializeComponent();
            _currentMap = pMap;//获取当前地图
            InitControl();
            
        }
        //获取空间书签，对tviewBookMark(treeView1)进行初始化
     
        private void InitControl()
        {
            mapBookmarks = _currentMap as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = mapBookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            string sBookMarkName = string.Empty;
            while (spatialBookmark != null)
            {
                sBookMarkName = spatialBookmark.Name;
                treeView1.Nodes.Add(sBookMarkName);//添加树节点
                pDictionary.Add(sBookMarkName, spatialBookmark);//添加到字典
                spatialBookmark = enumSpatialBookmark.Next();
            }
        }
        private void button1_Click(object sender, EventArgs e)//定位
        {
            TreeNode treeNode = treeView1.SelectedNode;
            //获取选中的书签对象
            ISpatialBookmark spatialBookmark = pDictionary[treeNode.Text];
            //缩放到选中书签的视图范围
            spatialBookmark.ZoomTo(_currentMap);
            IActiveView activeView = _currentMap as IActiveView;
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

        }

        private void button2_Click(object sender, EventArgs e)//删除
        {
            TreeNode treeNode = treeView1.SelectedNode;
            ISpatialBookmark spatialBookmark = pDictionary[treeNode.Text];
            //删除选中的书签对象
            mapBookmarks.RemoveBookmark(spatialBookmark);
            //删除字典中的数据
            pDictionary.Remove(treeNode.Text);
            //删除树节点
            treeView1.Nodes.Remove(treeNode);
            treeView1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)//关闭
        {
            Close();
        }
    }
}
