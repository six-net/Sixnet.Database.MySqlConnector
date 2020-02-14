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

namespace EZNEW.ServiceContract.CTask
{
    /// <summary>
    /// 任务执行日志服务接口
    /// </summary>
    public interface IExecuteLogService
    {
        #region 保存任务执行日志

        /// <summary>
        /// 保存任务执行日志
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result SaveExecuteLog(SaveExecuteLogCmdDto saveInfo);

        #endregion

        #region 获取任务执行日志

        /// <summary>
        /// 获取任务执行日志
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        ExecuteLogDto GetExecuteLog(ExecuteLogFilterDto filter);

        #endregion

        #region 获取任务执行日志列表

        /// <summary>
        /// 获取任务执行日志列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<ExecuteLogDto> GetExecuteLogList(ExecuteLogFilterDto filter);

        #endregion

        #region 获取任务执行日志分页

        /// <summary>
        /// 获取任务执行日志分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<ExecuteLogDto> GetExecuteLogPaging(ExecuteLogFilterDto filter);

        #endregion

        #region 删除任务执行日志

        /// <summary>
        /// 删除任务执行日志
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteExecuteLog(DeleteExecuteLogCmdDto deleteInfo);

        #endregion
    }
}
