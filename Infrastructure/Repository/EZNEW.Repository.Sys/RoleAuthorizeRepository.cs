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
using EZNEW.Application.Identity.Auth;
using EZNEW.Develop.CQuery;
using EZNEW.Develop.UnitOfWork;
using EZNEW.Query.Sys;
using EZNEW.Develop.Domain.Repository;

namespace EZNEW.Repository.Sys
{
    /// <summary>
    /// 角色权限存储
    /// </summary>
    public class RoleAuthorizeRepository : DefaultRelationRepository<Role, Authority, RoleAuthorizeEntity, IRoleAuthorizeDataAccess>, IRoleAuthorizeRepository
    {
        public override RoleAuthorizeEntity CreateEntityByRelationData(Tuple<Role, Authority> data)
        {
            return new RoleAuthorizeEntity()
            {
                Role = data.Item1.SysNo,
                Authority = data.Item2.Code
            };
        }

        public override IQuery CreateQueryByFirst(IEnumerable<Role> datas)
        {
            if (datas.IsNullOrEmpty())
            {
                return null;
            }
            IEnumerable<long> roleIds = datas.Select(c => c.SysNo);
            IQuery query = QueryFactory.Create<RoleAuthorizeQuery>(c => roleIds.Contains(c.Role));
            return query;
        }

        /// <summary>
        /// create query by first
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IQuery CreateQueryByFirst(IQuery query)
        {
            if (query == null)
            {
                return null;
            }
            var copyQuery = query.Clone();
            copyQuery.QueryFields.Clear();
            copyQuery.AddQueryFields<RoleQuery>(c => c.SysNo);
            var removeQuery = QueryFactory.Create<RoleAuthorizeQuery>();
            removeQuery.And<RoleAuthorizeQuery>(ur => ur.Role, CriteriaOperator.In, copyQuery);
            return removeQuery;
        }

        public override IQuery CreateQueryBySecond(IEnumerable<Authority> datas)
        {
            if (datas.IsNullOrEmpty())
            {
                return null;
            }
            IEnumerable<string> authIds = datas.Select(c => c.Code);
            IQuery query = QueryFactory.Create<RoleAuthorizeQuery>(c => authIds.Contains(c.Authority));
            return query;
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
            var removeQuery = QueryFactory.Create<RoleAuthorizeQuery>();
            removeQuery.And<RoleAuthorizeQuery>(ur => ur.Authority, CriteriaOperator.In, copyQuery);
            return removeQuery;
        }

        public override Tuple<Role, Authority> CreateRelationDataByEntity(RoleAuthorizeEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new Tuple<Role, Authority>(Role.CreateRole(entity.Role), Authority.CreateAuthority(entity.Authority));
        }
    }
}
