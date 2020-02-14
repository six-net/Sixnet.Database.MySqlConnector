using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 删除服务节点
    /// </summary>
    public class DeleteServerNodeCmdDto
    {
        /// <summary>
        /// 服务节点编号
        /// </summary>
        public IEnumerable<string> ServerNodeIds
        {
            get;set;
        }
    }
}
