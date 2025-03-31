using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class PointDetail
    {
        // 属性声明
        public string Name { get; set; }
        public string Intro { get; set; }
        public string Tel { get; set; }
        public string Addr { get; set; }
        public string Time { get; set; }
        public string Price { get; set; }

        // 构造函数
        public PointDetail(string name, string intro, string tel, string addr, string time, string price)
        {
            Name = name;
            Intro = intro;
            Tel = tel;
            Addr = addr;
            Time = time;
            Price = price;
        }


    }
}
