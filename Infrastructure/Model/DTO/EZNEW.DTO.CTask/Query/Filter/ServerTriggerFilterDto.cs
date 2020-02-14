using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query.Filter
{
    /// <summary>
    /// 根据服务获取执行计划筛选信息
    /// </summary>
    public class ServerTriggerFilterDto:TriggerFilterDto
    {
        /// <summary>
        /// 服务筛选信息
        /// </summary>
        public ServerNodeFilterDto ServerFilter
        {
            get;set;
        }
    }
}
