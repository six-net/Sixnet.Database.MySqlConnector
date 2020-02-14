using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 完整日期计划日期
    /// </summary>
    public class FullDateConditionDateDto
    {
        #region	属性

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date
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
