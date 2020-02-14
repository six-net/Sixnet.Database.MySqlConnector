using System;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 任务异常日志
    /// </summary>
    public class ErrorLogViewModel
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
        public ServerNodeViewModel Server
        {
            get;
            set;
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        public JobViewModel Job
        {
            get;
            set;
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        public TriggerViewModel Trigger
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