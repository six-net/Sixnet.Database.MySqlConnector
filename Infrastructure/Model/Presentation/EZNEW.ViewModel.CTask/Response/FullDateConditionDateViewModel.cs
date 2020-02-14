using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 完整计划日期
    /// </summary>
    public class FullDateConditionDateViewModel
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
