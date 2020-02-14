using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 年度计划
    /// </summary>
    public class TriggerAnnualConditionDto:TriggerConditionDto
    {
        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<AnnualConditionDayDto> Days
        {
            get; set;
        }

        #endregion
    }
}
