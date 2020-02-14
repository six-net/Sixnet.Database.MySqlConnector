using EZNEW.DataAccessContract.CTask;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using EZNEW.Entity.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Query.CTask;
using EZNEW.Develop.Entity;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 年度计划存储
    /// </summary>
    public class TriggerAnnualConditionRepository : DefaultAggregationRepository<TriggerCondition, TriggerAnnualConditionEntity, ITriggerAnnualConditionDataAccess>, ITriggerAnnualConditionRepository
    {
        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">要保存的数据</param>
        protected override async Task<IActivationRecord> ExecuteSaveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var annualCondition = data as TriggerAnnualCondition;
            if (annualCondition == null || annualCondition.Days.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerAnnualConditionEntity> annualDaysEntityList = new List<TriggerAnnualConditionEntity>();
            annualDaysEntityList.AddRange(annualCondition.Days.Select(c =>
            {
                var entity = c.MapTo<TriggerAnnualConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }).ToList());
            //移除当前的条件
            IQuery removeQuery = QueryFactory.Create<TriggerAnnualConditionQuery>(c => c.TriggerId == data.TriggerId);
            Remove(removeQuery, activationOption);
            //添加信息的计划
            return await SaveEntityAsync(annualDaysEntityList, activationOption).ConfigureAwait(false);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="data">要移除的数据</param>
        protected override async Task<IActivationRecord> ExecuteRemoveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var annualCondition = data as TriggerAnnualCondition;
            if (annualCondition == null || annualCondition.Days.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerAnnualConditionEntity> annualDaysEntityList = new List<TriggerAnnualConditionEntity>();
            annualDaysEntityList.AddRange(annualCondition.Days.Select(c =>
            {
                var entity = c.MapTo<TriggerAnnualConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }));
            return await RemoveEntityAsync(annualDaysEntityList, activationOption).ConfigureAwait(false);
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
            var query = QueryFactory.Create<TriggerAnnualConditionQuery>(c => triggerIds.Contains(c.TriggerId));
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
            List<TriggerAnnualConditionEntity> annualDaysEntityList = await repositoryWarehouse.GetListAsync(query);
            if (annualDaysEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = annualDaysEntityList.First().TriggerId;
            TriggerAnnualCondition annualCondtion = new TriggerAnnualCondition(triggerId)
            {
                Days = annualDaysEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<AnnualConditionDay>()).ToList()
            };
            return annualCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override async Task<List<TriggerCondition>> GetDataListAsync(IQuery query)
        {
            List<TriggerAnnualConditionEntity> annualDaysEntityList = await repositoryWarehouse.GetListAsync(query);
            if (annualDaysEntityList.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            IEnumerable<string> triggerIds = annualDaysEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerCondition> annualConditions = new List<TriggerCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerAnnualCondition annualCondtion = new TriggerAnnualCondition(triggerId)
                {
                    Days = annualDaysEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<AnnualConditionDay>()).ToList()
                };
                annualConditions.Add(annualCondtion);
            }
            return annualConditions;
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
            var query = QueryFactory.Create<TriggerAnnualConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            return GetList(query);
        }

        #endregion
    }
}
