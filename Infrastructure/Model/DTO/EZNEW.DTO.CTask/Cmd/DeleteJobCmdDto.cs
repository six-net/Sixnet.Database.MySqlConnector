using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 删除工作任务
    /// </summary>
    public class DeleteJobCmdDto
    {
        /// <summary>
        /// 工作任务编号
        /// </summary>
        public IEnumerable<string> JobIds
        {
            get; set;
        }
    }
}
