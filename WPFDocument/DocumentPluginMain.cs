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

using System.IO;
using CommonTools;
using LauncherCommon;
using MEFLib;

namespace WPFDocument
{
    /* ============================================================================== 
* 功能描述：PluginMain Document主插件控制器
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class DocumentPluginMain : IMainView
    {
        readonly MainWindow frmMain = null;
        private string name = "Document";

        public event MainViewExit ViewExitEvent;

        public string ViewName { get { return name; } set { name = value; } }

        public string DisplayName { get ; set ; }

        public DocumentPluginMain()
        {
          
             frmMain = new MainWindow();
            frmMain.Closed += FrmMain_Closed;
            Init();

        }

        private void TestConfig()
        {
            PluginConfig config = new PluginConfig();
            config.Tab = new System.Collections.Generic.List<NodeTab>();
            for (int i=0;i<5;i++)
            {
                NodeTab node = new NodeTab() { Title = "测试" + i };
                node.ImagePath = "Imge";
                node.Plugin = "PluginName";
                config.Tab.Add(node);
            }
            XmlUtility.SerializeWrite<PluginConfig>(config, "documentconfig.xml");
        }
        
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
        }

        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <param name="config"></param>
        private void PluginInit(PluginConfig config)
        {
            foreach (var item in config.Tab)
            {
                var p = PluginManager.GetNewObj<IView>(item.Plugin);
                object view = null;
                if (p != null)
                {
                    view = p.GetView(item.Title);

                }
                //
                if (string.IsNullOrEmpty(item.ImagePath)||!File.Exists(item.ImagePath))
                {
                    item.ImagePath =null;
                }
                frmMain.AddControl(item.Title, view, item.ImagePath);
            }
        }

        private void FrmMain_Closed(object sender, System.EventArgs e)
        {
            if (ViewExitEvent != null)
            {
                ViewExitEvent(this, name);
            }
        }

        public void Close()
        {
            frmMain.Close();
        }

        public void Show()
        {
            frmMain.ShowDialog();
        }
    }
}
