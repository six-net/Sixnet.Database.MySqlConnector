using EZNEW.Develop.CQuery;
using EZNEW.DataAccessContract.CTask;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using EZNEW.Entity.CTask;
using EZNEW.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Query.CTask;
using EZNEW.Develop.Entity;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 星期计划存储
    /// </summary>
    public class TriggerWeeklyConditionRepository : DefaultAggregationRepository<TriggerCondition, TriggerWeeklyConditionEntity, ITriggerWeeklyConditionDataAccess>, ITriggerWeeklyConditionRepository
    {
        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">要保存的数据</param>
        protected override async Task<IActivationRecord> ExecuteSaveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var weeklyCondition = data as TriggerWeeklyCondition;
            if (weeklyCondition == null || weeklyCondition.Days.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerWeeklyConditionEntity> weekEntityList = new List<TriggerWeeklyConditionEntity>();
            weekEntityList.AddRange(weeklyCondition.Days.Select(c =>
            {
                var entity = c.MapTo<TriggerWeeklyConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }).ToList());
            //移除当前的条件
            IQuery removeQuery = QueryFactory.Create<TriggerWeeklyConditionQuery>(c => c.TriggerId == data.TriggerId);
            await RemoveAsync(removeQuery, activationOption);
            //添加新的条件
            return await SaveEntityAsync(weekEntityList, activationOption).ConfigureAwait(false);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="data">要移除的数据</param>
        protected override async Task<IActivationRecord> ExecuteRemoveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var weeklyCondition = data as TriggerWeeklyCondition;
            if (weeklyCondition == null || weeklyCondition.Days.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerWeeklyConditionEntity> weekEntityList = new List<TriggerWeeklyConditionEntity>();
            weekEntityList.AddRange(weeklyCondition.Days.Select(c =>
            {
                var entity = c.MapTo<TriggerWeeklyConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }));
            return await RemoveEntityAsync(weekEntityList, activationOption).ConfigureAwait(false);
        }

        /// <summary>
        /// 移除计划附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        public void RemoveTriggerConditionByTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            List<string> triggerIds = triggers.Select(c => c.Id).Distinct().ToList();
            var query = QueryFactory.Create<TriggerWeeklyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            Remove(query, activationOption);
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override async Task<TriggerCondition> GetDataAsync(IQuery query)
        {
            List<TriggerWeeklyConditionEntity> weekEntityList = await repositoryWarehouse.GetListAsync(query);
            if (weekEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = weekEntityList.First().TriggerId;
            TriggerWeeklyCondition weekCondtion = new TriggerWeeklyCondition(triggerId)
            {
                Days = weekEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<WeeklyConditionDay>()).ToList()
            };
            return weekCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override async Task<List<TriggerCondition>> GetDataListAsync(IQuery query)
        {
            List<TriggerWeeklyConditionEntity> weekEntityList = await repositoryWarehouse.GetListAsync(query);
            if (weekEntityList.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            IEnumerable<string> triggerIds = weekEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerCondition> weekConditions = new List<TriggerCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerWeeklyCondition weekCondtion = new TriggerWeeklyCondition(triggerId)
                {
                    Days = weekEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<WeeklyConditionDay>()).ToList()
                };
                weekConditions.Add(weekCondtion);
            }
            return weekConditions;
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
            List<string> triggerIds = triggers.Select(c => c.Id).Distinct().ToList();
            var query = QueryFactory.Create<TriggerWeeklyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            return GetList(query);
        }

        #endregion
    }
}
