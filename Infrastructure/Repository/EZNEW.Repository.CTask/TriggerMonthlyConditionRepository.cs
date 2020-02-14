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
    /// 每月日期计划存储
    /// </summary>
    public class TriggerMonthlyConditionRepository : DefaultAggregationRepository<TriggerCondition, TriggerMonthlyConditionEntity, ITriggerMonthlyConditionDataAccess>, ITriggerMonthlyConditionRepository
    {
        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">要保存的数据</param>
        protected override async Task<IActivationRecord> ExecuteSaveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var monthlyCondition = data as TriggerMonthlyCondition;
            if (monthlyCondition == null || monthlyCondition.Days.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerMonthlyConditionEntity> monthlyEntityList = new List<TriggerMonthlyConditionEntity>();
            monthlyEntityList.AddRange(monthlyCondition.Days.Select(c =>
            {
                var entity = c.MapTo<TriggerMonthlyConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }).ToList());

            var packageRecord = DefaultActivationRecord<TriggerMonthlyConditionEntity, ITriggerMonthlyConditionDataAccess>.CreatePackageRecord();
            //移除当前的条件
            IQuery removeQuery = QueryFactory.Create<TriggerMonthlyConditionQuery>(c => c.TriggerId == data.TriggerId);
            Remove(removeQuery, activationOption);
            //添加新的条件
            return await SaveEntityAsync(monthlyEntityList, activationOption).ConfigureAwait(false);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="data">要移除的数据</param>
        protected override async Task<IActivationRecord> ExecuteRemoveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var monthlyCondition = data as TriggerMonthlyCondition;
            if (monthlyCondition == null || monthlyCondition.Days.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerMonthlyConditionEntity> monthlyEntityList = new List<TriggerMonthlyConditionEntity>();
            monthlyEntityList.AddRange(monthlyCondition.Days.Select(c =>
            {
                var entity = c.MapTo<TriggerMonthlyConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }));
            return await RemoveEntityAsync(monthlyEntityList, activationOption).ConfigureAwait(false);
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
            var query = QueryFactory.Create<TriggerMonthlyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
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
            List<TriggerMonthlyConditionEntity> monthlyEntityList = await repositoryWarehouse.GetListAsync(query);
            if (monthlyEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = monthlyEntityList.First().TriggerId;
            TriggerMonthlyCondition monthlyCondtion = new TriggerMonthlyCondition(triggerId)
            {
                Days = monthlyEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<MonthConditionDay>()).ToList()
            };
            return monthlyCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override async Task<List<TriggerCondition>> GetDataListAsync(IQuery query)
        {
            List<TriggerMonthlyConditionEntity> monthlyEntityList = await repositoryWarehouse.GetListAsync(query);
            if (monthlyEntityList.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            IEnumerable<string> triggerIds = monthlyEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerCondition> monthlyConditions = new List<TriggerCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerMonthlyCondition monthlyCondtion = new TriggerMonthlyCondition(triggerId)
                {
                    Days = monthlyEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<MonthConditionDay>()).ToList()
                };
                monthlyConditions.Add(monthlyCondtion);
            }
            return monthlyConditions;
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
            var query = QueryFactory.Create<TriggerMonthlyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            return GetList(query);
        }

        #endregion
    }
}
