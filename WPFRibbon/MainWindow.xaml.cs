using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFRibbon.Controls;
namespace WPFRibbon
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public event ShowPlugin PluginEventHandler;
        /// <summary>
        ///添加Tab
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int AddTabGroup(string title)
        {
           RibbonTab tab = new RibbonTab();
            tab.Header = title;
            return this.Ribbon.Items.Add(tab);
        }

        /// <summary>
        /// 添加Group
        /// </summary>
        /// <param name="index"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public int AddGroup(int index,string title)
        {
            var t = this.Ribbon.Items[index];
            RibbonTab tab =this.Ribbon.Items[index] as RibbonTab;

            if (tab != null)
            {
                if (tab.Items.Count == 0)
                {
                    RibbonGroup ribbonGroup = new RibbonGroup();
                    ribbonGroup.Header = title;
                    ribbonGroup.Tag = tab;
                    return tab.Items.Add(ribbonGroup);
                }
                else
                {
                    RibbonGroup group = null;
                    bool isFind = false;
                    for (int i = 0; i < tab.Items.Count; i++)
                    {
                        group = (RibbonGroup)tab.Items[i];
                        if (group.Header.ToString() == title)
                        {
                            isFind = true;
                            break;
                        }
                    }
                    if (!isFind)
                    {
                        RibbonGroup ribbonGroup = new RibbonGroup();
                        ribbonGroup.Header = title;
                        ribbonGroup.Tag = tab;
                        return tab.Items.Add(ribbonGroup);
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 添加控制按钮
        /// </summary>
        /// <param name="tabindex"></param>
        /// <param name="group"></param>
        /// <param name="title"></param>
        /// <param name="imageSource"></param>
        public void AddButton(int tabindex,int group,string title,string imageSource=null)
        {
            //
            RibbonTab tab = this.Ribbon.Items[tabindex] as RibbonTab;
            if(tab!=null)
            {
                RibbonGroup ribbonGroup= tab.Items[group] as RibbonGroup;
                if(ribbonGroup!=null)
                {
                    RibbonButton button = new RibbonButton
                    {
                        Label = title,
                        Tag = ribbonGroup
                    };
                  //  button.Style = (Style)this.FindResource("BtnImgTxtStyle1");
                    button.Click += Button_Click;
                    if (!string.IsNullOrEmpty(imageSource))
                    {
                        button.SmallImageSource = new BitmapImage(new Uri(imageSource, UriKind.Relative));
                    }
                    ribbonGroup.Items.Add(button);
                }
            }
        }

        /// <summary>
        /// 添加界面
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(string title,FrameworkElement control)
        {
            if (control == null)
            {
                MessageBox.Show("没有配置该插件", "提示");
            }
            else
            {
                Grid grid = new Grid();

                Frame frame = new Frame
                {
                    Margin = new Thickness(5, 5, 5, 5)
                };
                frame.Navigate(control);
                grid.Children.Add(frame);
                TabItemClose item = new TabItemClose()
                {
                    Content = grid,
                    Header = title
                };
               // item.Style = (Style)this.FindResource("TabItemStyle");
               // item.Height = 20;
              //  item.Width = 100;
              
                grdTab.Items.Add(item);
                grdTab.SelectedItem = item;
            }
        }

        /// <summary>
        /// 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton button = sender as RibbonButton;
            for (int i=0;i<grdTab.Items.Count;i++)
            {
                TabItem item= grdTab.Items[i] as TabItem;
                if(item.HasHeader)
                {
                    if(item.Header.ToString() == button.Label)
                    {
                        grdTab.SelectedIndex = i;
                        return;
                    }
                }
            }
            if(PluginEventHandler!=null)
            {
              
                RibbonGroup ribbonGroup = button.Tag as RibbonGroup;
                string name = "";
                if(ribbonGroup!=null)
                {
                    name = ribbonGroup.Header.ToString();
                }
                PluginEventHandler(name, button.Label);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="control"></param>
        internal void AddControl(string title,object control)
        {
            FrameworkElement  element=control as FrameworkElement ;
            AddControl(title,element);
        }
    }
}
