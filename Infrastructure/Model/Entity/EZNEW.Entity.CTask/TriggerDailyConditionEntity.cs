using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 计划时间计划条件
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerDailyCondition", "CTask", "计划时间计划条件")]
    public class TriggerDailyConditionEntity : BaseEntity<TriggerDailyConditionEntity>
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
        /// 开始时间
        /// </summary>
        [EntityField(Description = "开始时间")]
        public string BeginTime
        {
            get { return GetPropertyValue<string>("BeginTime"); }
            set { SetPropertyValue("BeginTime", value); }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        [EntityField(Description = "结束时间")]
        public string EndTime
        {
            get { return GetPropertyValue<string>("EndTime"); }
            set { SetPropertyValue("EndTime", value); }
        }

        /// <summary>
        /// 启用设定值范围以外
        /// </summary>
        [EntityField(Description = "启用设定值范围以外")]
        public bool Inversion
        {
            get { return GetPropertyValue<bool>("Inversion"); }
            set { SetPropertyValue("Inversion", value); }
        }

        #endregion
    }
}