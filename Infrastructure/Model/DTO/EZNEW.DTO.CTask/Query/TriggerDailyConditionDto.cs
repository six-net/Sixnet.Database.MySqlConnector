using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 每天时间段
    /// </summary>
    public class TriggerDailyConditionDto : TriggerConditionDto
    {
        #region	属性

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime
        {
            get; set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get; set;
        }

        /// <summary>
        /// 启用设定值范围以外
        /// </summary>
        public bool Inversion
        {
            get; set;
        }

        #endregion
    }
}
