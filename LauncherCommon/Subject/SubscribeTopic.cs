#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：LauncherCommon.Subject
* 项目描述 ：
* 类 名 称 ：Subscribe
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


namespace LauncherCommon.Subject
{
    /// <summary>
    /// 委托充当订阅者接口类
    /// </summary>
    /// <param name="sender"></param>
    public delegate void NotifyEventHandler(object sender);

    /* ============================================================================== 
* 功能描述：SubscribeTopic 一个主题信息
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    internal class SubscribeTopic
    {
        
        /// <summary>
        /// 订阅的委托
        /// </summary>
        private NotifyEventHandler NotifyEvent;

        /// <summary>
        /// 主题标识
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///信息
        /// </summary>
        public object Info { get; set; }

       
        public SubscribeTopic(string name, object info)
        {
            this.Name = name;
            this.Info = info;
        }

        /// <summary>
        /// 绑定订阅事件
        /// </summary>
        /// <param name="ob">订阅者</param>
        public void AddObserver(NotifyEventHandler observer)
        {
            NotifyEvent += observer;
        }

        /// <summary>
        /// 取消绑定订阅
        /// </summary>
        /// <param name="observer"></param>
        public void RemoveObserver(NotifyEventHandler observer)
        {
            NotifyEvent -= observer;
        }

        /// <summary>
        /// 发布通知
        /// </summary>
        public void PublishInfo()
        {
            if (NotifyEvent != null)
            {
                NotifyEvent(this);
            }
        }
    }
}
