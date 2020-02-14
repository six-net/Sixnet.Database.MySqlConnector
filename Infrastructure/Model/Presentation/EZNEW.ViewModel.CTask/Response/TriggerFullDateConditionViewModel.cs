using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 完整日期计划
    /// </summary>
    public class TriggerFullDateConditionViewModel : TriggerConditionViewModel
    {
        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<FullDateConditionDateViewModel> Dates
        {
            get; set;
        }

        #endregion
    }
}
