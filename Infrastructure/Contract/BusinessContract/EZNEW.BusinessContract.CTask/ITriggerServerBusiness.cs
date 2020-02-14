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
    /// 服务节点执行计划业务接口
    /// </summary>
    public interface ITriggerServerBusiness
    {
        #region 获取服务节点执行计划

        /// <summary>
        /// 获取服务节点执行计划
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        TriggerServerDto GetTriggerServer(TriggerServerFilterDto filter);

        #endregion

        #region 获取服务节点执行计划列表

        /// <summary>
        /// 获取服务节点执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<TriggerServerDto> GetTriggerServerList(TriggerServerFilterDto filter);

        #endregion

        #region 获取服务节点执行计划分页

        /// <summary>
        /// 获取服务节点执行计划分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<TriggerServerDto> GetTriggerServerPaging(TriggerServerFilterDto filter);

        #endregion

        #region 删除服务节点执行计划

        /// <summary>
        /// 删除服务节点执行计划
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteTriggerServer(DeleteTriggerServerCmdDto deleteInfo);

        #endregion

        #region 修改计划服务运行状态

        /// <summary>
        /// 修改计划服务运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        Result ModifyRunState(ModifyTriggerServerRunStatusCmdDto stateInfo);

        #endregion

        #region 保存服务节点执行计划

        /// <summary>
        /// 保存服务节点执行计划
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns></returns>
        Result SaveTriggerServer(SaveTriggerServerCmdDto saveInfo);

        #endregion
    }
}
