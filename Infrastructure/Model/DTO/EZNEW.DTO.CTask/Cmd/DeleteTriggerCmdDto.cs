using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 删除任务执行计划
    /// </summary>
    public class DeleteTriggerCmdDto
    {
        /// <summary>
        /// 任务执行计划编号
        /// </summary>
        public IEnumerable<string> TriggerIds
        {
            get; set;
        }
    }
}
