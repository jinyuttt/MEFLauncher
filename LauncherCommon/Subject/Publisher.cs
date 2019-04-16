#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：LauncherCommon.Subject
* 项目描述 ：
* 类 名 称 ：Publisher
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
    /* ============================================================================== 
* 功能描述：Publisher 
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public  class Publisher
    {

        /// <summary>
        /// 发布数据
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="info">信息</param>
        /// <param name="isAsyn">是否异步发送</param>
        public void Publish(string topic, object info,bool isAsyn=false)
        {
            if(isAsyn)
            {
                TopicList.PublishAsyn(topic, info);
            }
            else
            {
                TopicList.Publish(topic, info);
            }
            
        }
        
    }
}
