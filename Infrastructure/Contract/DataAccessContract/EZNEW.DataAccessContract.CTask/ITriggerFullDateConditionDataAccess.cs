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
    /// 计划完整日期条件数据访问接口
    /// </summary>
    public interface ITriggerFullDateConditionDataAccess : IDataAccess<TriggerFullDateConditionEntity>
    {
    }

    /// <summary>
    /// 计划完整日期条件数据库接口
    /// </summary>
    public interface ITriggerFullDateConditionDbAccess : ITriggerFullDateConditionDataAccess
    {
    }
}
