#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：LauncherCommon.Subject
* 项目描述 ：
* 类 名 称 ：Subscriber
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

using System.Collections.Generic;

namespace LauncherCommon.Subject
{
    public delegate void SubscribeNotifyHandler(object sender,string topic, object info);
    /* ============================================================================== 
* 功能描述：Subscriber 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class Subscriber
    {
        Dictionary<string, Observer> dicTopic = new Dictionary<string, Observer>();
        public event SubscribeNotifyHandler TopicDataNotify;
        private object lock_obj = new object();

        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <param name="topic"></param>
        public void Subscrib(string topic)
        {
            //所有订阅必须方法同步
            lock (lock_obj)
            {
                if (!dicTopic.ContainsKey(topic))
                {
                    Observer observer = new Observer(topic);//订阅该主题
                    TopicList.Subscribe(topic, observer.Receive);
                    dicTopic[topic] = observer;
                    observer.notifyHandler += Receive;
                }
            }
        }

        /// <summary>
        /// 移除主题订阅
        /// </summary>
        /// <param name="topic"></param>
        public void RemoveTopic(string topic)
        {
            lock (lock_obj)
            {
                Observer observer = null;
                if (dicTopic.TryGetValue(topic,out observer))
                {
                    dicTopic.Remove(topic);
                    TopicList.RemoveSubscribe(topic, observer.Receive);
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        private void Receive(object sender,object info)
        {
            Observer observer = sender as Observer;
            if(observer!=null&& TopicDataNotify!=null)
            {
                TopicDataNotify(this, observer.Name, info);
            }
        }


    }
}
