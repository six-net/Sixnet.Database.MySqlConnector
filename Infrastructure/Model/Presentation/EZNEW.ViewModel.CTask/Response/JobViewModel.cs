using EZNEW.CTask;
using System;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 工作任务
    /// </summary>
    public class JobViewModel
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
        public JobGroupViewModel Group
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
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
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
        /// 状态名称
        /// </summary>
        public string StateName
        {
            get
            {
                return Status.ToString();
            }
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