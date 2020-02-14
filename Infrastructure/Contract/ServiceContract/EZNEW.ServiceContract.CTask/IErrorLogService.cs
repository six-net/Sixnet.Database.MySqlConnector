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
    /// 任务异常日志服务接口
    /// </summary>
    public interface IErrorLogService
    {
        #region 保存任务异常日志

        /// <summary>
        /// 保存任务异常日志
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result SaveErrorLog(SaveErrorLogCmdDto saveInfo);

        #endregion

        #region 获取任务异常日志

        /// <summary>
        /// 获取任务异常日志
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        ErrorLogDto GetErrorLog(ErrorLogFilterDto filter);

        #endregion

        #region 获取任务异常日志列表

        /// <summary>
        /// 获取任务异常日志列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<ErrorLogDto> GetErrorLogList(ErrorLogFilterDto filter);

        #endregion

        #region 获取任务异常日志分页

        /// <summary>
        /// 获取任务异常日志分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<ErrorLogDto> GetErrorLogPaging(ErrorLogFilterDto filter);

        #endregion
    }
}
