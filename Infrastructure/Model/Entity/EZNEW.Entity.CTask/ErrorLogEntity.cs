using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务异常日志
    /// </summary>
    [Serializable]
    [Entity("CTask_ErrorLog", "CTask", "任务异常日志")]
    public class ErrorLogEntity : BaseEntity<ErrorLogEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public long Id
        {
            get { return GetPropertyValue<long>("Id"); }
            set { SetPropertyValue("Id", value); }
        }

        /// <summary>
        /// 服务节点
        /// </summary>
        [EntityField(Description = "服务节点")]
        public string Server
        {
            get { return GetPropertyValue<string>("Server"); }
            set { SetPropertyValue("Server", value); }
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        [EntityField(Description = "工作任务")]
        public string Job
        {
            get { return GetPropertyValue<string>("Job"); }
            set { SetPropertyValue("Job", value); }
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        [EntityField(Description = "执行计划")]
        public string Trigger
        {
            get { return GetPropertyValue<string>("Trigger"); }
            set { SetPropertyValue("Trigger", value); }
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        [EntityField(Description = "错误消息")]
        public string Message
        {
            get { return GetPropertyValue<string>("Message"); }
            set { SetPropertyValue("Message", value); }
        }

        /// <summary>
        /// 错误描述
        /// </summary>
        [EntityField(Description = "错误描述")]
        public string Description
        {
            get { return GetPropertyValue<string>("Description"); }
            set { SetPropertyValue("Description", value); }
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        [EntityField(Description = "错误类型")]
        public int Type
        {
            get { return GetPropertyValue<int>("Type"); }
            set { SetPropertyValue("Type", value); }
        }

        /// <summary>
        /// 时间
        /// </summary>
        [EntityField(Description = "时间")]
        public DateTime Date
        {
            get { return GetPropertyValue<DateTime>("Date"); }
            set { SetPropertyValue("Date", value); }
        }

        #endregion
    }
}