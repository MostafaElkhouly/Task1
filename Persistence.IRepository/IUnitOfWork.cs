
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Persistence.IRepository.IEntityRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.IRepository
{
    public interface IUnitOfWork<T> where T : DbContext
    {
        long SaveChanges();
        long SaveChangesTransaction();

        DataTable Get(string sql, Dictionary<string, string> parameters);

       

        /// <summary>
        /// SaveAsync Inteface
        /// </summary>
        Task<long> SaveAsync();

        Task<long> SaveAsyncTransaction();

        /// <summary>
        /// Get Reposatory InterFace
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        //IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase;
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase;

        

        List<TStored> GetData<TStored>(string sp_name, Dictionary<string, object> parames = null,
            CommandType CommandType = System.Data.CommandType.StoredProcedure);
        public Task<List<TStored>> GetDataAsync<TStored>(string sp_name, Dictionary<string, object> parames = null
      , CommandType CommandType = System.Data.CommandType.StoredProcedure);

        public Task<List<TStored>> GetDataStored<TStored>(string storedName, List<StoredPramaters> pramaters = null);
        public DataTable CreateDataTable<E>(IEnumerable<E> list);

    }
}
