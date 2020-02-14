using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query
{
    /// <summary>
    /// 工作主机信息
    /// </summary>
    public class JobServerHostDto
    {
        #region	属性

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNodeDto Server
        {
            get;
            set;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public JobDto Job
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerStatus RunStatus
        {
            get;
            set;
        }

        #endregion
    }
}
