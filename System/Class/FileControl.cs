using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace System
{
    static class FileControl
    {
        // 从文件中读取并创建 PointDetail 对象
        public static string currentDirectory;

        public static PointDetail ReadFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            // 创建 PointDetail 对象
            PointDetail pointDetail = new PointDetail(null, null, null, null, null, null);

            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(currentLine))
                    continue;

                // 获取属性名和值
                string currentPropertyName = currentLine.Substring(0, currentLine.Length - 1);
                string currentValue = i + 1 < lines.Length ? lines[i + 1].Trim() : string.Empty;

                // 设置属性值
                switch (currentPropertyName)
                {
                    case "名称":
                        pointDetail.Name = currentValue;
                        break;
                    case "介绍":
                        pointDetail.Intro = currentValue;
                        break;
                    case "电话":
                        pointDetail.Tel = currentValue;
                        break;
                    case "地址":
                        pointDetail.Addr = currentValue;
                        break;
                    case "营业时间":
                        pointDetail.Time = currentValue;
                        break;
                    case "价格":
                        pointDetail.Price = currentValue;
                        break;
                }

                // 跳过下一行，因为它已经被处理过了
                i++;
            }

            return pointDetail;
        }



    }
}
