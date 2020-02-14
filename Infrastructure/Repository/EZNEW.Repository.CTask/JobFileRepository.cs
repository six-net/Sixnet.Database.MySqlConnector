using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Domain.CTask.Model;
using EZNEW.Domain.CTask.Repository;
using EZNEW.Entity.CTask;
using EZNEW.Framework.Extension;
using EZNEW.DataAccessContract.CTask;
using EZNEW.Develop.CQuery;
using EZNEW.Query.CTask;
using EZNEW.Develop.Domain.Repository;


namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 任务工作文件存储
    /// </summary>
    public class JobFileRepository : DefaultAggregationRepository<JobFile, JobFileEntity, IJobFileDataAccess>, IJobFileRepository
    {
    }
}
