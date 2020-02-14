using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 保存任务执行计划信息
    /// </summary>
    public class SaveTriggerCmdDto
    {
        /// <summary>
        /// 任务执行计划信息
        /// </summary>
        public TriggerCmdDto Trigger
        {
            get; set;
        }

        /// <summary>
        /// 任务计划服务
        /// </summary>
        public List<TriggerServerCmdDto> TriggerServers
        {
            get; set;
        }
    }
}
