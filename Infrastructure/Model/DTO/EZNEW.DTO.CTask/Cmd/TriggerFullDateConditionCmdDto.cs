using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 完整日期计划
    /// </summary>
    public class TriggerFullDateConditionCmdDto : TriggerConditionCmdDto
    {
        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<FullDateConditionDateCmdDto> Dates
        {
            get;set;
        }

        #endregion
    }
}
