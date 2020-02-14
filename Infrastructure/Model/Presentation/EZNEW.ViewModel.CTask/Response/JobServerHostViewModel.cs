using EZNEW.CTask;
using System;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    public class JobServerHostViewModel
    {
        #region	属性

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNodeViewModel Server
        {
            get;
            set;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public JobViewModel Job
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerStatus RunStatus
        {
            get;
            set;
        }

        #endregion
    }
}