using System;
using System.Collections.Generic;
using EZNEW.Develop.CQuery;
using EZNEW.CTask;
using EZNEW.Entity.CTask;

namespace EZNEW.Query.CTask
{
    /// <summary>
    /// 工作任务
    /// </summary>
    [QueryEntity(typeof(JobEntity))]
    public class JobQuery : QueryModel<JobQuery>
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 分组
        /// </summary>
        public string Group
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        public JobApplicationType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        public JobRunType RunType
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public JobStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 任务路径
        /// </summary>
        public string JobPath
        {
            get;
            set;
        }

        /// <summary>
        /// 任务文件名称
        /// </summary>
        public string JobFileName
        {
            get;
            set;
        }

        #endregion
    }
}