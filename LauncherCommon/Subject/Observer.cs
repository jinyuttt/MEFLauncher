#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：LauncherCommon.Subject
* 项目描述 ：
* 类 名 称 ：Observer
* 类 描 述 ：
* 命名空间 ：LauncherCommon.Subject
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

namespace LauncherCommon.Subject
{
    public delegate void TopicNotifyHandler(object sender, object info);
    /* ============================================================================== 
* 功能描述：Observer  订阅主题
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    class Observer
    {
        /// <summary>
        /// 接收数据通知
        /// </summary>
        public TopicNotifyHandler notifyHandler = null;
        /// <summary>
        /// 订阅者名字
        /// </summary>
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 订阅者构造函数
        /// </summary>
        /// <param name="name">订阅者名字</param>
        public Observer(string name)
        {
            this.m_Name = name;
        }

        /// <summary>
        /// 订阅者接受函数
        /// </summary>
        /// <param name="blog"></param>
        public void Receive(object obj)
        {
            //接收
            if (notifyHandler != null)
            {
                notifyHandler(this,obj);
            }
        }
    }
}
