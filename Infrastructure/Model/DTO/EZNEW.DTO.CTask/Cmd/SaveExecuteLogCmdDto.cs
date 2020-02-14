using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 保存任务执行日志信息
    /// </summary>
    public class SaveExecuteLogCmdDto
    {
        /// <summary>
        /// 任务执行日志信息
        /// </summary>
        public IEnumerable<ExecuteLogCmdDto> ExecuteLogs
        {
            get; set;
        }
    }
}
