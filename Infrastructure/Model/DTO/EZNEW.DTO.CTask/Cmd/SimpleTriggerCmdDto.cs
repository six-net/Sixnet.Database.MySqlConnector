using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 简单类型执行计划
    /// </summary>
    public class SimpleTriggerCmdDto:TriggerCmdDto
    {
        #region	属性

        /// <summary>
        /// 重复次数
        /// </summary>
        public int RepeatCount
        {
            get;
            set;
        }

        /// <summary>
        /// 重复间隔
        /// </summary>
        public long RepeatInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 一直重复执行
        /// </summary>
        public bool RepeatForever
        {
            get;
            set;
        }

        #endregion
    }
}
