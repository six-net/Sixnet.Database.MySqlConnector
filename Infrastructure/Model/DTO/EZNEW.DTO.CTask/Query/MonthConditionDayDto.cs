using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 日期计划日期数据
    /// </summary>
    public class MonthConditionDayDto
    {
        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public int Day
        {
            get; set;
        }

        /// <summary>
        /// 包含当前日期
        /// </summary>
        public bool Include
        {
            get; set;
        }

        #endregion
    }
}
