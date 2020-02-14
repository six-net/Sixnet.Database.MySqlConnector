using EZNEW.Develop.DataAccess;
using EZNEW.Entity.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DataAccessContract.CTask
{
    /// <summary>
    /// 工作承载节点数据访问接口
    /// </summary>
    public interface IJobServerHostDataAccess : IDataAccess<JobServerHostEntity>
    {
    }

    /// <summary>
    /// 工作承载节点数据库接口
    /// </summary>
    public interface IJobServerHostDbAccess : IJobServerHostDataAccess
    {
    }
}
