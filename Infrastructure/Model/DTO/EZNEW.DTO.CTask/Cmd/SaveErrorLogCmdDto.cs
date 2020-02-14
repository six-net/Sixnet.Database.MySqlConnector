using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 保存任务异常日志信息
    /// </summary>
    public class SaveErrorLogCmdDto
    {
        /// <summary>
        /// 任务异常日志信息
        /// </summary>
        public IEnumerable<ErrorLogCmdDto> ErrorLogs
        {
            get; set;
        }
    }
}
