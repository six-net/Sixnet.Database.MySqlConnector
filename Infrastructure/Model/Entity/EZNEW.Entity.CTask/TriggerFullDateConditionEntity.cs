using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 计划完整日期条件
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerFullDateCondition", "CTask", "计划完整日期条件")]
    public class TriggerFullDateConditionEntity : BaseEntity<TriggerFullDateConditionEntity>
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
        /// 时间
        /// </summary>
        [EntityField(Description = "时间", PrimaryKey = true)]
        public DateTime Date
        {
            get { return GetPropertyValue<DateTime>("Date"); }
            set { SetPropertyValue("Date", value); }
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