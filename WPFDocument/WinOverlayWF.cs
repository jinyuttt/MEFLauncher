#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：WPFDocument
* 项目描述 ：
* 类 名 称 ：WebBrowserOverlayWF
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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPFDocument
{
    /* ============================================================================== 
* 功能描述：WinOverlayWF WinForm控件显示 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    class WinOverlayWF
    {
        /// <summary>
        /// 主界面
        /// </summary>
       private readonly Window _owner;

        /// <summary>
        /// 遮盖的控件
        /// </summary>
       private FrameworkElement _placementTarget;

        /// <summary>
        /// 显示的WinForm
        /// </summary>
       private readonly Form _form; 

        /// <summary>
        /// 放置控件
        /// </summary>
       private Panel _wb = new Panel();

       /// <summary>
       /// 封装位置
       /// </summary>
        public Panel WinBrowser
        {
            get { return _wb; }
        }

       /// <summary>
       /// 显示
       /// </summary>
        public bool Visible
        {
            get { return _form.Visible; }
            set { _form.Visible = value; }
        }

        public WinOverlayWF(FrameworkElement placementTarget,Window window)
        {
            _placementTarget = placementTarget;
            Window owner = window;
            Debug.Assert(owner != null);
            _owner = owner;
            _form = new Form
            {
                Opacity = owner.Opacity,
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.None
            };
            _wb.Dock = DockStyle.Fill;
            _form.Controls.Add(_wb);
            owner.SizeChanged += delegate { OnSizeLocationChanged(); };
            owner.LocationChanged += delegate { OnSizeLocationChanged(); };
            _placementTarget.SizeChanged += delegate { OnSizeLocationChanged(); };

            if (owner.IsVisible)
                InitialShow();
            else
                owner.SourceInitialized += delegate
                {
                    InitialShow();
                };

            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(UIElement.OpacityProperty, typeof(Window));
            dpd.AddValueChanged(owner, delegate { _form.Opacity = _owner.Opacity; });

            _form.FormClosing += delegate { _owner.Close(); };
        }

        /// <summary>
        /// 初始化窗口
        /// </summary>
       private  void InitialShow()
        {
            NativeWindow owner = new NativeWindow();
            owner.AssignHandle(((HwndSource)HwndSource.FromVisual(_owner)).Handle);
            _form.Show(owner);
            owner.ReleaseHandle();
        }

       /// <summary>
       /// 回调
       /// </summary>
       private DispatcherOperation _repositionCallback;

       private  void OnSizeLocationChanged()
        {
            // To reduce flicker when transparency is applied without DWM composition, 
            // do resizing at lower priority.
            if (_repositionCallback == null)
                _repositionCallback = _owner.Dispatcher.BeginInvoke(new Action(this.Reposition), DispatcherPriority.Input);

        }

       private  void Reposition()
        {
            _repositionCallback = null;

            Point offset = _placementTarget.TranslatePoint(new Point(), _owner);
            Point size = new Point(_placementTarget.ActualWidth, _placementTarget.ActualHeight);
            HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(_owner);
            CompositionTarget ct = hwndSource.CompositionTarget;
            offset = ct.TransformToDevice.Transform(offset);
            size = ct.TransformToDevice.Transform(size);

            Win32.POINT screenLocation = new Win32.POINT(offset);
            Win32.ClientToScreen(hwndSource.Handle, ref screenLocation);
            Win32.POINT screenSize = new Win32.POINT(size);

            Win32.MoveWindow(_form.Handle, screenLocation.X, screenLocation.Y, screenSize.X, screenSize.Y, true);
            _form.SetBounds(screenLocation.X, screenLocation.Y, screenSize.X, screenSize.Y);
            _form.Update();
        }
      
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            this._form.Close();
            this._form.Dispose();
        }

    }
}
