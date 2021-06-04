using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBus
{
    /// <summary>
    /// 消息总线类型
    /// </summary>
    public enum MessageBusType
    {
        /// <summary>
        /// 内存
        /// </summary>
        Memory = 0,
        RabbitMQ,
        ActiveMQ
    }
}