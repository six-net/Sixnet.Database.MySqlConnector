using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 保存工作任务信息
    /// </summary>
    public class SaveJobCmdDto
    {
        /// <summary>
        /// 工作任务信息
        /// </summary>
        public JobCmdDto Job
        {
            get; set;
        }
    }
}
