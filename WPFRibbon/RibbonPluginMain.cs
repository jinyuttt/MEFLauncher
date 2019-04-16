#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：WPFRibbon
* 项目描述 ：
* 类 名 称 ：PluginMain
* 类 描 述 ：
* 命名空间 ：WPFRibbon
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using CommonTools;
using LauncherCommon;
using MEFLib;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace WPFRibbon
{
    public delegate void ShowPlugin(string name, string title);
    /* ============================================================================== 
* 功能描述：PluginMain Ribbon主插件控制器
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class RibbonPluginMain : IMainView
    {
        readonly MainWindow frmMain = null;
        private string name = "Ribbon";
       
        public event MainViewExit ViewExitEvent;

        public string ViewName { get { return name; } set { name = value; } }

        public string DisplayName { get ; set; }

        private Dictionary<string, string> dicPlugins = null;
        
        public RibbonPluginMain()
        {
            dicPlugins = new Dictionary<string, string>();
            frmMain = new MainWindow();
            frmMain.Closed += FrmMain_Closed;
            Init();
        }

        private void FrmMain_Closed(object sender, System.EventArgs e)
        {
            if(ViewExitEvent!=null)
            {
                ViewExitEvent(this, name);
            }
        }

        private void TestConfig()
        {
            PluginConfig config = new PluginConfig();
            config.Tab = new List<NodeTab>();
            for (int k = 0; k < 5; k++)
            {
                NodeTab tab = new NodeTab();
                tab.Groups = new List<NodeGroup>();
                tab.IsVisible = true;
                tab.IsEnabled = true;
                tab.Title = "测试Tab" + k;
                tab.PluginName = "";
                for (int j = 0; j < 5; j++)
                {

                    NodeGroup group = new NodeGroup();
                    group.Title = "测试Gropu" + j;
                    group.IsEnabled = true;
                    group.IsVisible = true;
                    group.PluginName = "";
                    group.Nodes = new List<NodeButton>();
                    for (int i = 0; i < 5; i++)
                    {
                        NodeButton button = new NodeButton() { Name = "测试" + i };
                        button.IsVisible = true;
                        button.IsEnabled = true;
                        button.PluginName = "";
                        group.Nodes.Add(button);
                    }
                    tab.Groups.Add(group);
                }
                config.Tab.Add(tab);
            }
           
            XmlUtility.SerializeWrite<PluginConfig>(config, "ribbonconfig.xml");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            try
            {
               //TestConfig();
                string file = PluginManager.GetConfig("PluginConfig");
                if (!string.IsNullOrEmpty(file))
                {
                    PluginConfig config = XmlUtility.DeserializeFileToObject<PluginConfig>(file);
                    PluginInit(config);
                }
            }
            catch
            {

            }
            frmMain.PluginEventHandler += FrmMain_PluginEventHandler;
        }

        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="config"></param>
        private void PluginInit(PluginConfig config)
        {
            foreach (var tab in config.Tab)
            {
                if (tab.IsVisible)
                {
                    int index = frmMain.AddTabGroup(tab.Title);
                    int groupIndex = -1;
                    RibbonPluginIndex.dicTab[tab.Title] = index;
                   
                    foreach (var group in tab.Groups)
                    {
                        if (group.IsVisible)
                        {
                            groupIndex = frmMain.AddGroup(index, group.Title);
                            foreach (var button in group.Nodes)
                            {
                                if (button.IsVisible)
                                {
                                    frmMain.AddButton(index, groupIndex, button.Name);
                                }

                                //插件名称逐级替换
                                dicPlugins[button.Name] = tab.PluginName;
                                if (!string.IsNullOrEmpty(group.PluginName))
                                {
                                    dicPlugins[button.Name] = group.PluginName;
                                }
                                if (!string.IsNullOrEmpty(button.PluginName))
                                {
                                    dicPlugins[button.Name] = button.PluginName;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void FrmMain_PluginEventHandler(string name, string title)
        {
            string pluginName = "";
            object control = null;
            dicPlugins.TryGetValue(title, out pluginName);
           
            if(string.IsNullOrEmpty(pluginName))
            {
                pluginName = "";
            }
            IView view=  PluginManager.GetCurrentObj<IView>(pluginName);
            if (view != null)
            {
                 control =view.GetView(title);
            }
            frmMain.AddControl(title,control);
        }

        public void Close()
        {
            frmMain.Close();
           
        }

        public void Show()
        {
            if (!string.IsNullOrEmpty(DisplayName))
            {
                frmMain.Title = DisplayName;
            }
            frmMain.ShowDialog();
        }


    }
}
