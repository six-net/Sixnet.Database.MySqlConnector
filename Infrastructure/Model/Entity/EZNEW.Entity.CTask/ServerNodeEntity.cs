using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 服务节点
    /// </summary>
    [Serializable]
    [Entity("CTask_ServerNode", "CTask", "服务节点")]
    public class ServerNodeEntity : BaseEntity<ServerNodeEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string Id
        {
            get { return GetPropertyValue<string>("Id"); }
            set { SetPropertyValue("Id", value); }
        }

        /// <summary>
        /// 实例名称
        /// </summary>
        [EntityField(Description = "实例名称")]
        public string InstanceName
        {
            get { return GetPropertyValue<string>("InstanceName"); }
            set { SetPropertyValue("InstanceName", value); }
        }

        /// <summary>
        /// 节点名称
        /// </summary>
        [EntityField(Description = "节点名称")]
        public string Name
        {
            get { return GetPropertyValue<string>("Name"); }
            set { SetPropertyValue("Name", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityField(Description = "状态")]
        public int Status
        {
            get { return GetPropertyValue<int>("Status"); }
            set { SetPropertyValue("Status", value); }
        }

        /// <summary>
        /// 地址
        /// </summary>
        [EntityField(Description = "地址")]
        public string Host
        {
            get { return GetPropertyValue<string>("Host"); }
            set { SetPropertyValue("Host", value); }
        }

        /// <summary>
        /// 线程数量
        /// </summary>
        [EntityField(Description = "线程数量")]
        public int ThreadCount
        {
            get { return GetPropertyValue<int>("ThreadCount"); }
            set { SetPropertyValue("ThreadCount", value); }
        }

        /// <summary>
        /// 线程优先级
        /// </summary>
        [EntityField(Description = "线程优先级")]
        public string ThreadPriority
        {
            get { return GetPropertyValue<string>("ThreadPriority"); }
            set { SetPropertyValue("ThreadPriority", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Description
        {
            get { return GetPropertyValue<string>("Description"); }
            set { SetPropertyValue("Description", value); }
        }

        #endregion
    }
}