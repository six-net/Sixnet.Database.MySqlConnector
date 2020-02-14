using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.DTO.CTask.Cmd;
using EZNEW.DTO.CTask.Query;
using EZNEW.DTO.CTask.Query.Filter;
using EZNEW.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Response;

namespace EZNEW.BusinessContract.CTask
{
    /// <summary>
    /// 工作承载节点业务接口
    /// </summary>
    public interface IJobServerHostBusiness
    {
        #region 保存工作承载节点

        /// <summary>
        /// 保存工作承载节点
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result SaveJobServerHost(SaveJobServerHostCmdDto saveInfo);

        #endregion

        #region 获取工作承载节点

        /// <summary>
        /// 获取工作承载节点
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        JobServerHostDto GetJobServerHost(JobServerHostFilterDto filter);

        #endregion

        #region 获取工作承载节点列表

        /// <summary>
        /// 获取工作承载节点列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<JobServerHostDto> GetJobServerHostList(JobServerHostFilterDto filter);

        #endregion

        #region 获取工作承载节点分页

        /// <summary>
        /// 获取工作承载节点分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<JobServerHostDto> GetJobServerHostPaging(JobServerHostFilterDto filter);

        #endregion

        #region 修改承载服务运行状态

        /// <summary>
        /// 修改承载服务运行状态
        /// </summary>
        /// <param name="modifyInfo">修改信息</param>
        /// <returns></returns>
        Result ModifyRunState(ModifyJobServerHostRunStatusCmdDto modifyInfo);

        #endregion

        #region 删除工作承载节点

        /// <summary>
        /// 删除工作承载节点
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteJobServerHost(DeleteJobServerHostCmdDto deleteInfo);

        #endregion
    }
}
