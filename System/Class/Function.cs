using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Function
    {
        public static void OpenMxd(AxMapControl axMapControl1, string strFullPath)
        {
            if (axMapControl1.CheckMxFile(strFullPath))
            {
                axMapControl1.LoadMxFile(strFullPath);
                axMapControl1.Extent = axMapControl1.FullExtent;
            }
        }


    }
}
