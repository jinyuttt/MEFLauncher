#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：LauncherCommon
* 项目描述 ：
* 类 名 称 ：ConsoleHepler
* 类 描 述 ：
* 命名空间 ：LauncherCommon
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
using System.Runtime.InteropServices;

namespace LauncherCommon
{
    /* ============================================================================== 
* 功能描述：ConsoleHepler  win隐藏窗口
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class ConsoleHepler
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]   //找子窗体   
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]   //用于发送信息给窗体   
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        [DllImport("User32.dll", EntryPoint = "ShowWindow")]   //
        private static extern bool ShowWindow(IntPtr hWnd, int type);

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <param name="name"></param>
        public static void Hide(string name)
        {
            Console.Title = name;
            IntPtr ParenthWnd = new IntPtr(0);
            ParenthWnd = FindWindow(null,name);
            ShowWindow(ParenthWnd,0);//隐藏本dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化
        }
    }
}
