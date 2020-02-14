using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 执行日志保存对象
    /// </summary>
    public class ExecuteLogCmdDto
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
        public JobCmdDto Job
        {
            get;
            set;
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        public TriggerCmdDto Trigger
        {
            get;
            set;
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        public ServerNodeCmdDto Server
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
