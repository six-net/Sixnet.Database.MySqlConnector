using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 计划条件
    /// </summary>
    public class TriggerConditionDto
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
