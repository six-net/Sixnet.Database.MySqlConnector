using EZNEW.Framework.Paging;
using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query.Filter
{
    /// <summary>
    /// 工作承载节点查询信息
    /// </summary>
    public class JobServerHostFilterDto : PagingFilter
    {
        #region	属性

        /// <summary>
        /// 服务
        /// </summary>
        public List<string> Servers
        {
            get;
            set;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public List<string> Jobs
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerStatus? RunStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 服务关键字
        /// </summary>
        public string ServerKey
        {
            get;set;
        }

        /// <summary>
        /// 任务关键字
        /// </summary>
        public string JobKey
        {
            get;set;
        }

        #endregion
    }
}