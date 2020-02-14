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
    /// 任务文件数据访问接口
    /// </summary>
    public interface IJobFileDataAccess:IDataAccess<JobFileEntity>
    {
    }

	/// <summary>
    /// 任务文件数据库接口
    /// </summary>
    public interface IJobFileDbAccess:IJobFileDataAccess
    {
    }
}
