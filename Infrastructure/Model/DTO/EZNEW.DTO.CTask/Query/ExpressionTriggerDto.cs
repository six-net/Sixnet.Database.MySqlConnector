using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 自定义执行计划
    /// </summary>
    public class ExpressionTriggerDto:TriggerDto
    {
        #region	属性

        /// <summary>
        /// 表达式
        /// </summary>
        public List<ExpressionItemDto> ExpressionItems
        {
            get;
            set;
        }

        #endregion
    }
}
