using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class HighLight
    {
        public static void HighlightFeaturesByAttribute(IMap map, string attributeName, string searchValue)
        {
            IActiveView activeView = map as IActiveView;

            // 遍历地图中的每个图层
            for (int i = 0; i < map.LayerCount; i++)
            {
                try
                {
                    if (map.get_Layer(i) is IFeatureLayer)
                    {
                        IFeatureLayer featureLayer = map.get_Layer(i) as IFeatureLayer;

                        // 验证图层是否具有属性表
                        if (featureLayer == null || featureLayer.FeatureClass == null)
                            continue;

                        // 获取属性表
                        ITable table = featureLayer.FeatureClass as ITable;

                        // 创建查询过滤器
                        IQueryFilter queryFilter = new QueryFilterClass();
                        //queryFilter.WhereClause = $"{attributeName} = '{searchValue}'";
                        //queryFilter.WhereClause = $"\"{attributeName}\" = '{searchValue}'";
                        queryFilter.WhereClause = $"\"{attributeName}\" LIKE '%{searchValue}%'";



                        // 执行查询
                        ICursor cursor = table.Search(queryFilter, true);
                        IRow row = cursor.NextRow();

                        // 高亮显示找到的元素
                        while (row != null)
                        {
                            // 获取要素的几何体
                            IFeature feature = row as IFeature;
                            IGeometry geometry = feature.Shape;

                            // 高亮显示要素
                            HighlightFeature(activeView, featureLayer, geometry);

                            row = cursor.NextRow();
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            // 刷新视图
            activeView.Refresh();
        }

        public static void HighlightFeature(IActiveView activeView, IFeatureLayer featureLayer, IGeometry geometry)
        {
            // 创建高亮符号
            ISimpleFillSymbol fillSymbol = new SimpleFillSymbolClass();
            fillSymbol.Color = GetRGBColor(255, 0, 0); // 设置红色高亮

            // 创建元素
            IElement element = null;
            if (geometry.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                // 针对点要素创建点符号
                ISimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbolClass();
                markerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                markerSymbol.Size = 8;
                markerSymbol.Color = GetRGBColor(255, 0, 0);
                element = new MarkerElementClass();
                (element as IMarkerElement).Symbol = markerSymbol;
            }
            else if (geometry.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                // 针对线要素创建线符号
                ISimpleLineSymbol lineSymbol = new SimpleLineSymbolClass();
                lineSymbol.Color = GetRGBColor(255, 0, 0);
                lineSymbol.Width = 4;
                element = new LineElementClass();
                (element as ILineElement).Symbol = lineSymbol;
            }
            else if (geometry.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                // 针对面要素创建面符号
                element = new PolygonElementClass();
                (element as IFillShapeElement).Symbol = fillSymbol;
            }

            // 设置元素的几何体
            element.Geometry = geometry;

            // 添加元素到图层
            activeView.GraphicsContainer.AddElement(element, 0);
        }

        public static IRgbColor GetRGBColor(int red, int green, int blue)
        {
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = red;
            rgbColor.Green = green;
            rgbColor.Blue = blue;
            return rgbColor;
        }

        public static void ClearHighlights(IMap map)
        {
            IActiveView activeView = map as IActiveView;

            // 清除图形容器中的所有元素
            activeView.GraphicsContainer.DeleteAllElements();

            // 刷新视图
            activeView.Refresh();
        }
    }
}
