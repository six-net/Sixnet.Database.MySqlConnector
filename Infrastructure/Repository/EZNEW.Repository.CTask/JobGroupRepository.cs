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
using EZNEW.Domain.CTask.Repository;

namespace EZNEW.Repository.CTask
{
    /// <summary>
    /// 工作分组存储
    /// </summary>
    public class JobGroupRepository : DefaultAggregationRepository<JobGroup, JobGroupEntity, IJobGroupDataAccess>,IJobGroupRepository
    {
    }
}
