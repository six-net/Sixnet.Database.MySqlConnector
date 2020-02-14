using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Filter
{
    /// <summary>
    /// 任务异常日志查询信息
    /// </summary>
    public class ErrorLogFilterViewModel : PagingFilter
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public List<long> Ids
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
        public int? Type
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get;set;
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