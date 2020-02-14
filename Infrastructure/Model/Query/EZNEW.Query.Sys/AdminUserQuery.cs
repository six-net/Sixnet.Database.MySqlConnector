using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Query.Sys
{
    /// <summary>
    /// 管理账户筛选
    /// </summary>
    public class AdminUserQuery : UserQuery
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string Roles
        {
            get; set;
        }
    }
}
