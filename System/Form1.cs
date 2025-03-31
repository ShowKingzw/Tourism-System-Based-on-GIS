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
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS;
using Point = ESRI.ArcGIS.Geometry.Point;
using System.IO;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.NetworkAnalysis;
using ESRI.ArcGIS.esriSystem;
using System.Threading.Tasks;
using System.Class;

namespace System
{

    public partial class Form1 : Form
    {
        NetForm netForm;
        public static bool isEditing = false;
        public static bool isShowHot = false;
        public static string currentDirectory = Directory.GetCurrentDirectory();
        //缩放变量
        IEnvelope mEnvelope = null;
        IPoint mPoint;//= new PointClass();//缩放中心点
        IPoint mousePoint = null;//鼠标点击点
        IPoint startPnt;//= new PointClass();

        // 量测功能全局变量  
        private string MapUnits = "未知单位";//地图单位
        private IPoint pMove = null;//鼠标移动时的当前点
        private string MouseOperate = null;
        private INewLineFeedback NewLineFeedback;//追踪线对象
        private IPoint PointPt = null;//鼠标点击点
        private double ToltalLength = 0;//量测总长度
        private double SegmentLength = 0;//片段距离
        private MeasureResult measureResult = null;//量算结果窗体
        private INewPolygonFeedback NewPolygonFeedback;//追踪面对象
        private IPointCollection AreaPointCol;//面积量算时画的点进行存储；
        private object missing = Type.Missing;
        FormQuery formQuery = new FormQuery();

        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
            //缩放
            panel1.MouseWheel += new MouseEventHandler(panel1_MouseWheel);
            mPoint = new PointClass();
            axMapControl1.AutoMouseWheel = false;    //禁止axMapControl使用滚轮
            mEnvelope = axMapControl1.ActiveView.Extent;
            mPoint.X = (mEnvelope.XMax + mEnvelope.XMin) / 2;
            mPoint.Y = (mEnvelope.YMax + mEnvelope.YMin) / 2;
        }

        void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            mousePoint = axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.X, e.Y);
            double moveDisX = mousePoint.X - (mEnvelope.XMax + mEnvelope.XMin) / 2;
            double moveDisY = mousePoint.Y - (mEnvelope.YMax + mEnvelope.YMin) / 2;
            mEnvelope.CenterAt(mousePoint);
            if (e.Delta > 0)
            {
                mEnvelope.Width = mEnvelope.Width * 0.8;
                mEnvelope.Height = mEnvelope.Height * 0.8;
                mPoint.X = mousePoint.X - moveDisX * 0.8;
                mPoint.Y = mousePoint.Y - moveDisY * 0.8;
            }
            else if (e.Delta < 0)
            {
                mEnvelope.Width = mEnvelope.Width * 1.25;
                mEnvelope.Height = mEnvelope.Height * 1.25;
                mPoint.X = mousePoint.X - moveDisX * 1.2;
                mPoint.Y = mousePoint.Y - moveDisY * 1.2;
            }

            mEnvelope.CenterAt(mPoint);
            axMapControl1.ActiveView.Extent = mEnvelope;
            axMapControl1.ActiveView.Refresh();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = @"\西双版纳.mxd";
            Function.OpenMxd(axMapControl1, currentDirectory + path);
            Function.OpenMxd(axMapControl2, currentDirectory + path);
            LayerHide.HideFeatureLayers(axMapControl1.Map, "热门路线");
            LayerHide.HideFeatureLayers(axMapControl2.Map, "热门路线");
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(0.5, 0.5, true);
            axMapControl1.Extent = pEnvelope;
            axMapControl1.Refresh();
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            IEnvelope pEnvelope = axMapControl1.Extent;
            pEnvelope.Expand(1.5, 1.5, true);
            axMapControl1.Extent = pEnvelope;
            axMapControl1.Refresh();
        }

        private void 手动放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Controls.ControlsMapZoomInToolClass tool = new ControlsMapZoomInToolClass();
            tool.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = tool;
        }

        private void 手动缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ESRI.ArcGIS.Controls.ControlsMapZoomOutToolClass tool = new ControlsMapZoomOutToolClass();
            tool.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = tool;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormQuery formQuery = new FormQuery(axMapControl1);
            formQuery.Show();
        }

        private void 景区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLandscapeQuery formLandscapeQuery = new FormLandscapeQuery();
            formLandscapeQuery.Show();
        }

        private void 取消高亮ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
        }

        private void 邻近设施查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNearPoint formNearPoint = new FormNearPoint(axMapControl1);
            formNearPoint.Show();
        }

        private void 创建书签ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            bookmarks bookmarks = new bookmarks();
            bookmarks.ShowDialog();//keypoint

            string bName = bookmarks.Bookmark;//changed
            int check = bookmarks.Check;

            if (check == 1)//changed
            {
                IMapBookmarks mapBookmarks = axMapControl1.Map as IMapBookmarks;//changed
                bName = bookmarks.Bookmark;

                if (string.IsNullOrEmpty(bName)) return;

                //对书签进行重名判断
                // IMapBookmarks mapBookmarks = axMapControl1.Map as IMapBookmarks;//changed

                IEnumSpatialBookmark enumSpatialBookmark = mapBookmarks.Bookmarks;
                enumSpatialBookmark.Reset();
                ISpatialBookmark spatialBookmark;
                while ((spatialBookmark = enumSpatialBookmark.Next()) != null)
                {
                    if (bName == spatialBookmark.Name)
                    {
                        DialogResult dialog = MessageBox.Show("此书签已存在！是否替换？", "提示", MessageBoxButtons.YesNoCancel);
                        if (dialog == DialogResult.Yes)
                        {
                            mapBookmarks.RemoveBookmark(spatialBookmark);

                        }
                        else if (dialog == DialogResult.No)
                        {
                            bookmarks.Show();
                            MessageBox.Show("请重新输入点击并输入");
                        }
                        else
                        { return; }

                    }
                }
            }
            //获取当前地图的对象
            IActiveView activeView = axMapControl1.Map as IActiveView;
            //创建一个新的书签并设置其位置范围为当前视图的范围
            IAOIBookmark aOIBookmark = new AOIBookmarkClass();
            aOIBookmark.Location = activeView.Extent;

            aOIBookmark.Name = bName;//获得书签名
            //访问当前书签集,添加书签到书签集中
            IMapBookmarks mapBookmarks1 = axMapControl1.Map as IMapBookmarks;
            mapBookmarks1.AddBookmark(aOIBookmark);

        }

        private void 管理书签ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                BookmarkManagement BoManagement = new BookmarkManagement(axMapControl1.Map);
                BoManagement.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); };
        }

        private void 热门路线_Click(object sender, EventArgs e)
        {
            if (isShowHot == false)
            {
                LayerHide.CancelHideLayer(axMapControl1.Map, "热门路线");
                isShowHot = true;
            }
            else
            {
                LayerHide.HideFeatureLayers(axMapControl1.Map, "热门路线");
                isShowHot = false;
            }

        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            if (axMapControl1.Map.LayerCount > 0)
            {
                for (int i = axMapControl1.Map.LayerCount - 1; i >= 0; i--)
                {
                    axMapControl2.AddLayer(axMapControl1.get_Layer(i));
                }
                axMapControl2.Extent = axMapControl1.FullExtent;
                axMapControl2.Refresh();
            }
        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {

            if (axMapControl2.Handle != IntPtr.Zero && axMapControl2.Created)
            {
                IEnvelope pEnvelope = (IEnvelope)e.newEnvelope;
                IGraphicsContainer pGraphicsContainer = axMapControl2.Map as IGraphicsContainer;
                IActiveView pActiveView = pGraphicsContainer as IActiveView;

                pGraphicsContainer.DeleteAllElements();
                IRectangleElement pRectangleElement = new RectangleElementClass();
                IElement pElement = pRectangleElement as IElement;
                pElement.Geometry = pEnvelope;

                IRgbColor pColor = new RgbColor() as IRgbColor;
                pColor.Red = 255;
                pColor.Green = 0;
                pColor.Blue = 0;

                ILineSymbol pOutline = new SimpleLineSymbol();
                pOutline.Width = 3;
                pOutline.Color = pColor;
                pColor.Transparency = 0;

                IFillSymbol pFillSymbol = new SimpleFillSymbol();
                pFillSymbol.Color = pColor;
                pFillSymbol.Outline = pOutline;
                IFillShapeElement pFillShapeElement = pElement as IFillShapeElement;
                pFillShapeElement.Symbol = pFillSymbol;
                pGraphicsContainer.AddElement((IElement)pFillShapeElement, 0);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }

        private void 鹰眼ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (axMapControl1.Map.LayerCount > 0)
            {
                for (int i = axMapControl1.Map.LayerCount - 1; i >= 0; i--)
                {
                    axMapControl2.AddLayer(axMapControl1.get_Layer(i));
                }
                axMapControl2.Extent = axMapControl1.FullExtent;
                axMapControl2.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FormNearPoint formNearPoint = new FormNearPoint(axMapControl1);
                formNearPoint.Show();
            }
            catch
            {

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                netForm = new NetForm(axMapControl1);
                netForm.Show();
                isEditing = true;
            }
            catch
            {

            }
      
        }

        private void fMeasureResult_fColsed()
        {
            //清空线对象
            if (NewLineFeedback != null)
            {
                NewLineFeedback.Stop();
                NewLineFeedback = null;
            }
            // 清空面对象
            if (NewPolygonFeedback != null)
            {
                NewPolygonFeedback.Stop();
                NewPolygonFeedback = null;
                AreaPointCol.RemovePoints(0, AreaPointCol.PointCount); //清空点集中所有点
            }
            //清空量算画的线、面对象
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            //结束量算功能
            MouseOperate = string.Empty;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (isEditing)
            {
                netForm.AddPoint(sender, e);
            }
            //缩放
            if (e.button == 4)
            {//中键按下时，记住按下点的位置
                 startPnt = new PointClass();
                startPnt.X = axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y).X;
                startPnt.Y = axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y).Y;
                mEnvelope = axMapControl1.ActiveView.Extent;
                mPoint.X = (mEnvelope.XMax + mEnvelope.XMin) / 2;
                mPoint.Y = (mEnvelope.YMax + mEnvelope.YMin) / 2;
            }
            if (isEditing)
            {
                netForm.AddPoint(sender, e);
            }
            //量测
            //屏幕坐标点转化为地图坐标点
            PointPt = (axMapControl1.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            if (e.button == 1)
            {
                IActiveView pActiveView = axMapControl1.ActiveView;
                IEnvelope pEnvelope = new EnvelopeClass();
                switch (MouseOperate)
                {
                    case "MeasureLength":
                        //判断追踪线对象是否为空，若是则实例化并设置当前鼠标点为起始点
                        if (NewLineFeedback == null)
                        {
                            //实例化追踪线对象
                            NewLineFeedback = new NewLineFeedbackClass();
                            NewLineFeedback.Display = (axMapControl1.Map as IActiveView).ScreenDisplay;
                            //设置起点，开始动态线绘制
                            NewLineFeedback.Start(PointPt);
                            ToltalLength = 0;
                        }
                        else //如果追踪线对象不为空，则添加当前鼠标点
                        {
                            NewLineFeedback.AddPoint(PointPt);
                        }
                        //pGeometry = m_PointPt;
                        if (SegmentLength != 0)
                        {
                            ToltalLength = ToltalLength + SegmentLength;
                        }
                        break;
                    //case "MeasureArea":
                    //    if (NewPolygonFeedback == null)
                    //    {
                    //        //实例化追踪面对象
                    //        NewPolygonFeedback = new NewPolygonFeedback();
                    //        NewPolygonFeedback.Display = (axMapControl1.Map as IActiveView).ScreenDisplay;
                    //        //AreaPointCol = new PolygonClass();
                    //        AreaPointCol.RemovePoints(0, AreaPointCol.PointCount);
                    //        //开始绘制多边形
                    //        NewPolygonFeedback.Start(PointPt);
                    //        AreaPointCol.AddPoint(PointPt, ref missing, ref missing);
                    //    }
                    //    else
                    //    {
                    //        NewPolygonFeedback.AddPoint(PointPt);
                    //        AreaPointCol.AddPoint(PointPt, ref missing, ref missing);
                    //    }
                    //    break;
                    case "MeasureArea":
                        // AreaPointCol = new PolygonClass();
                        //if (AreaPointCol == null)
                        //{
                        //    NewPolygonFeedback = new NewPolygonFeedback();
                        //    NewPolygonFeedback.Display = (axMapControl1.Map as IActiveView).ScreenDisplay;
                        //    AreaPointCol = new PolygonClass();
                        //    NewPolygonFeedback.Start(PointPt);
                        //    AreaPointCol.AddPoint(PointPt);
                        //}


                        // 如果为 null，则初始化对象
                        if (NewPolygonFeedback == null)
                        {
                            NewPolygonFeedback = new NewPolygonFeedback();
                            NewPolygonFeedback.Display = (axMapControl1.Map as IActiveView).ScreenDisplay;
                            AreaPointCol = new PolygonClass();
                            NewPolygonFeedback.Start(PointPt);
                            AreaPointCol.AddPoint(PointPt);
                        }

                        else
                        {
                            NewPolygonFeedback.AddPoint(PointPt);
                            AreaPointCol.AddPoint(PointPt);
                        }
                        if (AreaPointCol.PointCount >= 3)
                        {
                            //计算周长：
                            IPolygon poly = AreaPointCol as IPolygon;
                            //measureResult.lbmeasureResult.Text = "周长为：" + poly.Length.ToString();

                            //计算面积
                            IGeometry Geo = poly as IGeometry;
                            ITopologicalOperator topo = Geo as ITopologicalOperator;
                            topo.Simplify();
                            Geo.Project(axMapControl1.Map.SpatialReference);
                            IArea area = Geo as IArea;
                            // measureResult.lbmeasureResult.Text += ";面积为：" + area.Area.ToString();
                            measureResult.lbmeasureResult.Text = String.Format(
                        "总面积为：{0:.####}平方{1};\r\n总长度为：{2:.####}{1}",
                       area.Area, MapUnits, poly.Length);
                        }
                        break;
                }
            }
            else if (e.button == 2)
            {
                MouseOperate = "";
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
        }

        private void 最短路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4_Click(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHelp formHelp = new FormHelp();
            formHelp.Show();
        }

        private void splitContainer4_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void axMapControl2_OnMouseMove_1(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.button == 1)
            {
                IPoint pPoint = new Point();
                pPoint.PutCoords(e.mapX, e.mapY);
                axMapControl1.CenterAt(pPoint);
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

            }
            else if (e.button == 2)
            {
                IEnvelope pEnvelope = axMapControl2.TrackRectangle();
                axMapControl1.Extent = pEnvelope;
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
        }

        private async void 小吃街ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "小吃街");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
            
        }

        private void 关于ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog();
        }

        private void 热门路线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            热门路线_Click(sender, e);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.ShowDialog();
        }
        /// <summary>
        /// 获取地图单位
        /// </summary>
        /// <param name="esriUnits"></param>
        /// <returns></returns>
        private string GetMapUnit(esriUnits esriUnits)
        {
            string MapUnits = string.Empty;
            switch (esriUnits)
            {
                case esriUnits.esriCentimeters:
                    MapUnits = "厘米";
                    break;
                case esriUnits.esriDecimalDegrees:
                    MapUnits = "十进制";
                    break;
                case esriUnits.esriDecimeters:
                    MapUnits = "分米";
                    break;
                case esriUnits.esriFeet:
                    MapUnits = "尺";
                    break;
                case esriUnits.esriInches:
                    MapUnits = "英寸";
                    break;
                case esriUnits.esriKilometers:
                    MapUnits = "千米";
                    break;
                case esriUnits.esriMeters:
                    MapUnits = "米";
                    break;
                case esriUnits.esriMiles:
                    MapUnits = "英里";
                    break;
                case esriUnits.esriMillimeters:
                    MapUnits = "毫米";
                    break;
                case esriUnits.esriNauticalMiles:
                    MapUnits = "海里";
                    break;
                case esriUnits.esriPoints:
                    MapUnits = "点";
                    break;
                case esriUnits.esriUnitsLast:
                    MapUnits = "UnitsLast";
                    break;
                case esriUnits.esriUnknownUnits:
                    MapUnits = "未知单位";
                    break;
                case esriUnits.esriYards:
                    MapUnits = "码";
                    break;
                default:
                    break;
            }
            return MapUnits;
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //缩放
            if (e.button == 4)
            {
                //鼠标中键按下且拖动时         
                this.Cursor = System.Windows.Forms.Cursors.SizeAll;
                mousePoint = axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                double moveDisX = mousePoint.X - startPnt.X;
                double moveDisY = mousePoint.Y - startPnt.Y;

                mPoint.X = mPoint.X - moveDisX;
                mPoint.Y = mPoint.Y - moveDisY;

                mEnvelope.CenterAt(mPoint);
                axMapControl1.ActiveView.Extent = mEnvelope;
                axMapControl1.ActiveView.Refresh();
            }
            MapUnits = GetMapUnit(axMapControl1.Map.MapUnits);
            barTxt.Text = string.Format("当前坐标：X = {0:#.###} Y = {1:#.###} {2}", e.mapX, e.mapY, MapUnits);
            pMove = (axMapControl1.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            //长度测量
            if (MouseOperate == "MeasureLength")
            {
                if (NewLineFeedback != null)
                {
                    NewLineFeedback.MoveTo(pMove);
                }
                double deltaX = 0; //两点之间X差值
                double deltaY = 0; //两点之间Y差值

                if ((PointPt != null) && (NewLineFeedback != null))
                {
                    deltaX = pMove.X - PointPt.X;
                    deltaY = pMove.Y - PointPt.Y;
                    SegmentLength = Math.Round(Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)), 3);
                    ToltalLength = ToltalLength + SegmentLength;
                    if (measureResult != null)
                    {
                        measureResult.lbmeasureResult.Text = String.Format(
                            "当前线段长度：{0:.###}{1};\r\n总长度为: {2:.###}{1}",
                            SegmentLength, MapUnits, ToltalLength);
                        ToltalLength = ToltalLength - SegmentLength; //鼠标移动到新点重新开始计算
                    }
                    measureResult.fClosed += new MeasureResult.ClosedEvent(fMeasureResult_fColsed);
                }
            }
            //面积测量
            //AreaPointCol = new MultipointClass();
            //if (MouseOperate == "MeasureArea")
            //{

            //    if (NewPolygonFeedback != null)
            //    {
            //        NewPolygonFeedback.MoveTo(pMove);
            //    }

            //    IPointCollection pPointCol = new Polygon();
            //    IPolygon pPolygon = new PolygonClass();
            //    IGeometry pGeo = null;

            //    ITopologicalOperator pTopo = null;
            //    for (int i = 0; i <= AreaPointCol.PointCount - 1; i++)
            //    {
            //        pPointCol.AddPoint(AreaPointCol.get_Point(i), ref missing, ref missing);
            //    }
            //    pPointCol.AddPoint(pMove, ref missing, ref missing);

            //    if (pPointCol.PointCount < 3) //{ return; }
            //    //if (pPointCol.PointCount >= 3)
            //    //{
            //    //    pPolygon = pPointCol as IPolygon;
            //    //}
            //    pPolygon = pPointCol as IPolygon;
            //    if ((pPolygon != null)&&(measureResult != null))
            //        {
            //            pPolygon.Close();
            //            pGeo = pPolygon as IGeometry;
            //            pTopo = pGeo as ITopologicalOperator;
            //            //使几何图形的拓扑正确
            //            pTopo.Simplify();
            //            pGeo.Project(axMapControl1.Map.SpatialReference);
            //            IArea pArea = pGeo as IArea;
            //        measureResult.lbmeasureResult.Text = String.Format(
            //         "总面积为：{0:.####}平方{1};\r\n总长度为：{2:.####}{1}",
            //        pArea.Area, MapUnits, pPolygon.Length);

            //        pPolygon = null;
            //        }
            //    //if (pPointCol.PointCount < 3) return;
            //    //else
            //    //{ return; }
            //}
        }

        private void axMapControl1_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            #region 长度量算
            if (MouseOperate == "MeasureLength")
            {
                if (measureResult != null)
                {
                    measureResult.lbmeasureResult.Text = "线段总长度为：" + ToltalLength + MapUnits;
                }
                if (NewLineFeedback != null)
                {
                    NewLineFeedback.Stop();
                    NewLineFeedback = null;
                    //清空所画的线对象
                    (axMapControl1.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                }
                ToltalLength = 0;
                SegmentLength = 0;
            }
            #endregion

            #region 面积量算
            if (MouseOperate == "MeasureArea")
            {
                if (NewPolygonFeedback != null)
                {
                    NewPolygonFeedback.Stop();
                    NewPolygonFeedback = null;
                    //清空所画的线对象
                    (axMapControl1.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                }
                AreaPointCol.RemovePoints(0, AreaPointCol.PointCount); //清空点集中所有点
            }
            #endregion
        }

        private void 距离测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //MeasureResult f = new MeasureResult();
            //f.Show(this);
            axMapControl1.CurrentTool = null;
            MouseOperate = "MeasureLength";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            if (measureResult == null || measureResult.IsDisposed)
            {
                measureResult = new MeasureResult();

                measureResult.fClosed += new MeasureResult.ClosedEvent(fMeasureResult_fColsed);
                measureResult.lbmeasureResult.Text = "";
                measureResult.Text = "距离量测";
                measureResult.Show();
                //measureResult.BringToFront();
            }
            else
            {
                measureResult.Activate();
            }
        }

        private void 面积测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            axMapControl1.CurrentTool = null;
            MouseOperate = "MeasureArea";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            if (measureResult == null || measureResult.IsDisposed)
            {
                measureResult = new MeasureResult();
                measureResult.fClosed += new MeasureResult.ClosedEvent(fMeasureResult_fColsed);
                measureResult.lbmeasureResult.Text = "";
                measureResult.Text = "面积量测";
                measureResult.Show();
            }
            else
            {
                measureResult.Activate();
            }
        }

        private void 酒店ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHotelQuery formHotelQuery = new FormHotelQuery();
            formHotelQuery.Show();
        }

        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {

        }

        private async void 基诺小吃街ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "基诺小吃街");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
        }

        private async void 江边夜市小吃街ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "江边夜市小吃街");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            } 
        }

        private async void 浩宇美食街ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "浩宇美食街");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
        }

        private async void 中缅泰小吃街ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "中缅泰小吃街");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
        }

        private async void 美食美课ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "美食美课");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
        }

        private async void 僾伲山咔咔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "僾伲山咔咔");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
        }

        private async void 晓杉小吃店ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuery.Query(axMapControl1, "晓杉小吃店");

            // 延迟5秒后尝试清除高亮
            await Delay.DelayAsync();
            try
            {
                HighLight.ClearHighlights(axMapControl1.Map);
            }
            catch
            {

            }
        }
    }
}
