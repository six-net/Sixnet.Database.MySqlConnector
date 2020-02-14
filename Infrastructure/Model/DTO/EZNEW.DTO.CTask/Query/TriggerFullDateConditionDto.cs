using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 完整日期计划
    /// </summary>
    public class TriggerFullDateConditionDto : TriggerConditionDto
    {
        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<FullDateConditionDateDto> Dates
        {
            get; set;
        }

        #endregion
    }
}
