using System;
using System.Collections.Generic;
using EZNEW.Develop.CQuery;
using EZNEW.Entity.CTask;

namespace EZNEW.Query.CTask
{
    /// <summary>
    /// 任务异常日志
    /// </summary>
    [QueryEntity(typeof(ErrorLogEntity))]
    public class ErrorLogQuery : QueryModel<ErrorLogQuery>
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
        /// 服务节点
        /// </summary>
        public string Server
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
        /// 错误消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date
        {
            get;
            set;
        }

        #endregion
    }
}