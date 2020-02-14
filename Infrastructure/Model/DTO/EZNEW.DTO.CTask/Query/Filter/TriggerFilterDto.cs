using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query.Filter
{
    /// <summary>
    /// 任务执行计划查询信息
    /// </summary>
    public class TriggerFilterDto : PagingFilter
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public List<string> Ids
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
        /// 说明
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 所属任务
        /// </summary>
        public string Job
        {
            get;
            set;
        }

        /// <summary>
        /// 应用到对象
        /// </summary>
        public int? ApplyTo
        {
            get;
            set;
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? PrevFireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextFirTime
        {
            get;
            set;
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int? Priority
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public int? Type
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime
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
        /// 执行失败操作类型
        /// </summary>
        public int? MisFireType
        {
            get;
            set;
        }

        /// <summary>
        /// 总触发次数
        /// </summary>
        public int? FireTotalCount
        {
            get;
            set;
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载对应的任务数据
        /// </summary>
        public bool LoadJob
        {
            get;set;
        }

        /// <summary>
        /// 加载对应条件
        /// </summary>
        public bool LoadCondition
        {
            get;set;
        }

        #endregion
    }
}