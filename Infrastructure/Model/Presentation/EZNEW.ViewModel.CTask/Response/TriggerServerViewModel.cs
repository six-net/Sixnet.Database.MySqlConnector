using EZNEW.CTask;
using System;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 服务节点执行计划
    /// </summary>
    public class TriggerServerViewModel
    {
        #region	属性

        /// <summary>
        /// 触发器
        /// </summary>
        public TriggerViewModel Trigger
        {
            get;
            set;
        }

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNodeViewModel Server
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public TaskTriggerServerRunStatus RunStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime LastFireDate
        {
            get;
            set;
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireDate
        {
            get;
            set;
        }

        #endregion
    }
}