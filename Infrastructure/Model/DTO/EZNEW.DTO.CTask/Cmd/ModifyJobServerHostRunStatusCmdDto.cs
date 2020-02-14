using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 修改服务承载运行状态
    /// </summary>
    public class ModifyJobServerHostRunStatusCmdDto
    {
        /// <summary>
        /// 要修改服务承载信息
        /// </summary>
        public List<JobServerHostCmdDto> JobServerHosts
        {
            get; set;
        }
    }
}
