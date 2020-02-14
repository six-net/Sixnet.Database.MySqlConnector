using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 每月日期计划
    /// </summary>
    public class TriggerMonthlyConditionDto : TriggerConditionDto
    {
        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<MonthConditionDayDto> Days
        {
            get; set;
        }

        #endregion
    }
}
