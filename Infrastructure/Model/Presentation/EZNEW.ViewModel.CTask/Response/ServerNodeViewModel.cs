using EZNEW.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 服务节点信息
    /// </summary>
    public class ServerNodeViewModel
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string Id
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
        public ServerStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusText
        {
            get
            {
                return Status.ToString();
            }
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
        public int ThreadCount
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

        #endregion
    }
}
