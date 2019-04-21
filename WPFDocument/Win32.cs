#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：WPFDocument
* 项目描述 ：
* 类 名 称 ：Win32
* 类 描 述 ：
* 命名空间 ：WPFDocument
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFDocument
{
    /* ============================================================================== 
* 功能描述：Win32 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
  public static  class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            public POINT(Point pt)
            {
                X = Convert.ToInt32(pt.X);
                Y = Convert.ToInt32(pt.Y);
            }
        };

        [DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    }
}
