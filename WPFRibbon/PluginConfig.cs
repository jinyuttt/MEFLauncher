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

namespace WPFRibbon
{
    /* ============================================================================== 
* 功能描述：PluginConfig 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/

    [XmlRootAttribute("RibbonConfig", Namespace = "abc.abc", IsNullable = false)]
    public class PluginConfig
    {
        [XmlArray]
        public List<NodeTab> Tab { get; set; }
       
    }

    public class NodeTab
    {
        [XmlArray]
        public List<NodeGroup> Groups { get; set; }
        [XmlAttribute("面板名称")]
        public string Title { get; set; }
        public string PluginName { get; set; }
        public bool IsEnabled { get; set; }

        public bool IsVisible { get; set; }
    }
    
    public class NodeGroup
    {
        [XmlAttribute("功能区名称")]
        public string Title { get; set; }
        [XmlArray]
        public List<NodeButton> Nodes { get; set; }
        public string PluginName { get; set; }
        public bool IsEnabled { get; set; }

        public bool IsVisible { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NodeButton
    {
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// img路径
        /// </summary>
        public string ImagePath { get; set; }

        
        public string PluginName { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsVisible { get; set; }
    }
}
