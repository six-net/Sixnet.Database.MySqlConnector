using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 删除工作分组
    /// </summary>
    public class DeleteJobGroupCmdDto
    {
        /// <summary>
        /// 分组编号
        /// </summary>
        public IEnumerable<string> JobGroupIds
        {
            get;set;
        }
    }
}
