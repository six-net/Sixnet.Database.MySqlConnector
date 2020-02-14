using EZNEW.DataAccessContract.CTask;
using EZNEW.Develop.DataAccess;
using EZNEW.Entity.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DataAccess.CTask
{
    /// <summary>
    /// 任务工作文件数据访问
    /// </summary>
    public class JobFileDataAccess : RdbDataAccess<JobFileEntity>, IJobFileDbAccess
    {
    }
}
