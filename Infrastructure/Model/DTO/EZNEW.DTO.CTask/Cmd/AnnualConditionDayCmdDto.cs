using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 年计划日期
    /// </summary>
    public class AnnualConditionDayCmdDto
    {
        #region	属性

        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get;set;
        }

        /// <summary>
        /// 日期
        /// </summary>
        public int Day
        {
            get;set;
        }

        /// <summary>
        /// 包含
        /// </summary>
        public bool Include
        {
            get;set;
        }

        #endregion
    }
}
