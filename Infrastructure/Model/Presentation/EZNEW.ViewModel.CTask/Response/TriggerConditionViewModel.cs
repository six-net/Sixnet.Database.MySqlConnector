using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 执行计划
    /// </summary>
    public class TriggerConditionViewModel
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string TriggerId
        {
            get; set;
        }

        /// <summary>
        /// 条件类型
        /// </summary>
        public TaskTriggerConditionType Type
        {
            get; set;
        }

        #endregion
    }
}
