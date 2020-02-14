using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Domain.CTask.Model;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Entity.CTask;
using EZNEW.Framework.Extension;
using EZNEW.DataAccessContract.CTask;
using EZNEW.Develop.CQuery;
using EZNEW.Framework.Paging;
using EZNEW.Domain.CTask.Repository;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 服务节点存储
    /// </summary>
    public class ServerNodeRepository : DefaultAggregationRepository<ServerNode, ServerNodeEntity, IServerNodeDataAccess>, IServerNodeRepository
    {
    }
}
