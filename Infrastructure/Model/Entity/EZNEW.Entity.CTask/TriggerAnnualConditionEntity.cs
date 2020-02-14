using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 计划年度条件
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerAnnualCondition", "CTask", "计划年度条件")]
    public class TriggerAnnualConditionEntity : BaseEntity<TriggerAnnualConditionEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string TriggerId
        {
            get { return GetPropertyValue<string>("TriggerId"); }
            set { SetPropertyValue("TriggerId", value); }
        }

        /// <summary>
        /// 月份
        /// </summary>
        [EntityField(Description = "月份", PrimaryKey = true)]
        public int Month
        {
            get { return GetPropertyValue<int>("Month"); }
            set { SetPropertyValue("Month", value); }
        }

        /// <summary>
        /// 日期
        /// </summary>
        [EntityField(Description = "日期", PrimaryKey = true)]
        public int Day
        {
            get { return GetPropertyValue<int>("Day"); }
            set { SetPropertyValue("Day", value); }
        }

        /// <summary>
        /// 包含
        /// </summary>
        [EntityField(Description = "包含")]
        public bool Include
        {
            get { return GetPropertyValue<bool>("Include"); }
            set { SetPropertyValue("Include", value); }
        }

        #endregion
    }
}