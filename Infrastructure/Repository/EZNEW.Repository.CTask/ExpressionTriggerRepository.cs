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
using EZNEW.CTask;
using EZNEW.Develop.CQuery;
using EZNEW.Query.CTask;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Domain.CTask.Repository;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 自定义表达式计划存储
    /// </summary>
    public class ExpressionTriggerRepository : DefaultAggregationRepository<ExpressionTrigger, TriggerExpressionEntity, ITriggerExpressionDataAccess>, IExpressionTriggerRepository
    {
        #region 接口方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void SaveExpressionTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<ExpressionTrigger> expressionTriggers = triggers.Where(c => c.Type == TaskTriggerType.自定义).Select(c => (ExpressionTrigger)c);
            if (!expressionTriggers.IsNullOrEmpty())
            {
                Save(expressionTriggers, activationOption);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void RemoveExpressionTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<ExpressionTrigger> expressionTriggers = triggers.Where(c => c.Type == TaskTriggerType.自定义).Select(c => (ExpressionTrigger)c);
            if (expressionTriggers.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerExpressionEntity> expressionEntityList = new List<TriggerExpressionEntity>();
            foreach (var expressionTrigger in expressionTriggers)
            {
                if (expressionTrigger == null || expressionTrigger.ExpressionItems.IsNullOrEmpty())
                {
                    continue;
                }
                expressionEntityList.AddRange(expressionTrigger.ExpressionItems.Select(c =>
                {
                    var entity = c.MapTo<TriggerExpressionEntity>();
                    entity.TriggerId = expressionTrigger.Id;
                    return entity;
                }));
            }
            RemoveEntity(expressionEntityList, activationOption);
        }

        /// <summary>
        /// 获取简单计划
        /// </summary>
        /// <param name="triggers">计划数据</param>
        /// <returns></returns>
        public List<Trigger> LoadExpressionTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return new List<Trigger>(0);
            }
            List<Trigger> expressionTriggers = triggers.Where(c => c.Type == TaskTriggerType.自定义).ToList();
            List<string> triggerIds = expressionTriggers.Select(c => c.Id).ToList();
            List<TriggerExpressionEntity> expressionTriggerDatas = repositoryWarehouse.GetListAsync(QueryFactory.Create<TriggerExpressionQuery>(c => triggerIds.Contains(c.TriggerId))).Result;
            List<Trigger> newExpressionTriggers = new List<Trigger>();
            foreach (var trigger in triggers)
            {
                if (trigger.Type != TaskTriggerType.自定义)
                {
                    newExpressionTriggers.Add(trigger);
                }
                else
                {

                    ExpressionTrigger nowExpressionTrigger = Trigger.CreateTrigger(trigger.Id, TaskTriggerType.自定义) as ExpressionTrigger;
                    nowExpressionTrigger.InitFromSimilarObject(trigger);//初始化信息
                    var nowExpressionDatas = expressionTriggerDatas.Where(s => s.TriggerId == trigger.Id);
                    if (!nowExpressionDatas.IsNullOrEmpty())
                    {
                        nowExpressionTrigger.ExpressionItems = nowExpressionDatas.Select(c => c.MapTo<ExpressionItem>()).ToList();
                    }
                    newExpressionTriggers.Add(nowExpressionTrigger);
                }
            }
            return newExpressionTriggers;
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存计划表达式
        /// </summary>
        /// <param name="data">要保存的数据</param>
        /// <param name="activationOption">操作记录配置</param>
        protected override async Task<IActivationRecord> ExecuteSaveAsync(ExpressionTrigger data, ActivationOption activationOption = null)
        {
            if (data == null || data.ExpressionItems.IsNullOrEmpty())
            {
                return null;
            }
            List<TriggerExpressionEntity> expressionEntitys = new List<TriggerExpressionEntity>();
            expressionEntitys.AddRange(data.ExpressionItems.Select(c =>
            {
                var expression = c.MapTo<TriggerExpressionEntity>();
                expression.TriggerId = data.Id;
                return expression;
            }));
            //移除现有数据
            IQuery removeQuery = QueryFactory.Create<TriggerExpressionQuery>(c => c.TriggerId == data.Id);
            Remove(removeQuery, activationOption);
            //保存数据
            return await SaveEntityAsync(expressionEntitys, activationOption).ConfigureAwait(false);
        }

        #endregion
    }
}
