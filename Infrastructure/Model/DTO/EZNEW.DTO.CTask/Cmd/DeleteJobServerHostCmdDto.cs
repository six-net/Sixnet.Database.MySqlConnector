using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 删除服务承载信息
    /// </summary>
    public class DeleteJobServerHostCmdDto
    {
        /// <summary>
        /// 工作承载服务信息
        /// </summary>
        public List<JobServerHostCmdDto> JobServerHosts
        {
            get;set;
        }
    }
}
