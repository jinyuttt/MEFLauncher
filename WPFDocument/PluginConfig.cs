#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：WPFRibbon
* 项目描述 ：
* 类 名 称 ：PluginConfig
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

using System.Collections.Generic;
using System.Xml.Serialization;

namespace WPFDocument
{
    /* ============================================================================== 
* 功能描述：PluginConfig 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/

    [XmlRootAttribute("DocumentConfig", Namespace = "abc.abc", IsNullable = false)]
    public class PluginConfig
    {
        [XmlArray]
        public List<NodeTab> Tab { get; set; }

        public string Title { get; set; }

        public string LeftFoot { get; set; }

        public string RightFoot { get; set; }

        public string LogoPath { get; set; }
       
    }

    public class NodeTab
    {
       
       
        [XmlAttribute("面板名称")]
        public string Title { get; set; }

        public string Plugin { get; set; }
        public string ImagePath { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsVisible { get; set; }
    }
   

}
