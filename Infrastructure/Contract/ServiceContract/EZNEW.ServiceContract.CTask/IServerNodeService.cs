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
    /// 服务节点服务接口
    /// </summary>
    public interface IServerNodeService
    {
        #region 保存服务节点

        /// <summary>
        /// 保存服务节点
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result<ServerNodeDto> SaveServerNode(SaveServerNodeCmdDto saveInfo);

        #endregion

        #region 获取服务节点

        /// <summary>
        /// 获取服务节点
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        ServerNodeDto GetServerNode(ServerNodeFilterDto filter);

        #endregion

        #region 获取服务节点列表

        /// <summary>
        /// 获取服务节点列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<ServerNodeDto> GetServerNodeList(ServerNodeFilterDto filter);

        #endregion

        #region 获取服务节点分页

        /// <summary>
        /// 获取服务节点分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<ServerNodeDto> GetServerNodePaging(ServerNodeFilterDto filter);

        #endregion

        #region 删除服务节点

        /// <summary>
        /// 删除服务节点
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteServerNode(DeleteServerNodeCmdDto deleteInfo);

        #endregion

        #region 修改服务节点运行状态

        /// <summary>
        /// 修改服务节点运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        Result ModifyRunState(ModifyServerNodeRunStatusCmdDto stateInfo);

        #endregion
    }
}
