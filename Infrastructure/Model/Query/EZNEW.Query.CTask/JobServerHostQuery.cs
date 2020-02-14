using System;
using System.Collections.Generic;
using EZNEW.Develop.CQuery;
using EZNEW.Entity.CTask;

namespace EZNEW.Query.CTask
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    [QueryEntity(typeof(JobServerHostEntity))]
    public class JobServerHostQuery : QueryModel<JobServerHostQuery>
    {
        #region	属性

        /// <summary>
        /// 服务
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public string Job
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int RunStatus
        {
            get;
            set;
        }

        #endregion
    }
}