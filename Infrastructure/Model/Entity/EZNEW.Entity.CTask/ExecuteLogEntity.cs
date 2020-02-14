using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务执行日志
    /// </summary>
    [Serializable]
    [Entity("CTask_ExecuteLog", "CTask", "任务执行日志")]
    public class ExecuteLogEntity : BaseEntity<ExecuteLogEntity>
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
        /// 执行服务
        /// </summary>
        [EntityField(Description = "执行服务")]
        public string Server
        {
            get { return GetPropertyValue<string>("Server"); }
            set { SetPropertyValue("Server", value); }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EntityField(Description = "开始时间")]
        public DateTime BeginTime
        {
            get { return GetPropertyValue<DateTime>("BeginTime"); }
            set { SetPropertyValue("BeginTime", value); }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        [EntityField(Description = "结束时间")]
        public DateTime EndTime
        {
            get { return GetPropertyValue<DateTime>("EndTime"); }
            set { SetPropertyValue("EndTime", value); }
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        [EntityField(Description = "记录时间")]
        public DateTime RecordTime
        {
            get { return GetPropertyValue<DateTime>("RecordTime"); }
            set { SetPropertyValue("RecordTime", value); }
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        [EntityField(Description = "执行状态")]
        public int Status
        {
            get { return GetPropertyValue<int>("Status"); }
            set { SetPropertyValue("Status", value); }
        }

        /// <summary>
        /// 消息
        /// </summary>
        [EntityField(Description = "消息")]
        public string Message
        {
            get { return GetPropertyValue<string>("Message"); }
            set { SetPropertyValue("Message", value); }
        }

        #endregion
    }
}