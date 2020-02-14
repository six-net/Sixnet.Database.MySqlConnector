using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 修改服务运行状态信息
    /// </summary>
    public class ModifyServerNodeRunStatusCmdDto
    {
        /// <summary>
        /// 要操作的服务信息
        /// </summary>
        public List<ServerNodeCmdDto> Servers
        {
            get;set;
        }
    }
}
