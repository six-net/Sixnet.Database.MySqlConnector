using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 月份日期计划
    /// </summary>
    public class TriggerMonthlyConditionCmdDto : TriggerConditionCmdDto
    {
        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<MonthConditionDayCmdDto> Days
        {
            get;set;
        }

        #endregion
    }
}
