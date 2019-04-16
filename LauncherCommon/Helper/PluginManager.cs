#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：MEFLib
* 项目描述 ：
* 类 名 称 ：PluginManager
* 类 描 述 ：
* 命名空间 ：MEFLib
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

namespace LauncherCommon
{
    public delegate T PluginSearch<T>(object sender, string name,string flage);
    /* ============================================================================== 
* 功能描述：PluginManager 管理所有插件接口
* 创 建 者：jinyu
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
   public class PluginManager
    {
        private static  PluginSearch<object> searchPugin=null;
        private static  PluginSearch<string>  searchConfig= null;
        private static Dictionary<string, object> dicPluginsParams = new Dictionary<string, object>();

        /// <summary>
        /// 获取插件实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static T GetNewObj<T>(string name=null,object sender=null)
        {
           return  (T)searchPugin(sender,name, "GetNewObj");
        }

        /// <summary>
        /// 获取当前已经存在的插件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static T GetCurrentObj<T>(string name = null,object sender=null)
        {
            return (T)searchPugin(sender, name, "GetCurrentObj");
        }

        /// <summary>
        /// 插件查询
        /// </summary>
        /// <param name="pluginSearch"></param>
        public static void Register(PluginSearch<object>  pluginSearch)
        {
            searchPugin += pluginSearch;
        }

        /// <summary>
        /// 添加配置查询
        /// </summary>
        /// <param name="search"></param>
        public static void RegisterConfig(PluginSearch<string> search)
        {
            searchConfig += search;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfig(string name)
        {
           return searchConfig(null, name, "");
        }

        /// <summary>
        /// 获取插件可以共享的配置
        /// 一般来自公共配置插件
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public static object GetParams(string pluginName)
        {
             object obj = null;
             dicPluginsParams.TryGetValue(pluginName, out obj);
            return obj;
        }

        /// <summary>
        /// 注入公共配置
        /// </summary>
        /// <param name="param"></param>
        public static void AddPluginsParams(Dictionary<string,object> param)
        {
            if(param==null||param.Count==0)
            {
                return;
            }
            else
            {
               foreach(var kv in param)
                {
                    dicPluginsParams[kv.Key] = kv.Value;
                }
            }
        }
    }
}
