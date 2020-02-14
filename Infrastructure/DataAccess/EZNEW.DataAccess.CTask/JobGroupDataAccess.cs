using EZNEW.Develop.DataAccess;
using EZNEW.DataAccessContract.CTask;
using EZNEW.Entity.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Develop.DataAccess;

namespace EZNEW.DataAccess.CTask
{
    /// <summary>
    /// 工作分组数据访问
    /// </summary>
    public class JobGroupDataAccess : RdbDataAccess<JobGroupEntity>, IJobGroupDataAccess
    {
    }
}
