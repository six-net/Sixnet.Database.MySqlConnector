using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EZNEW.ViewModel.Sys.Request
{
    /// <summary>
    /// 管理用户
    /// </summary>
    public class EditAdminUserViewModel : EditUserViewModel
    {
        #region	属性

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<EditRoleViewModel> Roles
        {
            get; set;
        }

        #endregion
    }
}
