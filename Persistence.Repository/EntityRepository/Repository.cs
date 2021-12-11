
using Persistence.IRepository.IEntityRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;

namespace Persistence.Repository.EntityRepository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        protected readonly DbSet<TEntity> DbSet;
        //private readonly AppDbContext dbContext;
        private readonly bool isAdmin;

        public Repository(DbContext context, bool isAdmin)
        {
            DbSet = context.Set<TEntity>();
            //DbSet.AsNoTracking();
            //dbContext = context;
            this.isAdmin = isAdmin;
        }
        public TEntity Add(TEntity entity)
        {
            try
            {
                EntityEntry dbEntityEntry = DbSet.Attach(entity);
                DbSet.Add(entity);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TEntity> AddRange(TEntity[] entities)
        {
            try
            {
                DbSet.AddRange(entities);
                return entities.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TEntity> UpdateRange(TEntity[] entities)
        {
            try
            {
                DbSet.UpdateRange(entities);
                return entities.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SoftDelete(TEntity entity)
        {
            entity.IsDeleted = !entity.IsDeleted;
            try
            {
                EntityEntry dbEntityEntry = DbSet.Attach(entity);
                dbEntityEntry.State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void SoftDeleteRing(TEntity[] entities)
        {
            foreach (var item in entities)
            {
                SoftDelete(item);
            }
        }


        public void SoftDeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = DbSet.Where(predicate).FirstOrDefault();
            entity.IsDeleted = true;
            try
            {
                EntityEntry dbEntityEntry = DbSet.Attach(entity);
                dbEntityEntry.State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TEntity Update(TEntity entity)
        {
            try
            {
                EntityEntry dbEntityEntry = DbSet.Attach(entity);
                dbEntityEntry.State = EntityState.Modified;
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDbSet(TEntity entity)
        {
            try
            {
                DbSet.Update(entity);
            }catch(Exception)
            {
                throw;
            }
        }

        public List<TEntity> UpdateRing(TEntity[] entities)
        {
            foreach (var item in entities)
            {
                Update(item);
            }

            return entities.ToList();
        }

        public List<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = null;
                if (!isAdmin)
                {
                    query = DbSet.Where(e => e.IsDeleted == false);
                }
                else
                {
                    query = DbSet;
                }
                
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Count()
        {
            try
            {
                if (isAdmin)
                {
                    return DbSet.Count();
                }
                else
                {
                    return DbSet.Where(e => e.IsDeleted == false).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {

                IQueryable<TEntity> query = null;

                if (isAdmin)
                {
                    query = DbSet;
                }
                else
                {
                    query = DbSet.Where(e => e.IsDeleted == false);
                }

                return query.Where(predicate).Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TEntity> GetAll()
        {
            try
            {
                if (isAdmin)
                {
                    return DbSet.ToList();
                }
                else
                {
                    //return DbSet.Where(e => e.IsDeleted == false).ToList();
                    return DbSet.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                //IQueryable<TEntity> query = null;
                //if (isAdmin)
                //{
                //    query = DbSet;
                //}
                //else
                //{
                //    query = DbSet.Where(e => e.IsDeleted == false);
                //}
                return DbSet.Where(predicate).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                var query = DbSet.Where(predicate);

                if (!isAdmin)
                {
                    //query = query.Where(e => e.IsDeleted == false);
                }

                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }


        }

        public TEntity GetSingle(Guid pId)
        {

            //var test = EqualityComparer<TKey>.Default.Equals(pId, pId);
            
            //var npId = Convert.ToInt32(pId);
            //var npId = Convert.ToInt64(pId);
            var query = DbSet.AsQueryable();
            try
            {
                if (isAdmin)
                {
                    //return DbSet.Where(e => e.Id == pId).FirstOrDefault();
                    return query.Where(e => e.Id == pId).FirstOrDefault();
                }
                return query.Where(e => e.Id == pId).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                //if (isAdmin)
                //{
                //    return DbSet.Where(predicate).FirstOrDefault();
                //}
                return DbSet.AsTracking().Where(predicate).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                var query = DbSet.Where(predicate);

                if (!isAdmin)
                {
                    query = query.Where(e => e.IsDeleted == false);
                }
                
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return query.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool HasAny(TEntity entity)
        {
            try
            {
                if (isAdmin)
                {
                    return DbSet.Where(e => e == entity).Any();
                }
                return DbSet.Where(e => e == entity).Any();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool HasAny(Guid pId)
        {
            try
            {
                if (isAdmin)
                {
                    return DbSet.Where(e => e.Id == pId).Any();
                }
                return DbSet.Where(e => e.Id == pId).Any();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool HasAny(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                if (isAdmin)
                {
                    return DbSet.Where(predicate).Any();
                }
                return DbSet.Where(predicate).Any();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TEntity> LoadHierarchy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;
                //query.Where(predicate);
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                if (isAdmin)
                {
                    return query.Where(predicate).ToList();
                }

                return query.Where(predicate).ToList();
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if(!isAdmin)
                //query = query.Where(e => e.IsDeleted == false);

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            else
            {
                query = query.AsTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }
            else
            {
                return query.FirstOrDefault();
            }
        }

        public List<TEntity> ToList(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            //if(!isAdmin)
            //    query = query.Where(e => e.IsDeleted == false);

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            //if(!isAdmin)
            //    query = query.Where(e => e.IsDeleted == false);

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public List<TEntity> GetAll(int count)
        {
            try
            {
                return DbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-------------------------------------------------------------------
        public void DeleteArc(TEntity entity)
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

        public void DeleteRange(TEntity[] entities)
        {
            foreach (var item in entities)
            {
                DeleteArc(item);
            }
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate, string UserName = "")
        {
            try
            {
                var entites = DbSet.Where(predicate);
                foreach (var entity in entites)
                {
                   // entity.UserName=UserName;
                    DbSet.Attach(entity).State = EntityState.Deleted;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void DeleteWherePermenant(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entites = DbSet.Where(predicate);
                DbSet.RemoveRange(entites);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<TEntity> FromSqlRaw(string sql)
        {
           return DbSet.FromSqlRaw(sql)
                .ToList();
        }
        public TEntity GetFirstOrDefaultRandom()
        {
            try
            {

                IQueryable<TEntity> query = DbSet;
                return query.OrderBy(r => Guid.NewGuid()).FirstOrDefault();


            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    
}

