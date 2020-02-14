using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Domain.Sys.Model;
using EZNEW.Domain.Sys.Repository;
using EZNEW.Entity.Sys;
using EZNEW.Framework.Extension;
using EZNEW.DataAccessContract.Sys;
using EZNEW.Query.Sys;
using EZNEW.Develop.Domain.Repository;
using EZNEW.Develop.CQuery;

namespace EZNEW.Repository.Sys
{
    /// <summary>
    /// 用户授权存储
    /// </summary>
    public class UserAuthorizeRepository : DefaultAggregationRelationRepository<UserAuthorize, User, Authority, UserAuthorizeEntity, IUserAuthorizeDataAccess>, IUserAuthorizeRepository
    {
        public override IQuery CreateQueryByFirst(IEnumerable<User> datas)
        {
            if (datas.IsNullOrEmpty())
            {
                return null;
            }
            var userIds = datas.Select(c => c.SysNo);
            return QueryFactory.Create<UserAuthorizeQuery>(c => userIds.Contains(c.User));
        }

        public override IQuery CreateQueryByFirst(IQuery query)
        {
            if (query == null)
            {
                return null;
            }
            var copyQuery = query.Clone();
            copyQuery.QueryFields.Clear();
            copyQuery.AddQueryFields<UserQuery>(c => c.SysNo);
            var removeQuery = QueryFactory.Create<UserAuthorizeQuery>();
            removeQuery.And<UserAuthorizeQuery>(ur => ur.User, CriteriaOperator.In, copyQuery);
            return removeQuery;
        }

        public override IQuery CreateQueryBySecond(IEnumerable<Authority> datas)
        {
            if (datas.IsNullOrEmpty())
            {
                return null;
            }
            var authCodes = datas.Select(c => c.Code);
            return QueryFactory.Create<UserAuthorizeQuery>(c => authCodes.Contains(c.Authority));
        }

        public override IQuery CreateQueryBySecond(IQuery query)
        {
            if (query == null)
            {
                return null;
            }
            var copyQuery = query.Clone();
            copyQuery.QueryFields.Clear();
            copyQuery.AddQueryFields<AuthorityQuery>(c => c.Code);
            var removeQuery = QueryFactory.Create<UserAuthorizeQuery>();
            removeQuery.And<UserAuthorizeQuery>(ur => ur.Authority, CriteriaOperator.In, copyQuery);
            return removeQuery;
        }
    }
}
