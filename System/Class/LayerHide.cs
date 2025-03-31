using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;

namespace System
{
    class LayerHide
    {


        /// <summary>
        /// 遍历图层，获取并隐藏图层
        /// </summary>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public static void HideFeatureLayers(IMap pMap,string layerName)
        {
            IFeatureLayer pFeatLayer;
           // ICompositeLayer pCompLayer;
          //  List<IFeatureLayer> pList = new List<IFeatureLayer>();
            //遍历地图
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                if (pMap.get_Layer(i) is IFeatureLayer)
                {
                    //获得图层要素
                    pFeatLayer = pMap.get_Layer(i) as IFeatureLayer;
                   // pList.Add(pFeatLayer);

                    if (pMap.get_Layer(i).Name == layerName)
                    {
                        pMap.get_Layer(i).Visible = false;
                    }
                }
                //else if (pMap.get_Layer(i) is IGroupLayer)
                //{
                //    //遍历图层组
                //    pCompLayer = pMap.get_Layer(i) as ICompositeLayer;
                //    for (int j = 0; j < pCompLayer.Count; j++)
                //    {
                //        if (pCompLayer.get_Layer(j) is IFeatureLayer)
                //        {
                //            pFeatLayer = pCompLayer.get_Layer(j) as IFeatureLayer;
                //            pList.Add(pFeatLayer);
                //        }
                //    }
                //}
            }
            //return pList.ToArray();
        }
        /// <summary>
        /// 取消图层的隐藏
        /// </summary>
        /// <param name="pMap"></param>
        public static void CancelHideLayer(IMap pMap,string layerName)
        {
            IFeatureLayer featureLayer;
            IActiveView activeView = pMap as IActiveView;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                if (pMap.get_Layer(i) is IFeatureLayer)
                {
                    featureLayer = pMap.get_Layer(i) as IFeatureLayer;
                   
                    if (pMap.get_Layer(i).Name == layerName)
                    {
                        pMap.get_Layer(i).Visible = true;
                    }
                }
            }
            activeView.Refresh();
        }




    }
}
