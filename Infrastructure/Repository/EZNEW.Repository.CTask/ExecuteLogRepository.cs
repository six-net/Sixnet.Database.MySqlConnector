using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Domain.CTask.Model;
using EZNEW.Entity.CTask;
using EZNEW.DataAccessContract.CTask;
using EZNEW.Domain.CTask.Repository;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 任务执行日志存储
    /// </summary>
    public class ExecuteLogRepository : DefaultAggregationRepository<ExecuteLog, ExecuteLogEntity, IExecuteLogDataAccess>, IExecuteLogRepository
    {
    }
}
