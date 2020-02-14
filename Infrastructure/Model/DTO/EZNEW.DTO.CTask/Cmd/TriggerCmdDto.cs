using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 执行计划信息对象
    /// </summary>
    public class TriggerCmdDto
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
        public JobCmdDto Job
        {
            get;
            set;
        }

        /// <summary>
        /// 应用到对象
        /// </summary>
        public TaskTriggerApplyTo ApplyTo
        {
            get;
            set;
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime PrevFireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public TaskTriggerStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public TaskTriggerType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 附加条件
        /// </summary>
        public TriggerConditionCmdDto Condition
        {
            get;set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
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
        /// 执行失败操作类型
        /// </summary>
        public TaskTriggerMisFireType MisFireType
        {
            get;
            set;
        }

        /// <summary>
        /// 总触发次数
        /// </summary>
        public int FireTotalCount
        {
            get;
            set;
        }

        #endregion

    }
}
