using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 计划服务
    /// </summary>
    public class TriggerServerDto
    {
        #region	属性

        /// <summary>
        /// 触发器
        /// </summary>
        public TriggerDto Trigger
        {
            get;
            set;
        }

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNodeDto Server
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public TaskTriggerServerRunStatus RunStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime LastFireDate
        {
            get;
            set;
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireDate
        {
            get;
            set;
        }

        #endregion

    }
}
