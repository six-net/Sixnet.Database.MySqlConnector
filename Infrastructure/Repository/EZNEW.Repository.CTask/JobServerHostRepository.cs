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
using EZNEW.Query.CTask;
using EZNEW.Domain.CTask.Repository;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 工作承载节点存储
    /// </summary>
    public class JobServerHostRepository : DefaultAggregationRepository<JobServerHost, JobServerHostEntity, IJobServerHostDataAccess>, IJobServerHostRepository
    {
        #region 删除工作时删除工作承载信息

        /// <summary>
        ///  删除工作时删除工作承载信息
        /// </summary>
        /// <param name="jobs">工作信息</param>
        public void RemoveJobServerHostByJob(IEnumerable<Job> jobs, ActivationOption activationOption = null)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<string> jobIds = jobs.Select(c => c.Id).Distinct();
            IQuery query = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
            Remove(query, activationOption);
        }

        #endregion

        #region 删除服务节点时删除工作承载信息

        /// <summary>
        /// 删除服务节点时删除工作承载信息
        /// </summary>
        /// <param name="servers">服务信息</param>
        public void RemoveJobServerHostByServer(IEnumerable<ServerNode> servers, ActivationOption activationOption = null)
        {
            if (servers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<string> serverIds = servers.Select(c => c.Id).Distinct();
            IQuery query = QueryFactory.Create<JobServerHostQuery>(c => serverIds.Contains(c.Server));
            Remove(query, activationOption);
        }

        #endregion
    }
}
