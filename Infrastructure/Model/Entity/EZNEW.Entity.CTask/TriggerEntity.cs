using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务执行计划
    /// </summary>
    [Serializable]
    [Entity("CTask_Trigger", "CTask", "任务执行计划")]
    public class TriggerEntity : BaseEntity<TriggerEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string Id
        {
            get { return GetPropertyValue<string>("Id"); }
            set { SetPropertyValue("Id", value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [EntityField(Description = "名称")]
        public string Name
        {
            get { return GetPropertyValue<string>("Name"); }
            set { SetPropertyValue("Name", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Description
        {
            get { return GetPropertyValue<string>("Description"); }
            set { SetPropertyValue("Description", value); }
        }

        /// <summary>
        /// 所属任务
        /// </summary>
        [EntityField(Description = "所属任务")]
        public string Job
        {
            get { return GetPropertyValue<string>("Job"); }
            set { SetPropertyValue("Job", value); }
        }

        /// <summary>
        /// 应用到对象
        /// </summary>
        [EntityField(Description = "应用到对象")]
        public int ApplyTo
        {
            get { return GetPropertyValue<int>("ApplyTo"); }
            set { SetPropertyValue("ApplyTo", value); }
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        [EntityField(Description = "上次执行时间")]
        public DateTime PrevFireTime
        {
            get { return GetPropertyValue<DateTime>("PrevFireTime"); }
            set { SetPropertyValue("PrevFireTime", value); }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        [EntityField(Description = "下次执行时间")]
        public DateTime NextFireTime
        {
            get { return GetPropertyValue<DateTime>("NextFireTime"); }
            set { SetPropertyValue("NextFireTime", value); }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        [EntityField(Description = "优先级")]
        public int Priority
        {
            get { return GetPropertyValue<int>("Priority"); }
            set { SetPropertyValue("Priority", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityField(Description = "状态")]
        public int Status
        {
            get { return GetPropertyValue<int>("Status"); }
            set { SetPropertyValue("Status", value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        [EntityField(Description = "类型")]
        public int Type
        {
            get { return GetPropertyValue<int>("Type"); }
            set { SetPropertyValue("Type", value); }
        }

        /// <summary>
        /// 额外条件类型
        /// </summary>
        [EntityField(Description = "额外条件类型")]
        public int ConditionType
        {
            get { return GetPropertyValue<int>("ConditionType"); }
            set { SetPropertyValue("ConditionType", value); }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EntityField(Description = "开始时间")]
        public DateTime StartTime
        {
            get { return GetPropertyValue<DateTime>("StartTime"); }
            set { SetPropertyValue("StartTime", value); }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        [EntityField(Description = "结束时间")]
        public DateTime EndTime
        {
            get { return GetPropertyValue<DateTime>("EndTime"); }
            set { SetPropertyValue("EndTime", value); }
        }

        /// <summary>
        /// 执行失败操作类型
        /// </summary>
        [EntityField(Description = "执行失败操作类型")]
        public int MisFireType
        {
            get { return GetPropertyValue<int>("MisFireType"); }
            set { SetPropertyValue("MisFireType", value); }
        }

        /// <summary>
        /// 总触发次数
        /// </summary>
        [EntityField(Description = "总触发次数")]
        public int FireTotalCount
        {
            get { return GetPropertyValue<int>("FireTotalCount"); }
            set { SetPropertyValue("FireTotalCount", value); }
        }

        #endregion
    }
}