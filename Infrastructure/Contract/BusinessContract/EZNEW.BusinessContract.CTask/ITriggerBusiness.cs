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
    /// 任务执行计划业务接口
    /// </summary>
    public interface ITriggerBusiness
    {
        #region 保存任务执行计划

        /// <summary>
        /// 保存任务执行计划
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result<TriggerDto> SaveTrigger(SaveTriggerCmdDto saveInfo);

        #endregion

        #region 获取任务执行计划

        /// <summary>
        /// 获取任务执行计划
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        TriggerDto GetTrigger(TriggerFilterDto filter);

        #endregion

        #region 获取任务执行计划列表

        /// <summary>
        /// 获取任务执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<TriggerDto> GetTriggerList(TriggerFilterDto filter);

        #endregion

        #region 获取任务执行计划分页

        /// <summary>
        /// 获取任务执行计划分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<TriggerDto> GetTriggerPaging(TriggerFilterDto filter);

        #endregion

        #region 删除任务执行计划

        /// <summary>
        /// 删除任务执行计划
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteTrigger(DeleteTriggerCmdDto deleteInfo);

        #endregion

        #region 修改执行计划状态

        /// <summary>
        /// 修改执行计划状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        Result ModifyTriggerState(ModifyTriggerStatusCmdDto stateInfo);

        #endregion

    }
}
