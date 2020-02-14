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
    /// 服务节点筛选
    /// </summary>
    public class ServerNodeFilterViewModel : PagingFilter
    {
        /// <summary>
        /// 编号
        /// </summary>
        public List<string> Ids
        {
            get;
            set;
        }

        /// <summary>
        /// 实例名称
        /// </summary>
        public string InstanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// 线程数量
        /// </summary>
        public int? ThreadCount
        {
            get;
            set;
        }

        /// <summary>
        /// 线程优先级
        /// </summary>
        public string ThreadPriority
        {
            get;
            set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 不包含已绑定指定任务的
        /// </summary>
        public IEnumerable<string> ExcludeBindJobIds
        {
            get; set;
        }
    }
}
