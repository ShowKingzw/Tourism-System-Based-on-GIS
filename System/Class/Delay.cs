using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace System.Class
{
    public class Delay
    {
        public Delay()
        {
            Timer timer = new Timer();

            // 设置定时器间隔（以毫秒为单位）
            timer.Interval = 5000; // 5000毫秒，即5秒

            // 绑定定时器触发事件
            //timer.Elapsed += TimerElapsed;

            // 启动定时器
            timer.Start();
        }

        public static async Task DelayAsync()
        {
            await Task.Delay(5000);
        }
    }
}
