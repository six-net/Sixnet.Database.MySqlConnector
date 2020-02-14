using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 星期计划
    /// </summary>
    public class TriggerWeeklyConditionViewModel : TriggerConditionViewModel
    {
        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<WeeklyConditionDayViewModel> Days
        {
            get; set;
        }

        #endregion
    }
}
