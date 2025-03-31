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

namespace System
{
    public partial class FormNearPoint : Form
    {

        public IFeatureLayer PointLayer;
        public AxMapControl mainMapControl;



        public FormNearPoint(AxMapControl axMapControl1)
        {
            InitializeComponent();
            mainMapControl = axMapControl1;
        }

        public List<object> GetUniqueValues(IFeatureClass featureClass, string fieldName)
        {
            int tFieldIndex = featureClass.Fields.FindField(fieldName);
            List<object> tUniqueValues = new List<object>();
            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();

            while (feature != null)
            {
                object value = feature.Value[tFieldIndex];
                if (!tUniqueValues.Contains(value))
                {
                    tUniqueValues.Add(value);
                }
                //将当前
                feature = featureCursor.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
            return tUniqueValues;



        }

        //public List<object> GetUniqueValues(IFeatureClass featureClass, string fieldName)
        //{
        //    //int tFieldIndex = featureClass.Fields.FindField(fieldName);
        //    //List<object> tUniqueValues = new List<object>();
        //    //IFeatureCursor featureCursor = featureClass.Search(null, false);
        //    //IFeature feature = featureCursor.NextFeature();

        //    //while (feature != null)
        //    //{
        //    //    object value = feature.Value[tFieldIndex];
        //    //    if (!tUniqueValues.Contains(value))
        //    //    {
        //    //        tUniqueValues.Add(value);
        //    //    }
        //    //    //将当前
        //    //    feature = featureCursor.NextFeature();
        //    //}
        //    //System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
        //    //return tUniqueValues;
        //    int tFieldIndex = featureClass.Fields.FindField(fieldName);

        //    if (tFieldIndex == -1)
        //    {
        //        // 如果字段未找到，输出错误消息并返回
        //        MessageBox.Show($"字段 '{fieldName}' 未找到！");
        //        return null;
        //    }

        //    List<object> tUniqueValues = new List<object>();
        //    IFeatureCursor featureCursor = featureClass.Search(null, false);
        //    IFeature feature = featureCursor.NextFeature();

        //    while (feature != null)
        //    {
        //        // 检查字段是否存在
        //        if (feature.Fields.Field[tFieldIndex] != null)
        //        {
        //            object value = feature.Value[tFieldIndex];
        //            if (!tUniqueValues.Contains(value))
        //            {
        //                tUniqueValues.Add(value);
        //            }
        //        }
        //        // 将当前
        //        feature = featureCursor.NextFeature();
        //    }

        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
        //    return tUniqueValues;


        //}

        private void button1_Click(object sender, EventArgs e)
        {
            IQueryFilter queryFilter = new QueryFilter();
            queryFilter.WhereClause = "NAME='" + this.textBox1.Text + "'";
            IFeatureCursor featureCursor = PointLayer.Search(queryFilter, false);

            IFeature feature = featureCursor.NextFeature();

            float distance = float.Parse(this.textBox2.Text);
            distance = distance / 10000;

            ITopologicalOperator topoOperator = feature.Shape as ITopologicalOperator;
            IGeometry bufferArea = topoOperator.Buffer(distance);

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            spatialFilter.WhereClause = "TYPE='" + this.comboBox1.SelectedItem.ToString() + "'";
            spatialFilter.Geometry = bufferArea;

            IFeatureSelection featureSelection = PointLayer as IFeatureSelection;
            featureSelection.SelectFeatures(spatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
            IGraphicsContainer graphicsContainer = this.mainMapControl.Map as IGraphicsContainer;
            graphicsContainer.DeleteAllElements();
            IElement element = new ArcEngineStudy.PolygonElement();

            element.Geometry = bufferArea;
            IFillShapeElement fillShapeElement = element as IFillShapeElement;
            ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            simpleFillSymbol.Color = new RgbColorClass() { Red = 255, Green = 192, Blue = 203 };
            fillShapeElement.Symbol = simpleFillSymbol;
            graphicsContainer.AddElement(element, 0);
            mainMapControl.Refresh();


            this.Close();



            //// 获取选择的地点名称和查询范围
            //string pointName = textBox1.Text;
            //float bufferDistance = float.Parse(textBox2.Text);

            //// 获取选择的设施类型
            //string selectedFacilityType = comboBox1.SelectedItem.ToString();

            //// 构造地点名称的查询过滤器
            //IQueryFilter pointQueryFilter = new QueryFilter();
            //pointQueryFilter.WhereClause = $"NAME = '{pointName}'";

            //// 获取地点图层
            //IFeatureLayer pointLayer = mainMapControl.get_Layer(0) as IFeatureLayer;

            //// 搜索符合地点名称的要素
            //IFeatureCursor pointCursor = pointLayer.Search(pointQueryFilter, false);
            //IFeature pointFeature = pointCursor.NextFeature();
            //if (pointFeature == null)
            //{
            //    MessageBox.Show($"未找到地点名称为 '{pointName}' 的要素");
            //    return;
            //}

            //// 获取地点的几何对象
            //IGeometry pointGeometry = pointFeature.Shape;

            //// 根据地点的几何对象构造缓冲区
            //ITopologicalOperator topoOperator = pointGeometry as ITopologicalOperator;
            //IGeometry bufferArea = topoOperator.Buffer(bufferDistance);

            //// 构造设施类型的查询过滤器
            //IQueryFilter facilityQueryFilter = new QueryFilter();
            //facilityQueryFilter.WhereClause = $"TYPE = '{selectedFacilityType}'";

            //// 获取设施图层
            //IFeatureLayer facilityLayer = mainMapControl.get_Layer(1) as IFeatureLayer;

            //// 根据缓冲区和设施类型搜索要素
            //ISpatialFilter spatialFilter = new SpatialFilterClass();
            //spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //spatialFilter.Geometry = bufferArea;
            //spatialFilter.WhereClause = $"TYPE = '{selectedFacilityType}'";

            //IFeatureCursor facilityCursor = facilityLayer.Search(spatialFilter, false);

            //// 选中符合条件的设施要素
            //IFeatureSelection featureSelection = facilityLayer as IFeatureSelection;
            //featureSelection.SelectFeatures(spatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

            //// 在地图上绘制缓冲区
            //IGraphicsContainer graphicsContainer = mainMapControl.Map as IGraphicsContainer;
            //graphicsContainer.DeleteAllElements();
            //IElement element = new PolygonElement();
            //element.Geometry = bufferArea;
            //IFillShapeElement fillShapeElement = element as IFillShapeElement;
            //ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            //simpleFillSymbol.Color = new RgbColorClass() { Red = 255, Green = 192, Blue = 203 };
            //fillShapeElement.Symbol = simpleFillSymbol;
            //graphicsContainer.AddElement(element, 0);

            //// 刷新地图
            //mainMapControl.Refresh();
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }


        private void FormNearPoint_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            PointLayer = mainMapControl.get_Layer(0) as IFeatureLayer;
            List<object> uniqueValues = GetUniqueValues(PointLayer.FeatureClass, "TYPE");
            this.comboBox1.Items.AddRange(uniqueValues.ToArray());
            textBox1.Text = "输入一个地点开始查询";
            textBox1.ForeColor=Color.Gray;
            textBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));



            //this.comboBox1.Items.Clear();//清空下拉列表中的选项。
            //PointLayer = mainMapControl.get_Layer(0) as IFeatureLayer;//获取第一个图层其强制转换为IFeatureLayer类理
            //List<object> uniqueValues = GetUniqueValues(PointLayer.FeatureClass, "TYPE");
            //this.comboBox1.Items.AddRange(uniqueValues.ToArray());
            //this.comboBox1.Items.Clear();

            //// 获取所有图层的唯一值列表
            //List<object> uniqueValues = GetUniqueValuesForAllLayers(mainMapControl, "TYPE");

            //// 添加唯一值到 ComboBox
            //this.comboBox1.Items.AddRange(uniqueValues.ToArray());



            //this.comboBox1.Items.Clear();

            //// 修改为获取正确的图层
            //PointLayer = mainMapControl.get_Layer(0) as IFeatureLayer;

            //// 获取唯一值列表
            //List<object> uniqueValues = GetUniqueValues(PointLayer.FeatureClass, "TYPE");

            //// 调试输出，确保 uniqueValues 包含正确的值
            //Console.WriteLine("Unique Values:");
            //foreach (var value in uniqueValues)
            //{
            //    Console.WriteLine(value.ToString());
            //}

            //// 添加唯一值到 ComboBox
            //this.comboBox1.Items.AddRange(uniqueValues.ToArray());
        }
        public List<object> GetUniqueValuesForAllLayers(AxMapControl mapControl, string fieldName)
        {
            List<object> uniqueValues = new List<object>();

            // 遍历地图控件中的所有图层
            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                // 获取当前图层
                ILayer layer = mapControl.get_Layer(i);

                // 如果图层是要素图层
                if (layer is IFeatureLayer featureLayer)
                {
                    // 获取当前图层的唯一值列表
                    List<object> layerUniqueValues = GetUniqueValues(featureLayer.FeatureClass, fieldName);

                    // 将当前图层的唯一值添加到总的唯一值列表中
                    uniqueValues.AddRange(layerUniqueValues);
                }
            }

            // 移除重复项，以确保最终的唯一值列表中没有重复的值
            uniqueValues = uniqueValues.Distinct().ToList();

            return uniqueValues;
        }



        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
            textBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //textBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        }
        //private void FormNearPoint_Load(object sender, EventArgs e)
        //{
        //    this.chboxSearch.Items.Clear(0);
        //    PointLayer = mainMapControl.get_Layer(0) as IFeatureLayer;
        //    List<object> uniqueValues = GetUniqueValues(PointLayer.FeatureClass, "fclass");
        //    this.chboxSearch.Items.AddRange(uniqueValues.ToArray());
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    IQueryFilter queryFilter = new QueryFilter();
        //    queryFilter.WhereClause = "NAME='" + this.textBox1.Text + "'";
        //    IFeatureCursor featureCursor = PointLayer.Search(queryFilter, false);
        ///////      //IFeature feature = featureCursor.NextFeature();
        //    float distance = float.Parse(this.textBox2.Text);
        //    ITopologicalOperator topo0perator = feature.Shape as ITopologicalOperator;
        //    IGeometry bufferArea = topoOperator.Buffer(distance);

        //    ISpatialFilter spatialFilter = new SpatialFilterClass();
        //    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //    spatialFilter.WhereClause = "fclass='" + this.chboxSearch.SelectedItem.ToString() + "'";
        //    spatialFilter.Geometry = bufferArea;

        //    IFeatureSelection featureSelection = PointLayer as IFeatureSelection;
        //    featureSelection.SelectFeatures(spatialFilter, esriSelectionResultEnum.esriSelectionResultNew,false);
        //    IGraphicsContainer graphicsContainer = this.mainMapControl.Map as IGraphicsContainer;
        //    graphicsContainer.DeleteAllElements();
        //    IElement element = new ArcEngineStudy.PolygonElement();

        //    //IElement element = new ArcEngineStudy.PolygonElement);
        //    element.Geometry = bufferArea;
        //    IFillShapeElement fillShapeElement = element as IFillShapeElement;
        //    ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
        //    simpleFillSymbol.Color = new RgbColorClass() { Red = 255, Green = 192, Blue = 203 };
        //    fillShapeElement.Symbol = simpleFillSymbol;
        //    graphicsContainer.AddElement(element,0);
        //    this.Close();


        //}


    }
}
