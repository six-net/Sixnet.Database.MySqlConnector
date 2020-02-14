using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Cmd
{
    /// <summary>
    /// 修改分组排序
    /// </summary>
    public class ModifyJobGroupSortCmdDto
    {
        /// <summary>
        /// 分组编号
        /// </summary>
        public string Code
        {
            get;set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int NewSort
        {
            get;set;
        }
    }
}
