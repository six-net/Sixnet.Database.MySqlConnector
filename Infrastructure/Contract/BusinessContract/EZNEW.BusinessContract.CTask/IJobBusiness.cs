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
    /// 工作任务业务接口
    /// </summary>
    public interface IJobBusiness
    {
        #region 保存工作任务

        /// <summary>
        /// 保存工作任务
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result<JobDto> SaveJob(SaveJobCmdDto saveInfo);

        #endregion

        #region 获取工作任务

        /// <summary>
        /// 获取工作任务
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        JobDto GetJob(JobFilterDto filter);

        #endregion

        #region 获取工作任务列表

        /// <summary>
        /// 获取工作任务列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<JobDto> GetJobList(JobFilterDto filter);

        #endregion

        #region 获取工作任务分页

        /// <summary>
        /// 获取工作任务分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<JobDto> GetJobPaging(JobFilterDto filter);

        #endregion

        #region 删除工作任务

        /// <summary>
        /// 删除工作任务
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteJob(DeleteJobCmdDto deleteInfo);

        #endregion

        #region 修改工作任务运行状态

        /// <summary>
        /// 修改工作任务运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        Result ModifyJobRunState(ModifyJobRunStatusCmdDto stateInfo);

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(JobFilterDto filter);

        #endregion
    }
}
