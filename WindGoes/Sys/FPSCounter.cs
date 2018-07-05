/*
 * 名称：帧数统计类
 * 
 * 作用：能够实现对屏幕绘制帧数的统计。
 * 	 只要2个参数一个是总帧数，另一个是当前时间。
 *	 通过 FPS() 返回帧数。
 * 作者：郝  伟
 * 
 * 时间：2010年3月21日 21：20
 * 2013-10-15 已经用于吴焱论文。
 * 
 * 更新：无
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WindGoes.Sys
{
    /// <summary>
    /// 用于计数，只要将当前帧数和时间在每次刷新时添加到类中，即可返回当前帧数。
    /// </summary>
	public class FPSCounter : ObjectBase
    {
        struct TimeCell
        {
            public int Frame;
            public DateTime Time;
            public TimeCell(int frame, DateTime dt)
            {
                Frame = frame;
                Time = dt;
            }
        }

        List<TimeCell> list = new List<TimeCell>();

        public FPSCounter() { }

        public FPSCounter(int frame, DateTime dt)
        {
            list.Add(new TimeCell(frame, dt));
        }

        public void Add(int frame, DateTime dt)
        {
            list.Insert(0, new TimeCell(frame, dt));
            if(list.Count > 100)
                list.RemoveAt(list.Count - 1);
        }

        public double FPS()
        {
            TimeSpan ts = list[0].Time - list[list.Count - 1].Time;
            return (list[0].Frame - list[list.Count - 1].Frame) / ts.TotalSeconds;
        }
    }
}
