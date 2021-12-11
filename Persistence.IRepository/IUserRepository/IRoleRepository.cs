using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.IRepository.IUserRepository
{
    public interface IRoleRepository
    {
        Task<bool> CreateRoleAsync(Role role);
        public Role GetRole(string RoleName);
        public List<Role> GetAllRole();
        public Role GetRoleById(string pId);
        public bool IsUsed(string pId);
        public Task<IdentityResult> UpdateRole(Role role);

        public Task<IdentityResult> RemoveRole(Role role);
        public List<Role> GetListOfRoles(
            Expression<Func<Role, bool>> predicate = null,
            Func<IQueryable<Role>, IIncludableQueryable<Role, object>> include = null);

        public List<Role> GetAllRoleByProjectId(string id, bool all = false);
    }
}
