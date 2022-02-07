using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.IRepository.IEntityRepository
{
    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        List<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);

        List<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                          bool disableTracking = true);

        List<TEntity> ToList(Expression<Func<TEntity, bool>> predicate = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                          bool disableTracking = true);

        public Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        List<TEntity> LoadHierarchy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        List<TEntity> GetAll();

        List<TEntity> GetAll(int count);

        List<TEntity> FromSqlRaw(string sql);

        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        TEntity GetSingle(Guid pId);

        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);

        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Add(TEntity entity);

        List<TEntity> AddRange(TEntity[] entities);
        public List<TEntity> UpdateRange(TEntity[] entities);
        TEntity Update(TEntity entity);
        public void UpdateDbSet(TEntity entity);
        List<TEntity> UpdateRing(TEntity[] entities);
        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);
        bool HasAny(TEntity entity);

        bool HasAny(Guid pId);
        bool HasAny(Expression<Func<TEntity, bool>> predicate);

        void SoftDelete(TEntity entity);

        void SoftDeleteRing(TEntity[] entities);

        void SoftDeleteWhere(Expression<Func<TEntity, bool>> predicate);

        //-------------------------------------------------------------

        void DeleteArc(TEntity entity);

        void DeleteRange(TEntity[] entities);

        void DeleteWhere( Expression<Func<TEntity, bool>> predicate, string UserName = "");
        void DeleteWherePermenant(Expression<Func<TEntity, bool>> predicate);

        public TEntity GetFirstOrDefaultRandom();

    }


}
