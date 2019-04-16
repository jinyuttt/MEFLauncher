using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
       
        public string Text
        {
            get { return labTitle.Content.ToString(); }
            set { labTitle.Content =value; } }

        public string LeftFoot
        {
            get {return labLeftFoot.Content.ToString(); }
            set { labLeftFoot.Content = value; }
        }
        public string RightFoot
        {
            get { return labRigtFoot.Content.ToString(); }
            set { labRigtFoot.Content = value; }
        }

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
      
            Grid grid = new Grid();
            Frame frame = new Frame();
            frame.Margin= new Thickness(2, 1, 2, 1);
            grid.Children.Add(frame);
            if (control != null)
            {
                control.Margin= new Thickness(2, 1, 2, 1);
                control.HorizontalAlignment = HorizontalAlignment.Stretch;
                control.VerticalAlignment = VerticalAlignment.Stretch;
                frame.Navigate(control);
            }
            else
            {
                Button button = new Button() { Content = "测试" };
                button.Background = new LinearGradientBrush(Colors.LightBlue, Colors.SlateBlue, 90);
                frame.Navigate(button);
            }
            TabItem item = new TabItem
            {
                Content = grid,
                Header = title
            };
           
            item.Height = 83;
            Thickness cur = new Thickness(tab.Items.Count * 74, 0, 0, 0);
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

        internal void AddControl(string title, object v, string imagePath)
        {
            FrameworkElement control = v as FrameworkElement;
          
            AddControl(title, control, imagePath);
        }

        private void MniButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Menu.IsOpen = true;
        }
    }
}
