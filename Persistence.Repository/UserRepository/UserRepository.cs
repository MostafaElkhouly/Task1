using Domain.Configration.EntitiesProperties;
using Persistence.IRepository.IUserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Domain.Entities;
namespace Persistence.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        protected readonly DbSet<User> DbSet;
        protected readonly DbSet<UserRole> DbSetUserRole;
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        public UserRepository(UserManager<User> userManager, AppDbContext context)
        {
            DbSet = context.Set<User>();
            DbSetUserRole = context.Set<UserRole>();
            this.context = context;
            this.userManager = userManager;
        }
        public async Task<IdentityResult> RegisterUserAsync(User user)
        {
            return await userManager.CreateAsync(user);
        }

        public bool CheckPasswordsMatch(User user,string newPassword)
        {           
            PasswordVerificationResult passwordMatch = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, newPassword);
            return  (passwordMatch == PasswordVerificationResult.Success);
        }

        public async Task<IdentityResult> AddUserToRole(User user, string role)
        {
            return await userManager.AddToRoleAsync(user, role);
        }
        public Task<IdentityResult> ChangePassword(User user, string password)
        {
            var newPassword = userManager.PasswordHasher.HashPassword(user, password);
            user.PasswordHash = newPassword;
            return userManager.UpdateAsync(user);
        }
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }
        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }
        public void Delete(User entity)
        {
            try
            {
                EntityEntry dbEntityEntry = DbSet.Attach(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void DeleteRing(User[] entities)
        {
            foreach (var item in entities)
            {
                Delete(item);
            }
        }
        public void DeleteWhere(Expression<Func<User, bool>> predicate)
        {
            var entity = DbSet.Where(predicate).FirstOrDefault();
            try
            {
                EntityEntry dbEntityEntry = DbSet.Attach(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User> FindByEmailAsync(string Email)
        {
            return await userManager.FindByEmailAsync(Email);
        }
        public async Task<User> FindByIdAsync(string Id)
        {
            return await userManager.FindByIdAsync(Id);
        }
        public async Task<User> FindByNameAsync(string Name)
        {
            return await userManager.FindByNameAsync(Name);
        }
        public async Task<User> FindUser(Expression<Func<User, bool>> predicate)
        {
            return await this.DbSet.Where(predicate).FirstOrDefaultAsync();
        }
        public string GetRolesAsync(User pUser)
        {

            return userManager.GetRolesAsync(pUser).Result.FirstOrDefault();
        }
        public bool HasAny(Expression<Func<User, bool>> predicate)
        {
            return DbSet.Where(predicate).Any();
        }
        public async Task SoftDelete(User user)
        {
            var logins = await userManager.GetLoginsAsync(user);
            var rolesForUser = await userManager.GetRolesAsync(user);

            using (var transaction = context.Database.BeginTransaction())
            {
                IdentityResult result = IdentityResult.Success;
                foreach (var login in logins)
                {
                    result = await userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                    if (result != IdentityResult.Success)
                        break;
                }
                if (result == IdentityResult.Success)
                {
                    foreach (var item in rolesForUser)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, item);
                        if (result != IdentityResult.Success)
                            break;
                    }
                }
                if (result == IdentityResult.Success)
                {
                    result = await userManager.DeleteAsync(user);
                    if (result == IdentityResult.Success)
                        transaction.Commit(); //only commit if user and all his logins/roles have been deleted  
                }
            }
        }
        public async Task SoftDeleteRing(User[] entities)
        {
            foreach (var item in entities)
            {
                await SoftDelete(item);
            }
        }
        public void SoftDeleteWhere(Expression<Func<User, bool>> predicate)
        {
            var entity = DbSet.Where(predicate).FirstOrDefault();
            entity.IsDeleted = true;
            try
            {
                EntityEntry dbEntityEntry = DbSet.Attach(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Task<IdentityResult> UpdateUser(User user)
        {
            return userManager.UpdateAsync(user);
        }
        public List<User> GetListOfUsers(Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null)
        {
            IQueryable<User> query = DbSet;
            if (include != null)
            {
                query = include(query);
            }
            return DbSet.ToList();
        }

        public List<User> GetUsers( Expression<Func<User, bool>> predicate = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null)
        {
            IQueryable<User> query = DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }



            return query.ToList();
        }


        public User GetUserById(string Id)
        {
            return userManager.FindByIdAsync(Id).Result;
        }
        public async Task<User> GetUserByProvider(string loginProvider, string providerKey)
        {
            return await userManager.FindByLoginAsync(loginProvider, providerKey);
        }
        public async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo loginInfo)
        {
            return await userManager.AddLoginAsync(user, loginInfo);
        }
        public Task<IdentityResult> DeleteRolesAsync(string roleName, User user)
        {
            return userManager.RemoveFromRoleAsync(user, roleName);
        }

        public void DeleteRolesAsyncByRep(string roleId, User user)
        {
            var userRole = this.DbSetUserRole
                .Where(e => e.RoleId.ToLower().Equals(roleId.ToLower())
            && e.UserId.Equals(user.Id)).FirstOrDefault();
            if (userRole == null)
                throw new Exception("this User has no role");
            this.DbSetUserRole.Remove(userRole);
        }
        public Task<IdentityResult> DeleteAsync(User user)
        {
            
            return userManager.DeleteAsync(user);

        }
        public Task<IdentityResult> ChangePassword(User user, String token, string newPassword)
        {
            return userManager.ResetPasswordAsync(user, token, newPassword);
        }

    }
}