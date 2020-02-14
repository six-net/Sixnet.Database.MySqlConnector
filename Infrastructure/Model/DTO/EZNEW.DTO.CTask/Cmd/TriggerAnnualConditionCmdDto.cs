using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 每年天计划信息
    /// </summary>
    public class TriggerAnnualConditionCmdDto : TriggerConditionCmdDto
    {
        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<AnnualConditionDayCmdDto> Days
        {
            get; set;
        }

        #endregion
    }
}
