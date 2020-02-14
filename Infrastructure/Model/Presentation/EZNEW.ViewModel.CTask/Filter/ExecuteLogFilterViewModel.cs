using EZNEW.Framework.Paging;
using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Filter
{
    /// <summary>
    /// 任务执行日志查询信息
    /// </summary>
    public class ExecuteLogFilterViewModel : PagingFilter
    {
        #region	筛选数据

        /// <summary>
        /// 编号
        /// </summary>
        public List<long> Ids
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
        public DateTime? BeginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime? RecordTime
        {
            get;
            set;
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        public ExecuteLogStatus? Status
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

        #region 数据加载

        /// <summary>
        /// 加载服务数据信息
        /// </summary>
        public bool LoadServer
        {
            get; set;
        }

        /// <summary>
        /// 加载工作任务
        /// </summary>
        public bool LoadJob
        {
            get; set;
        }

        /// <summary>
        /// 加载执行计划
        /// </summary>
        public bool LoadTrigger
        {
            get; set;
        }

        #endregion
    }
}