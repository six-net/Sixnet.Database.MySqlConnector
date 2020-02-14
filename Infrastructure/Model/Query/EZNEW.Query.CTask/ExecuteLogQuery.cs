using System;
using System.Collections.Generic;
using EZNEW.Develop.CQuery;
using EZNEW.CTask;
using EZNEW.Entity.CTask;

namespace EZNEW.Query.CTask
{
    /// <summary>
    /// 任务执行日志
    /// </summary>
    [QueryEntity(typeof(ExecuteLogEntity))]
    public class ExecuteLogQuery : QueryModel<ExecuteLogQuery>
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        public string Job
        {
            get;
            set;
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        public string Trigger
        {
            get;
            set;
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime
        {
            get;
            set;
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        public ExecuteLogStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        #endregion
    }
}