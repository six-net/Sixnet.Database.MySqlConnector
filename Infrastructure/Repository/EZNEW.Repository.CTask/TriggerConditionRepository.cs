using EZNEW.DataAccessContract.CTask;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Entity.CTask;
using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Develop.Domain.Aggregation;
using EZNEW.CTask;
using EZNEW.Query.CTask;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Develop.Command.Modify;
using EZNEW.Framework.Paging;
using System.Collections.Concurrent;
using EZNEW.Develop.Domain.Repository.Warehouse;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 计划附加条件存储
    /// </summary>
    public class TriggerConditionRepository : DefaultAggregationRootRepository<TriggerCondition>, ITriggerConditionRepository
    {
        ITriggerFullDateConditionRepository fullDateConditionRepository = null;//固定日期
        ITriggerAnnualConditionRepository annualConditionRepository = null;//每年日期
        ITriggerDailyConditionRepository dailyConditionRepository = null;//时间段计划存储
        ITriggerExpressionConditionRepository expressionConditionRepository = null;//自定义计划存储
        ITriggerMonthlyConditionRepository monthlyConditionRepository = null;//每月日期存储
        ITriggerWeeklyConditionRepository weeklyConditionRepository = null;//星期计划存储
        Dictionary<TaskTriggerConditionType, ITriggerConditionBaseRepository> triggerConditionRepositoryDict = new Dictionary<TaskTriggerConditionType, ITriggerConditionBaseRepository>();
        static ConcurrentDictionary<Guid, TaskTriggerConditionType> conditionTypeQueryModelDict = new ConcurrentDictionary<Guid, TaskTriggerConditionType>();// condition type&condition query model

        static TriggerConditionRepository()
        {
            conditionTypeQueryModelDict.TryAdd(typeof(TriggerFullDateConditionEntity).GUID, TaskTriggerConditionType.固定日期);
            conditionTypeQueryModelDict.TryAdd(typeof(TriggerAnnualConditionEntity).GUID, TaskTriggerConditionType.每年日期);
            conditionTypeQueryModelDict.TryAdd(typeof(TriggerDailyConditionEntity).GUID, TaskTriggerConditionType.每天时间段);
            conditionTypeQueryModelDict.TryAdd(typeof(TriggerExpressionConditionEntity).GUID, TaskTriggerConditionType.自定义);
            conditionTypeQueryModelDict.TryAdd(typeof(TriggerMonthlyConditionEntity).GUID, TaskTriggerConditionType.每月日期);
            conditionTypeQueryModelDict.TryAdd(typeof(TriggerWeeklyConditionEntity).GUID, TaskTriggerConditionType.星期配置);
        }

        public TriggerConditionRepository()
        {
            fullDateConditionRepository = this.Instance<ITriggerFullDateConditionRepository>();
            triggerConditionRepositoryDict.Add(TaskTriggerConditionType.固定日期, fullDateConditionRepository);
            annualConditionRepository = this.Instance<ITriggerAnnualConditionRepository>();
            triggerConditionRepositoryDict.Add(TaskTriggerConditionType.每年日期, annualConditionRepository);
            dailyConditionRepository = this.Instance<ITriggerDailyConditionRepository>();
            triggerConditionRepositoryDict.Add(TaskTriggerConditionType.每天时间段, dailyConditionRepository);
            expressionConditionRepository = this.Instance<ITriggerExpressionConditionRepository>();
            triggerConditionRepositoryDict.Add(TaskTriggerConditionType.自定义, expressionConditionRepository);
            monthlyConditionRepository = this.Instance<ITriggerMonthlyConditionRepository>();
            triggerConditionRepositoryDict.Add(TaskTriggerConditionType.每月日期, monthlyConditionRepository);
            weeklyConditionRepository = this.Instance<ITriggerWeeklyConditionRepository>();
            triggerConditionRepositoryDict.Add(TaskTriggerConditionType.星期配置, weeklyConditionRepository);
        }

        #region Imple

        protected override async Task<VT> AvgValueAsync<VT>(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            VT value = default(VT);
            if (repository != null)
            {
                value = await repository.AvgAsync<VT>(query).ConfigureAwait(false);
            }
            return value;
        }

        protected override async Task<long> CountValueAsync(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            long value = 0;
            if (repository != null)
            {
                value = await repository.CountAsync(query).ConfigureAwait(false);
            }
            return value;
        }

        protected override async Task<IActivationRecord> ExecuteModifyAsync(IModify expression, IQuery query, ActivationOption activationOption = null)
        {
            var repository = GetRepositoryByQuery(query);
            if (repository == null)
            {
                return null;
            }
            await repository.ModifyAsync(expression, query, activationOption).ConfigureAwait(false);
            return EmptyActivationRecord.Default;
        }

        protected override async Task<IActivationRecord> ExecuteRemoveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            if (data == null)
            {
                return null;
            }
            var repository = GetRepositoryByConditionType(data.Type);
            if (repository == null)
            {
                return null;
            }
            await repository.RemoveAsync(data, activationOption).ConfigureAwait(false);
            return EmptyActivationRecord.Default;
        }

        protected override async Task<IActivationRecord> ExecuteRemoveAsync(IQuery query, ActivationOption activationOption = null)
        {
            var repository = GetRepositoryByQuery(query);
            if (repository == null)
            {
                return null;
            }
            await repository.RemoveAsync(query, activationOption).ConfigureAwait(false);
            return EmptyActivationRecord.Default;
        }

        protected override async Task<IActivationRecord> ExecuteSaveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            if (data == null)
            {
                return null;
            }
            var repository = GetRepositoryByConditionType(data.Type);
            if (repository == null)
            {
                return null;
            }
            await repository.SaveAsync(data, activationOption).ConfigureAwait(false);
            return EmptyActivationRecord.Default;
        }

        protected override async Task<TriggerCondition> GetDataAsync(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            if (repository == null)
            {
                return null;
            }
            return await repository.GetAsync(query).ConfigureAwait(false);
        }

        protected override async Task<List<TriggerCondition>> GetDataListAsync(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            if (repository == null)
            {
                return new List<TriggerCondition>(0);
            }
            return await repository.GetListAsync(query).ConfigureAwait(false);
        }

        protected override async Task<IPaging<TriggerCondition>> GetDataPagingAsync(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            if (repository == null)
            {
                return Paging<TriggerCondition>.EmptyPaging();
            }
            return await repository.GetPagingAsync(query).ConfigureAwait(false);
        }

        protected override async Task<bool> IsExistAsync(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            if (repository == null)
            {
                return false;
            }
            return await repository.ExistAsync(query).ConfigureAwait(false);
        }

        protected override async Task<VT> MaxValueAsync<VT>(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            VT value = default(VT);
            if (repository != null)
            {
                value = await repository.MaxAsync<VT>(query).ConfigureAwait(false);
            }
            return value;
        }

        protected override async Task<VT> MinValueAsync<VT>(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            VT value = default(VT);
            if (repository != null)
            {
                value = await repository.MinAsync<VT>(query).ConfigureAwait(false);
            }
            return value;
        }

        protected override async Task<VT> SumValueAsync<VT>(IQuery query)
        {
            var repository = GetRepositoryByQuery(query);
            VT value = default(VT);
            if (repository != null)
            {
                value = await repository.SumAsync<VT>(query).ConfigureAwait(false);
            }
            return value;
        }

        public override DataLifeSource GetLifeSource(IAggregationRoot data)
        {
            var triggerCondition = data as TriggerCondition;
            if (triggerCondition == null)
            {
                return DataLifeSource.New;
            }
            var repository = GetRepositoryByConditionType(triggerCondition.Type);
            if (repository == null)
            {
                return DataLifeSource.New;
            }
            return repository.GetLifeSource(triggerCondition);
        }

        public override void ModifyLifeSource(IAggregationRoot data, DataLifeSource lifeSource)
        {
            var triggerCondition = data as TriggerCondition;
            if (triggerCondition == null)
            {
                return;
            }
            var repository = GetRepositoryByConditionType(triggerCondition.Type);
            if (repository == null)
            {
                return;
            }
            repository.ModifyLifeSource(triggerCondition, lifeSource);
        }

        /// <summary>
        /// remove condition by trigger
        /// </summary>
        /// <param name="triggers"></param>
        public void RemoveTriggerConditionByTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            foreach (var repository in triggerConditionRepositoryDict.Values)
            {
                repository.RemoveTriggerConditionByTrigger(triggers, activationOption);
            }
        }


        /// <summary>
        /// 根据执行执行计划获取附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        /// <returns></returns>
        public List<TriggerCondition> GetTriggerConditionByTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return new List<TriggerCondition>();
            }
            List<TriggerCondition> conditions = new List<TriggerCondition>();
            triggerConditionRepositoryDict.Values.AsParallel<ITriggerConditionBaseRepository>().ForAll(c =>
            {
                conditions.AddRange(c.GetTriggerConditionByTrigger(triggers));
            });
            return conditions;
        }

        /// <summary>
        /// 保存执行计划中的附加条件
        /// </summary>
        /// <param name="triggers"></param>
        public void SaveTriggerConditionFromTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            foreach (var trigger in triggers)
            {
                var condition = trigger?.Condition;
                if (condition == null)
                {
                    continue;
                }
                condition.TriggerId = trigger.Id;
                triggerConditionRepositoryDict.TryGetValue(condition.Type, out var triggerConditionRepository);
                triggerConditionRepository?.Save(condition, activationOption);
            }
        }

        #endregion

        #region util

        /// <summary>
        /// get condition type by query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        TaskTriggerConditionType? GetConditonTypeByQuery(IQuery query)
        {
            if (query == null || query.EntityType == null)
            {
                return null;
            }
            if (!conditionTypeQueryModelDict.TryGetValue(query.EntityType.GUID, out TaskTriggerConditionType conditionType))
            {
                return null;
            }
            return conditionType;
        }

        /// <summary>
        /// get condition repository by query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ITriggerConditionBaseRepository GetRepositoryByQuery(IQuery query)
        {
            if (query == null)
            {
                return null;
            }
            var conditionType = GetConditonTypeByQuery(query);
            if (conditionType == null)
            {
                return null;
            }
            return GetRepositoryByConditionType(conditionType.Value);
        }

        /// <summary>
        /// get repository by condition type
        /// </summary>
        /// <param name="conditionType"></param>
        /// <returns></returns>
        ITriggerConditionBaseRepository GetRepositoryByConditionType(TaskTriggerConditionType conditionType)
        {
            triggerConditionRepositoryDict.TryGetValue(conditionType, out ITriggerConditionBaseRepository repository);
            return repository;
        }

        #endregion
    }
}
