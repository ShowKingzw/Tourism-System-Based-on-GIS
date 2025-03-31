using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.NetworkAnalyst;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Editor;
//using ESRI.ArcGIS.GeoAnalyst;
//using System.Xml;
//using ESRI.ArcGIS.ArcMapUI;
//using ESRI.ArcGIS.Framework;
//using ESRI.ArcGIS.ArcMap;









namespace System
{
    public partial class NetForm : Form
    {
        private AxMapControl axMapControl1;
        //AxTOCControl axTOCControl1;
        //private bool isEditingNow;
        public NetForm(AxMapControl axMapControl)
        {
            InitializeComponent();
            axMapControl1 = axMapControl;
            //axTOCControl1= axTOCControl;
            //isEditingNow = isEditing;
        }


        //全局变量
        private INetworkDataset my_networkDataset;//网络数据集
        private INAContext my_NAContexts;//网络分析上下文
        private IFeatureClass my_InputFeatureClass;//存储输入点要素类
        private IActiveView my_ActiveView;
        private IGraphicsContainer my_GraphicsContainer;
        bool NetworkAnalysis = false;//分析准备
        int count = 0;//节点数目
        //private OpenFileDialog openFileDialog1;
        //bool isNetworkAnalysisEnabled = false;
        //IFeatureLayer pFeatureLayer;
        //IDataset pDataset;
        //IWorkspace pWs;
        IWorkspaceEdit pWorkspaceEdit;

        ////private IWorkspaceEdit pWorkspaceEdit;
        //private IFeature pFeature;
        //IWorkspaceEdit workspaceEdit;

        IFeatureLayer layer;
        IFeatureClass myClass;
        INetworkLayer netLayer;
        INALayer NALayer;


        /// <summary>
        /// 网络分析初始化
        /// </summary>
        private void initNetworkAnalysis()
        {
            string mdbPath = Form1.currentDirectory + @"\mdb\main.mdb";

            //openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Title = "打开网络数据集数据库";
            //openFileDialog1.Filter = "Personal GeoDatabase(*.mdb)|*.mdb";
            //openFileDialog1.Multiselect = false;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //打开工作空间
            IFeatureWorkspace featureWorkspace = my_OpenWorkspace(mdbPath);
            //打开网络数据集
            my_networkDataset = my_OpenNetworkDataset(featureWorkspace as IWorkspace, "main_ND", "main");
            //创建网络分析上下文
            my_NAContexts = my_CreateSolverContext(my_networkDataset);
            //获取输入点要素类
            my_InputFeatureClass = featureWorkspace.OpenFeatureClass("addpoint");
            //添加road图层
            layer = new FeatureLayerClass();
            myClass = featureWorkspace.OpenFeatureClass("lrdl1_1");
            layer.FeatureClass = myClass;
            layer.Name = myClass.AliasName;
            axMapControl1.AddLayer(layer);
            //添加NetDatset_ND_Junctions
            myClass = featureWorkspace.OpenFeatureClass("main_ND_Junctions");
            layer.FeatureClass = myClass;
            layer.Name = myClass.AliasName;
            axMapControl1.AddLayer(layer);
            //添加网络数据集图层
            netLayer = new NetworkLayerClass();
            netLayer.NetworkDataset = my_networkDataset;
            ILayer my_layer = netLayer as ILayer;
            my_layer.Name = "NetworkDataset";
            axMapControl1.AddLayer(my_layer);
            //添加网络分析图层
            NALayer = my_NAContexts.Solver.CreateLayer(my_NAContexts);
            my_layer = NALayer as ILayer;
            my_layer.Name = my_NAContexts.Solver.DisplayName;
            axMapControl1.AddLayer(my_layer);
            my_ActiveView = axMapControl1.ActiveView;
            my_GraphicsContainer = axMapControl1.ActiveView.FocusMap as IGraphicsContainer;
            //}
        }

        /// <summary>
        /// 打开工作空间
        /// </summary>
        /// <param name="strMDBName"></param>
        /// <returns></returns>
        private IFeatureWorkspace my_OpenWorkspace(string strMDBName)
        {
            IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
            IWorkspace workspace = workspaceFactory.OpenFromFile(strMDBName, 0);
            return workspace as IFeatureWorkspace;
        }

        /// <summary>
        /// 打开网络数据集
        /// </summary>
        /// <param name="networkDatasetWorkspace"></param>
        /// <param name="networkDatasetName"></param>
        /// <param name="featureDatasetName"></param>
        /// <returns></returns>
        private INetworkDataset my_OpenNetworkDataset(IWorkspace networkDatasetWorkspace, String networkDatasetName, string featureDatasetName)
        {
            if (networkDatasetWorkspace == null || networkDatasetName == "")
            {
                return null;
            }

            IDatasetContainer3 datasetContainer3 = null;
            // Geodatabase network dataset workspace
            ESRI.ArcGIS.Geodatabase.IFeatureWorkspace featureWorkspace = networkDatasetWorkspace as ESRI.ArcGIS.Geodatabase.IFeatureWorkspace; // Dynamic Cast
            ESRI.ArcGIS.Geodatabase.IFeatureDataset featureDataset = featureWorkspace.OpenFeatureDataset(featureDatasetName);
            ESRI.ArcGIS.Geodatabase.IFeatureDatasetExtensionContainer featureDatasetExtensionContainer = featureDataset as ESRI.ArcGIS.Geodatabase.IFeatureDatasetExtensionContainer; // Dynamic Cast
            ESRI.ArcGIS.Geodatabase.IFeatureDatasetExtension featureDatasetExtension = featureDatasetExtensionContainer.FindExtension(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTNetworkDataset);
            datasetContainer3 = featureDatasetExtension as ESRI.ArcGIS.Geodatabase.IDatasetContainer3; // Dynamic Cast
            if (datasetContainer3 == null)
                return null;
            ESRI.ArcGIS.Geodatabase.IDataset dataset = datasetContainer3.get_DatasetByName(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTNetworkDataset, networkDatasetName);
            return dataset as ESRI.ArcGIS.Geodatabase.INetworkDataset; // Dynamic Cast    
        }

        /// <summary>
        /// 创建网络分析上下文
        /// </summary>
        /// <param name="networkDataset"></param>
        /// <returns></returns>
        private INAContext my_CreateSolverContext(INetworkDataset networkDataset)
        {
            IDatasetComponent datasetComponent = networkDataset as IDatasetComponent;
            IDENetworkDataset deNetworkDataset = datasetComponent.DataElement as IDENetworkDataset;
            INASolver naSolver = new NARouteSolver();
            INAContextEdit naContextEdit = naSolver.CreateContext(deNetworkDataset, naSolver.Name) as INAContextEdit;
            naContextEdit.Bind(networkDataset, new GPMessagesClass());
            return naContextEdit as INAContext;
        }

        /// <summary>
        /// 网络分析准备
        /// </summary>
        private void DoNetIni()
        {
            try
            {
                NetworkAnalysis = true;
                //清除输入点要素
                ITable table = my_InputFeatureClass as ITable;
                table.DeleteSearchedRows(null);
                //清除规划路径
                table = my_NAContexts.NAClasses.get_ItemByName("Routes") as ITable;
                table.DeleteSearchedRows(null);
                //清除Stops
                INAClass naClass = my_NAContexts.NAClasses.get_ItemByName("Stops") as INAClass;
                table = naClass as ITable;
                table.DeleteSearchedRows(null);
                //清除Barriers
                naClass = my_NAContexts.NAClasses.get_ItemByName("Barriers") as INAClass;
                table = naClass as ITable;
                table.DeleteSearchedRows(null);
                my_GraphicsContainer.DeleteAllElements();
                count = 0;
                my_ActiveView.Refresh();
                //MessageBox.Show("请选择规划点");
            }
            catch
            {

            }
        }

        /// <summary>
        /// 手动添加点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddPoint(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (NetworkAnalysis == true)
            {


                IPoint pt;
                pt = my_ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                IFeature feature = my_InputFeatureClass.CreateFeature();//编辑状态下
                feature.Shape = pt;
                feature.Store();
                count++;
                ITextElement textElement = new TextElementClass();
                textElement.Text = count.ToString();
                textElement.Symbol = new TextSymbol();
                IElement ele = textElement as IElement;
                ele.Geometry = pt;
                my_GraphicsContainer.AddElement(ele, 0);
                my_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }

        /// <summary>
        /// 加载站点
        /// </summary>
        /// <param name="snapTolerance"></param>
        private void LoadNetworkLocation(int snapTolerance)
        {
            //清除项
            INAClass naClass = my_NAContexts.NAClasses.get_ItemByName("Stops") as INAClass;
            naClass.DeleteAllRows();
            //加载网络分析对象，设置容差值
            INAClassLoader naClassLoader = new NAClassLoaderClass();
            naClassLoader.Locator = my_NAContexts.Locator;
            if (snapTolerance > 0)
                naClassLoader.Locator.SnapTolerance = snapTolerance;
            naClassLoader.NAClass = naClass;
            //加载网络分析类
            int rowsIn = 0;
            int rowsLocated = 0;
            IFeatureCursor cursor = my_InputFeatureClass.Search(null, true);
            naClassLoader.Load((ICursor)cursor, null, ref rowsIn, ref rowsLocated);
            ((INAContextEdit)my_NAContexts).ContextChanged();
        }

        /// <summary>
        /// 执行最短路径分析
        /// </summary>
        private void DoNet()
        {
            try
            {
                IGPMessages messages = new GPMessagesClass();
                LoadNetworkLocation(80);
                INASolver naSolver = my_NAContexts.Solver;
                naSolver.Solve(my_NAContexts, messages, null);
                //实施后删除输入点
                ITable table = my_InputFeatureClass as ITable;
                table.DeleteSearchedRows(null);
            }
            catch
            {

            }


            my_ActiveView.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //initNetworkAnalysis();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DoNetIni();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //isNetworkAnalysisEnabled = !isNetworkAnalysisEnabled;

            //// 如果开启了网络分析，更新按钮文本
            //if (isNetworkAnalysisEnabled)
            //{
            //    button3.Text = "网络分析已开启";
            //}
            //else
            //{
            //    button3.Text = "开启网络分析";
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DoNet();
            }
            catch
            {

            }
        }

        private void NetForm_Load(object sender, EventArgs e)
        {
            //IFeatureLayer pFeatureLayer = GetFeatureLayerByName("addpoint");

            //if (pFeatureLayer != null)
            //{
            //    IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            //    IDataset pDataset = pFeatureClass as IDataset;
            //    IWorkspace pWorkspace = pDataset.Workspace;
            //    pWorkspaceEdit = pWorkspace as IWorkspaceEdit;
            //    pWorkspaceEdit.StartEditing(true);

            //    // 网络初始化
            //    initNetworkAnalysis();
            //    DoNetIni();
            //}
            //else
            //{
            //    // 处理图层未找到的情况
            //}
            try
            {
                IFeatureLayer pFeatureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
                IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                IDataset pDataset = pFeatureClass as IDataset;
                IWorkspace pWorkspace = pDataset.Workspace;
                pWorkspaceEdit = pWorkspace as IWorkspaceEdit;
                pWorkspaceEdit.StartEditing(true);

                //网络初始化
                initNetworkAnalysis();
                DoNetIni();
            }
            catch
            {

            }


        }

        private void NetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (pWorkspaceEdit != null)
            //{
            //    pWorkspaceEdit.StopEditOperation();
            //    pWorkspaceEdit.StopEditing(true);
            //}
            //结束编辑
            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);
            RemoveRoutesLayer();
            RemoveAddedLayers();
            LayerHide.HideFeatureLayers(axMapControl1.Map, "NetworkDataset");
            LayerHide.HideFeatureLayers(axMapControl1.Map, "main_ND_Junctions");
            Form1.isEditing = false;
            //base.axTOCControl1.Visible = false;
        }

        private void RemoveRoutesLayer()
        {
            // 获取地图控件
            IMap map = axMapControl1.Map;

            // 遍历地图中的所有图层
            for (int i = 0; i < map.LayerCount; i++)
            {
                // 获取当前图层
                ILayer layer = map.get_Layer(i);

                // 检查图层是否为 Routes 图层
                if (layer is IFeatureLayer featureLayer && layer.Name == "Routes")
                {
                    // 移除图层
                    map.DeleteLayer(layer);
                    break; // 找到并移除图层后，退出循环
                }
            }
        }

        private void RemoveAddedLayers()
        {
            IMap map = axMapControl1.Map;

            // 移除road图层
            if (layer != null)
            {
                //map.DeleteLayer(layer);
                layer = null;
            }


            // 移除junctions图层
            if (myClass != null)
            {
                //map.DeleteLayer((ILayer)myClass);
                myClass = null;
            }


            // 移除网络数据集图层
            if (netLayer != null)
            {
                //map.DeleteLayer((ILayer)netLayer);
                netLayer = null;
            }


            // 移除网络分析图层
            if (NALayer != null)
            {
                map.DeleteLayer((ILayer)NALayer);
                NALayer = null;
            }

        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                DoNetIni();
            }
            catch
            {

            }
        }
    }
}
