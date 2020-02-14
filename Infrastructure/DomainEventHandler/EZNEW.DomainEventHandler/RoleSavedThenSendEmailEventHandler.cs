using EZNEW.Develop.Domain.Event;
using EZNEW.Domain.Sys.Event;
using System;
using System.Threading.Tasks;

namespace EZNEW.DomainEventHandler
{
    /// <summary>
    /// 角色保存后发送邮件
    /// </summary>
    public class RoleSavedThenSendEmailEventHandler : IDomainEventHandler
    {
        public EventTriggerTime ExecuteTime => EventTriggerTime.WorkCompleted;//工作单元成功提交后触发

        public DomainEventExecuteResult Execute(IDomainEvent domainEvent)
        {
            return ExecuteAsync(domainEvent).Result;
        }

        public async Task<DomainEventExecuteResult> ExecuteAsync(IDomainEvent domainEvent)
        {
            var saveRoleEvent = domainEvent as SaveRoleEvent;
            if (saveRoleEvent?.Role == null)
            {
                return DomainEventExecuteResult.EmptyResult();
            }
            Console.WriteLine("添加角色{0}，邮件发送成功", saveRoleEvent.Role.Name);
            return await Task.FromResult(DomainEventExecuteResult.CodeResult(DomainEventExecuteResultCode.Success));
        }
    }
}
