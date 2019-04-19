#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：MEFLoader
* 项目描述 ：
* 类 名 称 ：CatalogLoader
* 类 描 述 ：
* 命名空间 ：MEFLoader
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MEFLoader
{
    /* ============================================================================== 
* 功能描述：CatalogLoader 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class CatalogLoader
    {
        
        private readonly Dictionary<string, CompositionHost> dic = new Dictionary<string, CompositionHost>();
        private static readonly Lazy<CatalogLoader> Instance = new Lazy<CatalogLoader>();

        /// <summary>
        /// 单例
        /// </summary>
        public static CatalogLoader Singleton
        {
            get { return Instance.Value; }
        }

        /// <summary>
        /// 默认加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DefaultLoader<T>()
        {
            if (dic.ContainsKey(typeof(T).FullName))
            {
                return;
            }
            var files = Directory.GetFiles(AppContext.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            List<Assembly> assembiles = new List<Assembly>();
            foreach(var file in files)
            {
                try
                {
                    assembiles.Add(Assembly.LoadFrom(file));
                }catch
                {

                }
            }
            
            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom<T>()
                .Export<T>();
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assembiles, conventions);
            var  container = configuration.CreateContainer();
           
            dic[typeof(T).FullName] = container;
        }

        /// <summary>
        /// 默认加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DefaultLoader(Type type)
        {
            if (dic.ContainsKey(type.FullName))
            {
                return;
            }
            var files = Directory.GetFiles(AppContext.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            List<Assembly> assembiles = new List<Assembly>();
            foreach (var file in files)
            {
                try
                {
                    assembiles.Add(Assembly.LoadFrom(file));
                }
                catch
                {

                }
            }

            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom(type).Export();
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assembiles, conventions);
            var container = configuration.CreateContainer();

            dic[type.FullName] = container;
        }

        /// <summary>
        /// 通用加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dirs"></param>
        public void DirectoryCatalogLoader<T>(string[] dirs)
        {
            if(dic.ContainsKey(typeof(T).FullName))
            {
                return;
            }
            List<Assembly> assembiles = new List<Assembly>();
            List<string> lstDir = dirs.ToList();
            if(!lstDir.Contains(AppContext.BaseDirectory))
            {
                lstDir.Add(AppContext.BaseDirectory);
            }
            foreach (string dir in lstDir)
            {
                var files = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    try
                    {
                        assembiles.Add(Assembly.LoadFrom(file));
                    }
                    catch
                    {

                    }
                }
            }
            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom<T>()
                .Export<T>();
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assembiles, conventions);

           var container = configuration.CreateContainer();
            dic[typeof(T).FullName] = container;
        }

        /// <summary>
        /// 通用加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dirs"></param>
        public void DirectoryCatalogLoader(Type type,string[] dirs)
        {
            if (dic.ContainsKey(type.FullName))
            {
                return;
            }
            List<Assembly> assembiles = new List<Assembly>();
            List<string> lstDir = dirs.ToList();
            if (!lstDir.Contains(AppContext.BaseDirectory))
            {
                lstDir.Add(AppContext.BaseDirectory);
            }
            foreach (string dir in lstDir)
            {
                var files = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    try
                    {
                        assembiles.Add(Assembly.LoadFrom(file));
                    }
                    catch
                    {

                    }
                }
            }
            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom(type).Export();
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assembiles, conventions);
            var container = configuration.CreateContainer();
            dic[type.FullName] = container;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetObj<T>()
        {
            T obj = default(T);
            CompositionHost container = null;
            if (dic.TryGetValue(typeof(T).FullName, out container))
            {
                obj=  container.GetExport<T>();
            }
            return obj;
        }

        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetList<T>()
        {
            List<T> lst = new List<T>();
            CompositionHost container = null;
            if (dic.TryGetValue(typeof(T).FullName, out container))
            {
                lst.AddRange(container.GetExports<T>());
            }
            return lst;
        
        }

    }
}
