using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using WinForms = System.Windows.Forms;


namespace WPFDocument
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FullScreenManager.RepairWpfWindowFullScreenBehavior(this);
            
        }
      
        /// <summary>
        /// 记录WinForm
        /// </summary>
        private readonly Dictionary<TabItem, WinOverlayWF> dic = new Dictionary<TabItem, WinOverlayWF>();


        /// <summary>
        /// 设置标题
        /// </summary>
        public string Text
        {
            get { return labTitle.Content.ToString(); }
            set { labTitle.Content =value; }
        }

        /// <summary>
        /// 设置左下脚显示
        /// </summary>
        public string LeftFoot
        {
            get {return labLeftFoot.Content.ToString(); }
            set { labLeftFoot.Content = value; }
        }

        /// <summary>
        /// 设置右下角显示
        /// </summary>
        public string RightFoot
        {
            get { return labRigtFoot.Content.ToString(); }
            set { labRigtFoot.Content = value; }
        }

        /// <summary>
        /// 设置图标
        /// </summary>
        /// <param name="imageSource"></param>
        public void SetLogo(string imageSource)
        {
            if (!string.IsNullOrEmpty(imageSource))
            {
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(imageSource, UriKind.Relative)));
                imgLogo.Fill = imageBrush;
            }
          
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(string title, FrameworkElement control,string imageSource)
        {
            if (control is Window)
            {
                Window cur = control as Window;
                cur.Show();
            }
            else
            {
               
                Frame frame = new Frame();
                if (control != null)
                {
                    control.Margin = new Thickness(2, 1, 2, 1);
                    control.HorizontalAlignment = HorizontalAlignment.Stretch;
                    control.VerticalAlignment = VerticalAlignment.Stretch;
                    frame.Navigate(control);
                }
                else
                {
                    Label button = new Label() { Content = "没有插件填充" };
                    button.Background = new LinearGradientBrush(Colors.LightBlue, Colors.SlateBlue, 90);
                    frame.Navigate(button);
                }
                TabItem item = new TabItem
                {
                    Content = frame,
                    Header = title
                };

                item.Height = 83;
                Thickness cur = new Thickness(tab.Items.Count * 74+5, 0, 0, 0);
                item.Margin = cur;
                item.Width = 74;
                Style myStyle = (Style)this.FindResource("TabItemStyle");//TabItemStyle 这个样式是引用的资源文件中的样式名称
                item.Style = myStyle;
                if (!string.IsNullOrEmpty(imageSource))
                {
                    ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(imageSource, UriKind.Relative)));
                    item.Background = imageBrush;
                }
                else
                {
                    ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("skin/ico/ico_PluginCleaner.png", UriKind.Relative)));
                    item.Background = imageBrush;
                }
                tab.Items.Add(item);
            }
        }

        /// <summary>
        /// 添加界面
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(string title, WinForms.Control control, string imageSource)
        {
            if (control is WinForms.Form)
            {
                WinForms.Form frm = control as WinForms.Form;
                if (frm.TopLevel)
                {
                    frm.Show();
                    return;
                }
                frm.Opacity = this.Opacity;
                frm.ShowInTaskbar = false;
                frm.FormBorderStyle = WinForms.FormBorderStyle.None;
            }
            Grid grid = new Grid();
            grid.Children.Add(new Label() { Content="正在显示WinForm" });
            TabItem item = new TabItem
            {
               Content = grid,
                Header = title
            };
            
            item.Height = 83;
            item.Width = 74;
            Thickness cur = new Thickness(tab.Items.Count * 74 + 5, 0, 0, 0);
            item.Margin = cur;
            Style myStyle = (Style)this.FindResource("TabItemStyle");//TabItemStyle 这个样式是引用的资源文件中的样式名称
            item.Style = myStyle;
            if (!string.IsNullOrEmpty(imageSource))
            {
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(imageSource, UriKind.Relative)));
                item.Background = imageBrush;
            }
            else
            {
                ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("skin/ico/ico_PluginCleaner.png", UriKind.Relative)));
                item.Background = imageBrush;
            }
            control.Show();
            WinOverlayWF wF = new WinOverlayWF(grid, this);
            wF.WinBrowser.Controls.Add(control);
            tab.Items.Add(item);
            dic[item] = wF;
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="v"></param>
        /// <param name="imagePath"></param>
        internal void AddControl(string title, object v, string imagePath)
        {
            if (v == null)
            {
                AddControl(title, (FrameworkElement)v, imagePath);
            }
            else
            {
                FrameworkElement control = v as FrameworkElement;
                if (control == null)
                {
                    WinForms.Control win = v as WinForms.Control;
                    AddControl(title, win, imagePath);
                }
                else
                {
                    AddControl(title, control, imagePath);
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;  
        }

        private void MniButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Menu.IsOpen = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(e.LeftButton== MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (e.RemovedItems.Count > 0)
                {
                    TabItem item = e.RemovedItems[0] as TabItem;
                    if (item != null && dic.ContainsKey(item))
                    {
                        var frm = dic[item];
                        frm.Visible = false;
                    }
                }
                if (e.AddedItems.Count > 0)
                {
                    TabItem item = e.AddedItems[0] as TabItem;
                    if (item != null && dic.ContainsKey(item))
                    {
                        var frm = dic[item];
                        this.Dispatcher.InvokeAsync(() =>
                        {
                            frm.Visible = true;
                        });

                    }
                    this.LeftFoot = item.Header.ToString();
                }
               e.Handled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(dic.Count>0)
            {
                double ScreenWidth = SystemParameters.PrimaryScreenWidth;//WPF
                this.Top = this.ActualWidth / 5;
                this.Left = ScreenWidth - this.ActualWidth * 1.3;
            }
        }

     
    }
}
