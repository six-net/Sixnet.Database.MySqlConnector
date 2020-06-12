using EZNEW.Module.Sys;
using System.Collections.Generic;

namespace EZNEW.DTO.Sys.Cmd
{
    /// <summary>
    /// 修改权限状态信息
    /// </summary>
    public class ModifyAuthorityStatusCmdDto
    {
        #region 属性

        /// <summary>
        /// 要修改的权限状态信息
        /// </summary>
        public Dictionary<string, AuthorityStatus> AuthorityStatusInfo
        {
            get;set;
        }

        #endregion
    }
}
