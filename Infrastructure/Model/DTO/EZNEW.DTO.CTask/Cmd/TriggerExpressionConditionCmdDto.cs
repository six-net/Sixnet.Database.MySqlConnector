using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 自定义计划
    /// </summary>
    public class TriggerExpressionConditionCmdDto : TriggerConditionCmdDto
    {
        #region	属性

        /// <summary>
        /// 表达式配置项
        /// </summary>
        public List<ExpressionItemCmdDto> ExpressionItems
        {
            get;set;
        }

        #endregion
    }
}
