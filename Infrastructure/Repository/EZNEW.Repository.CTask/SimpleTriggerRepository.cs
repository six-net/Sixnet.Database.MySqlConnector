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
using EZNEW.CTask;
using EZNEW.Develop.CQuery;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Query.CTask;
using EZNEW.Develop.UnitOfWork;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 简单计划存储
    /// </summary>
    public class SimpleTriggerRepository : DefaultAggregationRepository<SimpleTrigger, TriggerSimpleEntity, ITriggerSimpleDataAccess>, ISimpleTriggerRepository
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void SaveSimpleTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<SimpleTrigger> simpleTriggers = triggers.Where(c => c.Type == TaskTriggerType.简单).Select(c => (SimpleTrigger)c);
            if (!simpleTriggers.IsNullOrEmpty())
            {
                Save(simpleTriggers, activationOption);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void RemoveSimpleTrigger(IEnumerable<Trigger> triggers, ActivationOption activationOption = null)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<SimpleTrigger> simpleTriggers = triggers.Where(c => c.Type == TaskTriggerType.简单).Select(c => (SimpleTrigger)c);
            if (simpleTriggers.IsNullOrEmpty())
            {
                return;
            }
            var entitys = simpleTriggers.Select(c =>
             {
                 return new TriggerSimpleEntity()
                 {
                     TriggerId = c.Id,
                     RepeatCount = c.RepeatCount,
                     RepeatForever = c.RepeatForever,
                     RepeatInterval = c.RepeatInterval
                 };
             });
            RemoveEntity(entitys, activationOption);
        }

        /// <summary>
        /// 获取简单计划
        /// </summary>
        /// <param name="triggers">计划数据</param>
        /// <returns></returns>
        public List<Trigger> LoadSimpleTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return new List<Trigger>(0);
            }
            List<Trigger> simpleTriggers = triggers.Where(c => c.Type == TaskTriggerType.简单).ToList();
            IEnumerable<string> triggerIds = simpleTriggers.Select(c => c.Id);
            List<TriggerSimpleEntity> simpleTriggerDatas = repositoryWarehouse.GetListAsync(QueryFactory.Create<TriggerSimpleQuery>(c => triggerIds.Contains(c.TriggerId))).Result;
            List<Trigger> newSimpleTriggers = new List<Trigger>();
            foreach (var trigger in triggers)
            {
                if (trigger.Type != TaskTriggerType.简单)
                {
                    newSimpleTriggers.Add(trigger);
                }
                else
                {
                    SimpleTrigger nowSimpleTrigger = null;
                    var nowSimpleData = simpleTriggerDatas.FirstOrDefault(s => s.TriggerId == trigger.Id);
                    if (nowSimpleData != null)
                    {
                        nowSimpleTrigger = nowSimpleData.MapTo<SimpleTrigger>();
                    }
                    else
                    {
                        nowSimpleTrigger = Trigger.CreateTrigger(id: trigger.Id) as SimpleTrigger;
                    }
                    nowSimpleTrigger.InitFromSimilarObject(trigger);
                    newSimpleTriggers.Add(nowSimpleTrigger);
                }
            }
            return newSimpleTriggers;
        }
    }
}
