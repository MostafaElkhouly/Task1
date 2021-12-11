
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
    public interface IUserRepository
    {
        public Task<IdentityResult> RegisterUserAsync(User user);
        public bool CheckPasswordsMatch(User user, string newPassword);

        Task<User> FindByNameAsync(string Name);
        Task<User> FindByEmailAsync(string Email);
        Task<User> FindByIdAsync(string Id);
        Task<User> FindUser(Expression<Func<User, bool>> predicate);
        bool HasAny(Expression<Func<User, bool>> predicate);
        string GetRolesAsync(User pUser);
        void Delete(User entity);
        void DeleteRing(User[] entities);
        void DeleteWhere(Expression<Func<User, bool>> predicate);
        Task SoftDelete(User user);
        Task SoftDeleteRing(User[] entities);
        void SoftDeleteWhere(Expression<Func<User, bool>> predicate);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> AddUserToRole(User user, string role);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> UpdateUser(User user);
        Task<IdentityResult> ChangePassword(User user, string password);
        public List<User> GetListOfUsers(Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null);
        public List<User> GetUsers(Expression<Func<User, bool>> predicate = null, Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null);
        public User GetUserById(string Id);
        public Task<User> GetUserByProvider(string loginProvider, string providerKey);
        public Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo loginInfo);
        public Task<IdentityResult> DeleteRolesAsync(string roleName, User user);
        public void DeleteRolesAsyncByRep(string roleId, User user);
        public Task<IdentityResult> ChangePassword(User user, String token, string newPassword);
        public Task<IdentityResult> DeleteAsync(User user);
    }
}