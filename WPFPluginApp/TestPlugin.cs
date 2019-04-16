#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：WPFPluginApp
* 项目描述 ：
* 类 名 称 ：TestPlugin
* 类 描 述 ：
* 命名空间 ：WPFPluginApp
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using MEFLib;

namespace WPFPluginApp
{
    /* ============================================================================== 
* 功能描述：TestPlugin 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class TestPlugin : IView
    {
        private string name = "Test";
        public string ViewName { get {  return name; } set{ name = value; } }
        public string DisplayName { get; set; }

        public void Close()
        {
            
        }

        public object GetView(string name)
        {
            return new TestPage();
        }

        public void Show()
        {
           
        }
    }
}
