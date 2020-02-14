using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Filter
{
    /// <summary>
    /// 服务节点执行计划查询信息
    /// </summary>
    public class TriggerServerFilterViewModel : PagingFilter
    {
        #region	属性

        /// <summary>
        /// 触发器
        /// </summary>
        public List<string> Triggers
        {
            get;
            set;
        }

        /// <summary>
        /// 服务
        /// </summary>
        public List<string> Servers
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int? RunStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? LastFireDate
        {
            get;
            set;
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextFireDate
        {
            get;
            set;
        }

        #endregion
    }
}