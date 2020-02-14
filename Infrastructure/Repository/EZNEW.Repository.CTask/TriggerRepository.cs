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
using EZNEW.Query.CTask;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 任务执行计划存储
    /// </summary>
    public class TriggerRepository : DefaultAggregationRepository<Trigger, TriggerEntity, ITriggerDataAccess>, ITriggerRepository
    {
        #region 删除工作任务时删除相应的计划

        /// <summary>
        /// 删除工作任务时删除相应的计划
        /// </summary>
        /// <param name="jobs">工作任务</param>
        public void RemoveTriggerByJob(IEnumerable<Job> jobs, ActivationOption activationOption = null)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            List<string> jobIds = jobs.Select(c => c.Id).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerQuery>(c => jobIds.Contains(c.Job));
            Remove(removeQuery, activationOption);
        }

        #endregion
    }
}
