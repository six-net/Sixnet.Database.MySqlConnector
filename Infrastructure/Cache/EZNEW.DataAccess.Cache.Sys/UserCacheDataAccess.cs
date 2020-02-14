using EZNEW.Data.Cache;
using EZNEW.DataAccessContract.Sys;
using EZNEW.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.DataAccess.Cache.Sys
{
    public class UserCacheDataAccess : DefaultCacheDataAccess<IUserDbAccess, UserEntity>, IUserDataAccess
    {
    }
}
