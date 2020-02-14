using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 表达式项
    /// </summary>
    public class ExpressionItemDto
    {
        #region	属性

        /// <summary>
        /// 表达式配置项
        /// </summary>
        public TaskTriggerExpressionItem Option
        {
            get; set;
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public TaskTriggerExpressionItemConfigType ValueType
        {
            get; set;
        }

        /// <summary>
        /// 开始值
        /// </summary>
        public int BeginValue
        {
            get; set;
        }

        /// <summary>
        /// 结束值
        /// </summary>
        public int EndValue
        {
            get; set;
        }

        /// <summary>
        /// 集合值
        /// </summary>
        public List<string> ArrayValue
        {
            get; set;
        }

        #endregion
    }
}
