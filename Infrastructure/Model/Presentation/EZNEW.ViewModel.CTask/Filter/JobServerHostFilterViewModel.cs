using EZNEW.Framework.Paging;
using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Filter
{
    /// <summary>
    /// 工作承载服务筛选信息
    /// </summary>
    public class JobServerHostFilterViewModel : PagingFilter
    {
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
        /// 服务关键字
        /// </summary>
        public string ServerKey
        {
            get; set;
        }

        /// <summary>
        /// 工作关键字
        /// </summary>
        public string JobKey
        {
            get; set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerStatus? RunStatus
        {
            get; set;
        }
    }
}
