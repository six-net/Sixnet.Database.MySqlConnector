using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 删除任务工作文件
    /// </summary>
    public class DeleteJobFileCmdDto
    {
        /// <summary>
        /// 任务工作文件编号
        /// </summary>
        public IEnumerable<long> JobFileIds
        {
            get; set;
        }
    }
}
