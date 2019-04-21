#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：LauncherCommon
* 项目描述 ：
* 类 名 称 ：LauncherHelper
* 类 描 述 ：
* 命名空间 ：LauncherCommon
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using LauncherCommon;
using MEFLib;
using MEFLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MEFLauncher
{
    /* ============================================================================== 
* 功能描述：LauncherHelper 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class LauncherHelper
    {
        static  AutoResetEvent resetEvent = null;
        /// <summary>
        /// 配置文件内容
        /// </summary>
        static Dictionary<string, string> dicConfig = new Dictionary<string, string>();

        /// <summary>
        /// 配置插件名称
        /// </summary>
        static Dictionary<string, string> dicPlugin = new Dictionary<string, string>();
         
        /// <summary>
        /// 主插件
        /// </summary>
        static IMainView MainView;

        /// <summary>
        /// 当前插件实例
        /// </summary>
        static Dictionary<string, object> dicPluginObj = new Dictionary<string, object>();
        static string[] lstPluginPath = null;

        /// <summary>
        /// 启动插件
        /// </summary>
       public  static void Run(AutoResetEvent consoleEvent)
        {
            resetEvent = consoleEvent;
            Read();
            PluginManager.Register(GetPlugin);
            PluginManager.RegisterConfig(GetConfig);
            PluginDirectory();
            Thread plugin = new Thread(RunPlugin);
            plugin.IsBackground = true;
            plugin.Name = "Plugin";

            Thread frame = new Thread(() =>
            {
                LoadView();
                string view = null;
                dicConfig.TryGetValue("MainPlugin", out view);
                if (string.IsNullOrEmpty(view))
                {
                    view = "Document";
                }
                try
                {
                    IMainView mainView = null;
                    if(lstPluginPath==null)
                    {
                        CatalogLoader.Singleton.DefaultLoader<IMainView>();
                    }
                    else
                    {
                        CatalogLoader.Singleton.DirectoryCatalogLoader<IMainView>(lstPluginPath);
                    }
                  
                    var objs = CatalogLoader.Singleton.GetList<IMainView>();
                    plugin.Start();//启动线程加载其它插件
                    if (objs.Count == 1)
                    {
                        mainView = objs[0];
                    }
                    else
                    {
                        //Ribbon Document
                        //
                        if(!string.IsNullOrEmpty(view))
                        {
                            view = view.Trim();
                        }
                        var obj = objs.Find(p => p.ViewName.Trim() == view);
                        mainView = obj;

                    }
                    if (mainView != null)
                    {
                        dicPlugin[mainView.ViewName] = typeof(IMainView).Name;
                        string title = null;
                        dicConfig.TryGetValue("Title", out title);
                        mainView.DisplayName = title;
                        MainView = mainView;

                        mainView.ViewExitEvent += MainView_ViewExitEvent;
                        mainView.Show();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            frame.SetApartmentState(ApartmentState.STA);
            frame.IsBackground = true;
            frame.Start();
        }

        /// <summary>
        /// 加载其它插件
        /// </summary>
        static void RunPlugin()
        {
            StartArgs();
            LoadPlugin();
            StartPlugin();
        }

        /// <summary>
        /// 处理路径
        /// </summary>
        static void PluginDirectory()
        {
            string path = null;
            if (dicConfig.ContainsKey("PluginPath"))
            {
                path = dicConfig["PluginPath"];
                if (path != null)
                {
                    path = path.Trim();
                }
            }
            if (!string.IsNullOrEmpty(path))
            {
                List<string> lst = new List<string>();
                string[]temp = path.Split(',');
                foreach(string k in temp)
                {
                    if(string.IsNullOrEmpty(k)||k.Trim().Length==0)
                    {
                        continue;
                    }
                    if (Directory.Exists(k)|| Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,k)))
                    {
                        lst.Add(k);
                    }
                   
                }
                if(lst.Count>0)
                {
                    lstPluginPath = lst.ToArray();
                }
            }
            
        }

        /// <summary>
        /// 加载业务插件
        /// </summary>
        static void LoadPlugin()
        {

            if (lstPluginPath == null)
            {
                CatalogLoader.Singleton.DefaultLoader<IPlugin>();

            }
            else
            {
                CatalogLoader.Singleton.DirectoryCatalogLoader<IPlugin>(lstPluginPath);
            }
            var plugins = CatalogLoader.Singleton.GetList<IPlugin>();
            if (plugins != null)
            {
                foreach (var plugin in plugins)
                {
                    if (!string.IsNullOrEmpty(plugin.PuginName))
                    {
                        dicPlugin[plugin.PuginName] = typeof(IPlugin).Name;
                    }

                }
            }
        }

       /// <summary>
       /// 加载View插件
       /// </summary>
        static void LoadView()
        {
            //
            if (lstPluginPath == null)
            {
                CatalogLoader.Singleton.DefaultLoader<IView>();
            }
            else
            {
                CatalogLoader.Singleton.DirectoryCatalogLoader<IView>(lstPluginPath);
            }
            var views = CatalogLoader.Singleton.GetList<IView>();
            if (views != null)
            {
                foreach (var plugin in views)
                {
                    dicPlugin[plugin.ViewName] = typeof(IView).Name;
                }
            }
            //
        }

        /// <summary>
        /// 启动初始化插件
        /// </summary>
        static void StartArgs()
        {
            if (lstPluginPath == null)
            {
                CatalogLoader.Singleton.DefaultLoader<IArgs>();
            }
            else
            {
                CatalogLoader.Singleton.DirectoryCatalogLoader<IArgs>(lstPluginPath);
            }
            var plugins = CatalogLoader.Singleton.GetList<IArgs>();

            if (plugins != null)
            {
                string[] order = null;
                List<string> lst = new List<string>();
                if (dicConfig.ContainsKey("PluginArgsOrder"))
                {
                    string argPlugin = dicConfig["PluginArgsOrder"];
                    order = argPlugin.Trim().Split(',');
                }
                if (order != null)
                {
                    foreach (string k in order)
                    {
                        var cur = plugins.Find((p) => p.PuginName == k);
                        if (cur != null)
                        {
                            cur.Init();
                        }
                    }
                    lst.AddRange(order);
                }
                foreach (var plugin in plugins)
                {
                    dicPlugin[plugin.PuginName] = typeof(IArgs).Name;
                    if (!lst.Contains(plugin.PuginName))
                    {
                        plugin.Init();
                    }
                }
            }
        }

       /// <summary>
       /// 按照配置顺序启动插件
       /// </summary>
        static void StartPlugin()
        {
            string[] order = null;
            List<string> lst = new List<string>();
            if (dicConfig.ContainsKey("PluginBusinessOrder"))
            {
                string argPlugin = dicConfig["PluginBusinessOrder"];
                order = argPlugin.Trim().Split(',');
            }
            if (order != null)
            {
                var plugins = CatalogLoader.Singleton.GetList<IPlugin>();
                foreach (string cur in order)
                {
                    var plugin = plugins.Find(p => p.PuginName == cur);
                    if (plugin != null)
                    {
                        plugin.Start();
                    }
                }
            }
        }

        /// <summary>
        /// 读取启动器的配置
        /// </summary>
        static void Read()
        {
            using (StreamReader rd = new StreamReader("config.txt"))
            {
                while (rd.Peek() != -1)
                {
                    string line = rd.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Trim() == "#")
                        {
                            continue;
                        }
                        string[] p = line.Split('=');
                        if (p.Length == 2)
                        {
                            dicConfig[p[0]] = p[1];
                        }
                    }
                }
            }
        }

       /// <summary>
       /// 通过启动器获取全部插件
       /// 启动与插件管理器沟通
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="name"></param>
       /// <param name="flage"></param>
       /// <returns></returns>
        private static object GetPlugin(object sender, string name, string flage)
        {
            //根据名称获取插件
            object obj = null;
            if (dicPlugin.ContainsKey(name))
            {
                string PName = dicPlugin[name];
                switch (PName)
                {
                    case "IMainView":
                        obj = MainView;
                        break;
                    case "IView":
                        if ("GetCurrentObj" == flage)
                        {
                            if (dicPluginObj.ContainsKey(name))
                            {
                                obj = dicPluginObj[name];
                            }
                            else
                            {
                                var lst = CatalogLoader.Singleton.GetList<IView>();
                                obj = lst.Find(p => p.ViewName == name);
                                if (obj != null)
                                {
                                    dicPluginObj[name] = obj;
                                }
                            }
                        }
                        else
                        {
                            var lst = CatalogLoader.Singleton.GetList<IView>();
                            obj = lst.Find(p => p.ViewName == name);
                            if (obj != null)
                            {
                                dicPluginObj[name] = obj;
                            }
                        }
                        break;
                    case "IPlugin":
                        if ("GetCurrentObj" == flage)
                        {
                            if (dicPluginObj.ContainsKey(name))
                            {
                                obj = dicPluginObj[name];
                            }
                            else
                            {
                                var lst = CatalogLoader.Singleton.GetList<IPlugin>();
                                obj = lst.Find(p => p.PuginName == name);
                                if (obj != null)
                                {
                                    dicPluginObj[name] = obj;
                                }
                            }
                        }
                        else
                        {
                            var lst = CatalogLoader.Singleton.GetList<IPlugin>();
                            obj = lst.Find(p => p.PuginName == name);
                            if (obj != null)
                            {
                                dicPluginObj[name] = obj;
                            }
                        }
                        break;
                    case "IArgs":
                        if ("GetCurrentObj" == flage)
                        {
                            if (dicPluginObj.ContainsKey(name))
                            {
                                obj = dicPluginObj[name];
                            }
                            else
                            {
                                var lst = CatalogLoader.Singleton.GetList<IArgs>();
                                obj = lst.Find(p => p.PuginName == name);
                                if (obj != null)
                                {
                                    dicPluginObj[name] = obj;
                                }
                            }
                        }
                        else
                        {
                            var lst = CatalogLoader.Singleton.GetList<IArgs>();
                            obj = lst.Find(p => p.PuginName == name);
                            if (obj != null)
                            {
                                dicPluginObj[name] = obj;
                            }
                        }
                        break;

                }
            }
            return obj;

        }

        /// <summary>
        /// 获取启动器的配置内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="name"></param>
        /// <param name="flage"></param>
        /// <returns></returns>
        private static string GetConfig(object sender, string name, string flage)
        {
            return dicConfig[name.ToString()];
        }

        /// <summary>
        /// 主View退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        private static void MainView_ViewExitEvent(object sender, object info = null)
        {
            resetEvent.Set();
        }

    }
}
