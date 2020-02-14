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
using EZNEW.Develop.UnitOfWork;
using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Query.CTask;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 固定日期存储
    /// </summary>
    public class TriggerFullDateConditionRepository : DefaultAggregationRepository<TriggerCondition, TriggerFullDateConditionEntity, ITriggerFullDateConditionDataAccess>, ITriggerFullDateConditionRepository
    {
        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">要保存的数据</param>
        protected override async Task<IActivationRecord> ExecuteSaveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var fullDateCondition = data as TriggerFullDateCondition;
            if (fullDateCondition == null || fullDateCondition.Dates.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerFullDateConditionEntity> fullDateEntityList = new List<TriggerFullDateConditionEntity>();
            fullDateEntityList.AddRange(fullDateCondition.Dates.Select(c =>
            {
                var entity = c.MapTo<TriggerFullDateConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }).ToList());

            var packageRecord = DefaultActivationRecord<TriggerFullDateConditionEntity, ITriggerFullDateConditionDataAccess>.CreatePackageRecord();
            //移除当前的条件
            IQuery removeQuery = QueryFactory.Create<TriggerFullDateConditionQuery>(c => c.TriggerId == data.TriggerId);
            Remove(removeQuery, activationOption);
            //添加新的条件
            return await SaveEntityAsync(fullDateEntityList, activationOption).ConfigureAwait(false);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="data">要移除的数据</param>
        protected override async Task<IActivationRecord> ExecuteRemoveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var fullDateCondition = data as TriggerFullDateCondition;
            if (fullDateCondition == null || fullDateCondition.Dates.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerFullDateConditionEntity> fullDateConditionList = new List<TriggerFullDateConditionEntity>();
            fullDateConditionList.AddRange(fullDateCondition.Dates.Select(c =>
            {
                var entity = c.MapTo<TriggerFullDateConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }));
            return await RemoveEntityAsync(fullDateConditionList, activationOption).ConfigureAwait(false);
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
            var query = QueryFactory.Create<TriggerFullDateConditionQuery>(c => triggerIds.Contains(c.TriggerId));
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
            List<TriggerFullDateConditionEntity> fullDateEntityList = await repositoryWarehouse.GetListAsync(query);
            if (fullDateEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = fullDateEntityList.First().TriggerId;
            TriggerFullDateCondition fullDateCondtion = new TriggerFullDateCondition(triggerId)
            {
                Dates = fullDateEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<FullDateConditionDate>()).ToList()
            };
            return fullDateCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override async Task<List<TriggerCondition>> GetDataListAsync(IQuery query)
        {
            List<TriggerFullDateConditionEntity> fullDateEntityList = await repositoryWarehouse.GetListAsync(query);
            if (fullDateEntityList.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            IEnumerable<string> triggerIds = fullDateEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerCondition> fullDateConditions = new List<TriggerCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerFullDateCondition fullDateCondtion = new TriggerFullDateCondition(triggerId)
                {
                    Dates = fullDateEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<FullDateConditionDate>()).ToList()
                };
                fullDateConditions.Add(fullDateCondtion);
            }
            return fullDateConditions;
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
            var query = QueryFactory.Create<TriggerFullDateConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            return GetList(query);
        }

        #endregion
    }
}
