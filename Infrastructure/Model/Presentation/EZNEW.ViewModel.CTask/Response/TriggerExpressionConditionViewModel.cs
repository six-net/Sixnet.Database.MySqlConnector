using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 自定义计划
    /// </summary>
    public class TriggerExpressionConditionViewModel : TriggerConditionViewModel
    {
        #region	属性

        /// <summary>
        /// 表达式配置项
        /// </summary>
        public List<ExpressionItemViewModel> ExpressionItems
        {
            get; set;
        }

        #endregion
    }
}
