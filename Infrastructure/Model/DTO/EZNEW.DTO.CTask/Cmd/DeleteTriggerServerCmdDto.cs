using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 删除计划服务节点信息
    /// </summary>
    public class DeleteTriggerServerCmdDto
    {
        /// <summary>
        /// 要删除的服务计划信息
        /// </summary>
        public List<TriggerServerCmdDto> TriggerServers
        {
            get;set;
        }
    }
}
