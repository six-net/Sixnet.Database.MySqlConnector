using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using EZNEW.Entity.CTask;
using EZNEW.DataAccessContract.CTask;
using EZNEW.Framework.Extension;
using EZNEW.Develop.CQuery;
using EZNEW.Query.CTask;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 工作任务存储
    /// </summary>
    public class JobRepository : DefaultAggregationRepository<Job, JobEntity, IJobDataAccess>, IJobRepository
    {
        #region 移除工作日分组时移除任务信息

        /// <summary>
        /// 移除工作分组时移除任务信息
        /// </summary>
        /// <param name="jobGroups">任务分组</param>
        public void RemoveByJobGroup(IEnumerable<JobGroup> jobGroups, ActivationOption activationOption = null)
        {
            if (jobGroups.IsNullOrEmpty())
            {
                return;
            }
            List<string> groupIds = jobGroups.Select(c => c.Code).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<JobQuery>(c => groupIds.Contains(c.Group));
            Remove(removeQuery, activationOption);
        }

        /// <summary>
        /// 移除工作分组时移除任务信息
        /// </summary>
        /// <param name="jobGroups">query</param>
        public void RemoveByJobGroup(IQuery query, ActivationOption activationOption = null)
        {
            if (query == null)
            {
                return;
            }
            var removeGroupQuery = query.Clone();
            removeGroupQuery.QueryFields.Clear();
            removeGroupQuery.AddQueryFields<JobGroupQuery>(c => c.Code);
            //remove job
            var removeJobQuery = QueryFactory.Create<JobQuery>();
            removeJobQuery.And<JobQuery>(c => c.Group, CriteriaOperator.In, removeGroupQuery);
            Remove(removeJobQuery, activationOption);
        }

        #endregion
    }
}
