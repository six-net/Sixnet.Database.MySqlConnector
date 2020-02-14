using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 保存服务节点命令信息
    /// </summary>
    public class SaveServerNodeCmdDto
    {
        /// <summary>
        /// 服务节点信息
        /// </summary>
        public ServerNodeCmdDto ServerNode
        {
            get;set;
        }
    }
}
