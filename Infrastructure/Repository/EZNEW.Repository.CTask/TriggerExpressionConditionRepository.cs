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
using EZNEW.Query.CTask;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Develop.Entity;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 自定义附加计划存储
    /// </summary>
    public class TriggerExpressionConditionRepository : DefaultAggregationRepository<TriggerCondition, TriggerExpressionConditionEntity, ITriggerExpressionConditionDataAccess>, ITriggerExpressionConditionRepository
    {
        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">要保存的数据</param>
        protected override async Task<IActivationRecord> ExecuteSaveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var expressionCondition = data as TriggerExpressionCondition;
            if (expressionCondition == null || expressionCondition.ExpressionItems.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = new List<TriggerExpressionConditionEntity>();
            expressionConditionEntityList.AddRange(expressionCondition.ExpressionItems.Select(c =>
            {
                var entity = c.MapTo<TriggerExpressionConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }).ToList());
            //移除当前的条件
            IQuery removeQuery = QueryFactory.Create<TriggerExpressionConditionQuery>(c => c.TriggerId == data.TriggerId);
            Remove(removeQuery, activationOption);
            //添加新的条件
            return await SaveEntityAsync(expressionConditionEntityList, activationOption).ConfigureAwait(false);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="data">要移除的数据</param>
        protected override async Task<IActivationRecord> ExecuteRemoveAsync(TriggerCondition data, ActivationOption activationOption = null)
        {
            var expressionCondition = data as TriggerExpressionCondition;
            if (expressionCondition == null || expressionCondition.ExpressionItems.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = new List<TriggerExpressionConditionEntity>();
            expressionConditionEntityList.AddRange(expressionCondition.ExpressionItems.Select(c =>
            {
                var entity = c.MapTo<TriggerExpressionConditionEntity>();
                entity.TriggerId = data.TriggerId;
                return entity;
            }));
            return await RemoveEntityAsync(expressionConditionEntityList, activationOption).ConfigureAwait(false);
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
            var query = QueryFactory.Create<TriggerExpressionConditionQuery>(c => triggerIds.Contains(c.TriggerId));
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
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = await repositoryWarehouse.GetListAsync(query);
            if (expressionConditionEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = expressionConditionEntityList.First().TriggerId;
            TriggerExpressionCondition expressionCondtion = new TriggerExpressionCondition(triggerId)
            {
                ExpressionItems = expressionConditionEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<ExpressionItem>()).ToList()
            };
            return expressionCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override async Task<List<TriggerCondition>> GetDataListAsync(IQuery query)
        {
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = await repositoryWarehouse.GetListAsync(query);
            if (expressionConditionEntityList.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            IEnumerable<string> triggerIds = expressionConditionEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerCondition> expressionConditions = new List<TriggerCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerExpressionCondition expressionCondtion = new TriggerExpressionCondition(triggerId)
                {
                    ExpressionItems = expressionConditionEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<ExpressionItem>()).ToList()
                };
                expressionConditions.Add(expressionCondtion);
            }
            return expressionConditions;
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
            var query = QueryFactory.Create<TriggerExpressionConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            return GetList(query);
        }

        #endregion
    }
}
