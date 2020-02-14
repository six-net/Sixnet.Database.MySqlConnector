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
    /// 服务节点执行计划存储
    /// </summary>
    public class TriggerServerRepository : DefaultAggregationRepository<TriggerServer, TriggerServerEntity, ITriggerServerDataAccess>, ITriggerServerRepository
    {
        #region 根据执行计划移除计划服务

        /// <summary>
        /// 根据执行计划移除计划服务
        /// </summary>
        /// <param name="triggers">执行计划数据</param>
        public void RemoveTriggerServerByTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            List<string> triggerIds = triggers.Select(c => c.Id).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerServerQuery>(c => triggerIds.Contains(c.Trigger));
            Remove(removeQuery, activationOption);
        }

        #endregion

        #region 根据服务节点移除计划服务

        /// <summary>
        /// 根据服务节点移除计划服务
        /// </summary>
        /// <param name="servers">服务数据</param>
        public void RemoveTriggerServerByServer(IEnumerable<ServerNode> servers, ActivationOption activationOption = null)
        {
            if (servers.IsNullOrEmpty())
            {
                return;
            }
            List<string> serverIds = servers.Select(c => c.Id).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerServerQuery>(c => serverIds.Contains(c.Server));
            Remove(removeQuery, activationOption);
        }

        #endregion

        #region 移除任务&服务承载时移除服务执行计划

        /// <summary>
        /// 移除任务&服务承载时移除服务执行计划
        /// </summary>
        /// <param name="jobServerHosts">任务服务承载信息</param>
        public void RemoveTriggerServerByJobHost(IEnumerable<JobServerHost> jobServerHosts, ActivationOption activationOption = null)
        {
            if (jobServerHosts.IsNullOrEmpty())
            {
                return;
            }
            List<string> serverIds = jobServerHosts.Select(c => c.Server?.Id).Distinct().ToList();
            List<string> jobIds = jobServerHosts.Select(c => c.Job?.Id).Distinct().ToList();
            //获取任务对应的执行计划
            var triggerQuery = QueryFactory.Create<TriggerQuery>(c => jobIds.Contains(c.Job));
            triggerQuery.AddQueryFields<TriggerQuery>(c => c.Id);
            List<Trigger> triggers = this.Instance<ITriggerRepository>().GetList(triggerQuery);
            List<TriggerServerEntity> triggerServerEntityList = new List<TriggerServerEntity>();
            foreach (var trigger in triggers)
            {
                foreach (var serverId in serverIds)
                {
                    triggerServerEntityList.Add(new TriggerServerEntity()
                    {
                        Trigger = trigger.Id,
                        Server = serverId
                    });
                }
            }
            RemoveEntity(triggerServerEntityList, activationOption);
        }

        #endregion
    }
}
