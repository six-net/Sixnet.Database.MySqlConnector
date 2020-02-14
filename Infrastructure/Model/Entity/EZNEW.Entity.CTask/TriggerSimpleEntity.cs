using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 简单类型执行计划
    /// </summary>
    [Serializable]
    [Entity("CTask_TriggerSimple", "CTask", "简单类型执行计划")]
    public class TriggerSimpleEntity : BaseEntity<TriggerSimpleEntity>
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
        /// 重复次数
        /// </summary>
        [EntityField(Description = "重复次数")]
        public int RepeatCount
        {
            get { return GetPropertyValue<int>("RepeatCount"); }
            set { SetPropertyValue("RepeatCount", value); }
        }

        /// <summary>
        /// 重复间隔
        /// </summary>
        [EntityField(Description = "重复间隔")]
        public long RepeatInterval
        {
            get { return GetPropertyValue<long>("RepeatInterval"); }
            set { SetPropertyValue("RepeatInterval", value); }
        }

        /// <summary>
        /// 一直重复执行
        /// </summary>
        [EntityField(Description = "一直重复执行")]
        public bool RepeatForever
        {
            get { return GetPropertyValue<bool>("RepeatForever"); }
            set { SetPropertyValue("RepeatForever", value); }
        }

        #endregion
    }
}