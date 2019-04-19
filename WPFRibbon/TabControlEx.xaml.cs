using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFRibbon
{
    /// <summary>
    /// TabControlEx.xaml 的交互逻辑
    /// </summary>
    public partial class TabControlEx : TabControl
    {
        /// <summary>
        /// TabItem右键菜单源
        /// </summary>
        private TabItem _contextMenuSource;
        public TabControlEx()
        {
            InitializeComponent();
        }
        private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TabItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _contextMenuSource = (sender as Grid).TemplatedParent as TabItem;
            this.menu.PlacementTarget = sender as Grid;
            this.menu.Placement = PlacementMode.MousePoint;
            this.menu.IsOpen = true;
        }

        #region TabItem右键菜单点击事件
        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem btn = e.Source as MenuItem;
            int data = Convert.ToInt32(btn.CommandParameter.ToString());

            if (_contextMenuSource != null)
            {
                List<TabItem> tabItemList = new List<TabItem>();
                if (data == 0)
                {
                    tabItemList.Add(_contextMenuSource);
                }
                if (data == 1)
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        TabItem tabItem = this.Items[i] as TabItem;
                        if (tabItem != _contextMenuSource)
                        {
                            tabItemList.Add(tabItem);
                        }
                    }
                }
                if (data == 2)
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        TabItem tabItem = this.Items[i] as TabItem;
                        if (tabItem != _contextMenuSource)
                        {
                            tabItemList.Add(tabItem);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (data == 3)
                {
                    for (int i = this.Items.Count - 1; i >= 0; i--)
                    {
                        TabItem tabItem = this.Items[i] as TabItem;
                        if (tabItem != _contextMenuSource)
                        {
                            tabItemList.Add(tabItem);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                foreach (TabItem tabItem in tabItemList)
                {
                    CloseTabItem(tabItem);
                }
            }
        }
        #endregion

        private void BtnTabItemClose_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var tmplParent = (btn.Parent as Grid).TemplatedParent;
            var tabItem = tmplParent as TabItem;
            CloseTabItem(tabItem);
        }

        #region 关闭TabItem
        /// <summary>
        /// 关闭TabItem
        /// </summary>
        private void CloseTabItem(TabItem tabItem)
        {
        //    if (tabItem.Content is Page)
        //    {
        //        var content = tabItem.Content as Page;
        //        if (content != null)
        //        {
                 
        //        }
        //        tabItem.Content = null;
        //        content = null;
        //    }
            this.Items.Remove(tabItem);
        }
        #endregion

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (UIElement tabItem in e.RemovedItems)
                {
                    Panel.SetZIndex(tabItem, 99);
                }
                foreach (UIElement tabItem in e.AddedItems)
                {
                    Panel.SetZIndex(tabItem, 999);
                }
            }
            catch
            {

            }
        }
    }
}
