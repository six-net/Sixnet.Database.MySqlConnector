using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 星期计划
    /// </summary>
    public class TriggerWeeklyConditionDto : TriggerConditionDto
    {
        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<WeeklyConditionDayDto> Days
        {
            get; set;
        }

        #endregion
    }
}
