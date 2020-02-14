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
    /// 工作分组业务接口
    /// </summary>
    public interface IJobGroupBusiness
    {
        #region 保存工作分组

        /// <summary>
        /// 保存工作分组
        /// </summary>
        /// <param name="jobGroup">工作分组对象</param>
        /// <returns>执行结果</returns>
        Result<JobGroupDto> SaveJobGroup(JobGroupCmdDto jobGroup);

        #endregion

        #region 获取工作分组

        /// <summary>
        /// 获取工作分组
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        JobGroupDto GetJobGroup(JobGroupFilterDto filter);

        #endregion

        #region 获取工作分组列表

        /// <summary>
        /// 获取工作分组列表
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        List<JobGroupDto> GetJobGroupList(JobGroupFilterDto filter);

        #endregion

        #region 获取工作分组分页

        /// <summary>
        /// 获取工作分组分页
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        IPaging<JobGroupDto> GetJobGroupPaging(JobGroupFilterDto filter);

        #endregion

        #region 删除工作分组

        /// <summary>
        /// 删除工作分组
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteJobGroup(DeleteJobGroupCmdDto deleteInfo);

        #endregion

        #region 修改工作分组排序

        /// <summary>
        /// 修改分组排序
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <returns>执行结果</returns>
        Result ModifySortIndex(ModifyJobGroupSortCmdDto sortInfo);

        #endregion

    }
}
