using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query.Filter
{
    /// <summary>
    /// 服务允许调度的任务计划筛选信息
    /// </summary>
    public class ServerScheduleTriggerFilterDto: TriggerFilterDto
    {
        #region 属性

        /// <summary>
        /// 服务筛选信息
        /// </summary>
        public ServerNodeFilterDto ServerFilter
        {
            get; set;
        }

        /// <summary>
        /// 任务筛选信息
        /// </summary>
        public JobFilterDto JobFilter
        {
            get; set;
        }

        #endregion
    }
}
