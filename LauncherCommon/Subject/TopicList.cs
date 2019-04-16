#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：LauncherCommon.Subject
* 项目描述 ：
* 类 名 称 ：TopicList
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
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace LauncherCommon.Subject
{
    /* ============================================================================== 
* 功能描述：TopicList 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    internal class TopicList
    {
        static readonly Dictionary<string, Topic> topic = new Dictionary<string, Topic>();
        static readonly ConcurrentQueue<Message> queue = new ConcurrentQueue<Message>();
        private static object lock_obj = new object();
        private static int state = 0;//异步线程运行；
        /// <summary>
        /// 同步发布数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="info"></param>
        public static void Publish(string name,object info)
        {
            Topic subscriber = null;
            if (topic.TryGetValue(name, out subscriber))
            {
                subscriber.Info = info;
                subscriber.PublishInfo();
            }
        }

        private static void Process()
        {
            Task.Factory.StartNew(() =>
            {
                Message message;
                while(!queue.IsEmpty)
                {
                    if(queue.TryDequeue(out message))
                    {
                        Publish(message.name, message.info);
                    }
                }
                //
                Interlocked.CompareExchange(ref state,0, 1);
            });
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="name"></param>
        /// <param name="info"></param>
        public static void PublishAsyn(string name, object info)
        {
            queue.Enqueue(new Message { name = name, info = info });
            if(state==0)
            {
                if(0==Interlocked.Exchange(ref state,1))
                {
                    Process();
                }
              
            }
        }
       
        
        
        /// <summary>
        /// 订阅数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="notifyEvent"></param>
        public static void Subscribe(string name, NotifyEventHandler notifyEvent)
        {
            //必须方法同步
            lock (lock_obj)
            {
                Topic subscriber = null;
                if (topic.TryGetValue(name, out subscriber))
                {
                    subscriber.AddObserver(notifyEvent);
                }
                else
                {
                    //说明是先订阅
                    subscriber = new Topic(name, null);
                    subscriber.AddObserver(notifyEvent);
                    topic[name] = subscriber;

                }
            }
        }

        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <param name="name"></param>
        /// <param name="notifyEvent"></param>
        public static void RemoveSubscribe(string name, NotifyEventHandler notifyEvent)
        {
            //必须方法同步
            lock (lock_obj)
            {
                Topic subscriber = null;
                if (topic.TryGetValue(name, out subscriber))
                {
                    subscriber.RemoveObserver(notifyEvent);
                }
            }
        }


    }

    /// <summary>
    /// 存储消息
    /// </summary>
    public class Message
    {
        public string name;
        public object info;
    }
}
