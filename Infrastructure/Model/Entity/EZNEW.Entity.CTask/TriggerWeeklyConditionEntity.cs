using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 计划星期条件
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerWeeklyCondition", "CTask", "计划星期条件")]
    public class TriggerWeeklyConditionEntity : BaseEntity<TriggerWeeklyConditionEntity>
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
        /// 日期
        /// </summary>
        [EntityField(Description = "日期", PrimaryKey = true)]
        public int Day
        {
            get { return GetPropertyValue<int>("Day"); }
            set { SetPropertyValue("Day", value); }
        }

        /// <summary>
        /// 包含当前日期
        /// </summary>
        [EntityField(Description = "包含当前日期")]
        public bool Include
        {
            get { return GetPropertyValue<bool>("Include"); }
            set { SetPropertyValue("Include", value); }
        }

        #endregion
    }
}