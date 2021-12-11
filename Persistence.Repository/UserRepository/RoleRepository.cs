using Persistence.IRepository.IUserRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Configration.EntitiesProperties;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Domain.Entities;

namespace Persistence.Repository.UserRepository
{
    public class RoleRepository : IRoleRepository
    {
        private RoleManager<Role> roleManager;
        protected readonly DbSet<Role> DbSet;
        private readonly DbSet<UserRole> DbSetUserRole;

        public RoleRepository(RoleManager<Role> roleManager, AppDbContext context)
        {
            DbSet = context.Set<Role>();
            DbSetUserRole = context.UserRoles;
            this.roleManager = roleManager;
        }

        public Role GetRole(string RoleName)
        {
            return DbSet.Where(e => e.NormalizedName.Equals(RoleName.ToUpper())).FirstOrDefault();
        }

        public async Task<bool> CreateRoleAsync(Role role)
        {
            try
            {
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                    return true;
                else
                {
                    foreach (var item in result.Errors)
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return false;
        }

        public List<Role> GetAllRole()
        {
            return DbSet.ToList();
        }

        public List<Role> GetAllRoleByProjectId(string id, bool all = false)
        {
            return DbSet.Where(p => p.ProjectId.ToString().Equals(id) && p.IsDeleted == all && p.AccessAdminPanel == true).ToList();
        }


        public List<Role> GetListOfRoles(
            Expression<Func<Role, bool>> predicate = null,
            Func<IQueryable<Role>, IIncludableQueryable<Role, object>> include = null)
        {
            IQueryable<Role> query = DbSet;

            if(include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }



            return query.ToList();
        }

        

        public bool IsUsed(string pId)
        {
            var test = this.DbSetUserRole.Where(e => e.RoleId.Equals(pId)).Include(e => e.User).FirstOrDefault();
            return test != null;
        }

        public async Task<IdentityResult> UpdateRole(Role role)
        {
            
            return await roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> RemoveRole(Role role)
        {
            return await roleManager.DeleteAsync(role);
        }

        public Role GetRoleById(string pId)
        {
            return DbSet.Where(e => e.Id.Equals(pId)).FirstOrDefault();
        }
    }
}
