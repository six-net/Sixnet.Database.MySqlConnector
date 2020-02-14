using System;
using System.Collections.Generic;
using EZNEW.Develop.CQuery;
using EZNEW.Entity.CTask;

namespace EZNEW.Query.CTask
{
    /// <summary>
    /// 计划月份附加条件
    /// </summary>
    [QueryEntity(typeof(TriggerMonthlyConditionEntity))]
    public class TriggerMonthlyConditionQuery : QueryModel<TriggerMonthlyConditionQuery>
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string TriggerId
        {
            get;
            set;
        }

        /// <summary>
        /// 日期
        /// </summary>
        public int Day
        {
            get;
            set;
        }

        /// <summary>
        /// 包含当前日期
        /// </summary>
        public bool Include
        {
            get;
            set;
        }

        #endregion
    }
}