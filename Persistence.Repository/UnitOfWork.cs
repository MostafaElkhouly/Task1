
using Persistence.Repository.EntityRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using Domain.Configration.EntitiesProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Persistence.IRepository.IEntityRepository;
using Domain.Entities.Base;
using Persistence.IRepository;

namespace Persistence.Repository
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : DbContext
    {
        private readonly T context;
        private Dictionary<Type, object> _repositories;

        /// <summary>
        /// Constructor For Db Context (movies)
        /// </summary>
        /// <param name="Dbcontext"></param>
        public UnitOfWork(T context)
        {
            this.context = context;
        }

        public List<TStored> GetData<TStored>(string sp_name, Dictionary<string, object> parames = null
            , CommandType CommandType = System.Data.CommandType.StoredProcedure)
        {

            using var cmd = context.Database.GetDbConnection().CreateCommand();

            cmd.CommandText = sp_name;
            cmd.CommandType = CommandType;
            //cmd.CommandTimeout = 180;

            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                cmd.Connection.Open();

            if (parames != null)
                foreach (var item in parames)
                {
                    var param1 = new SqlParameter
                    {
                        ParameterName = item.Key,
                        SqlValue = item.Value
                    };

                    cmd.Parameters.Add(param1);
                }

            try
            {

                var reader = cmd.ExecuteReader();

                if (reader == null)
                {
                    return null;
                }

                List<TStored> list = new List<TStored>();

                var columns = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(reader.GetName(i).ToLower());
                }

                while (reader.Read())
                {
                    TStored obj = Activator.CreateInstance<TStored>();

                    int i = 0;
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {

                        if (columns.Contains(prop.Name.ToLower()) &&
                            !object.Equals(reader[prop.Name], DBNull.Value))
                        {
                            try
                            {
                                prop.SetValue(obj, reader[prop.Name], null);

                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("Boolean"))
                                {
                                    prop.SetValue(obj,Convert.ToBoolean( reader[prop.Name]), null);

                                }
                                else
                                {
                                    prop.SetValue(obj, reader[prop.Name].ToString(), null);

                                }

                            }
                        }
                        i++;




                    }
                    list.Add(obj);
                }

                reader.Close();

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region oldWay
        //public List<TStored> GetData<TStored>(string sp_name, Dictionary<string, object> parames = null
        //, CommandType CommandType = System.Data.CommandType.StoredProcedure)
        //{
        //    using var cmd = context.Database.GetDbConnection().CreateCommand();

        //    cmd.CommandText = sp_name;
        //    cmd.CommandType = CommandType;
        //    //cmd.CommandTimeout = 180;

        //    if (cmd.Connection.State != System.Data.ConnectionState.Open)
        //        cmd.Connection.Open();

        //    if (parames != null)
        //        foreach (var item in parames)
        //        {
        //            var param1 = new SqlParameter
        //            {
        //                ParameterName = item.Key,
        //                SqlValue = item.Value
        //            };

        //            cmd.Parameters.Add(param1);
        //        }

        //    try
        //    {

        //        var reader = cmd.ExecuteReader();

        //        if (reader == null)
        //        {
        //            return null;
        //        }

        //        List<TStored> list = new List<TStored>();

        //        var columns = new List<string>();

        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            columns.Add(reader.GetName(i).ToLower());
        //        }

        //        while (reader.Read())
        //        {
        //            TStored obj = Activator.CreateInstance<TStored>();

        //            int i = 0;
        //            foreach (PropertyInfo prop in obj.GetType().GetProperties())
        //            {

        //                if (columns.Contains(prop.Name.ToLower()) &&
        //                    !object.Equals(reader[prop.Name], DBNull.Value))
        //                {
        //                    prop.SetValue(obj, reader[prop.Name], null);
        //                }
        //                i++;




        //            }
        //            list.Add(obj);
        //        }

        //        reader.Close();

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        #endregion

        public async Task<List<TStored>> GetDataStored<TStored>(string storedName, List<StoredPramaters> pramaters = null)
        {
            using var conn = context.Database.GetDbConnection();

            var command = conn.CreateCommand();
            command.CommandText = storedName;
            command.CommandType = CommandType.StoredProcedure;

            if (pramaters != null && pramaters.Count > 0)
            {
                pramaters.ForEach(item =>
                {
                    var forDateParam = command.CreateParameter();
                    forDateParam.ParameterName = item.ParameterName;
                    forDateParam.DbType = item.ParameterType;
                    forDateParam.Value = item.Value;
                    command.Parameters.Add(forDateParam);
                });
            }

            if (command.Connection.State != System.Data.ConnectionState.Open)
            {

                await conn.OpenAsync();
            }

            var reader = await command.ExecuteReaderAsync();

            var table = new DataTable();
            table.Load(reader);

            return JsonConvert.DeserializeObject<List<TStored>>(DataTableToJSONWithJSONNet(table));
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public async Task<List<TStored>> GetDataAsync<TStored>(string sp_name, Dictionary<string, object> parames = null
          , CommandType CommandType = System.Data.CommandType.StoredProcedure)
        {
            using var connection = context.Database.GetDbConnection();
            var cmd = connection.CreateCommand();
            cmd.CommandText = sp_name;
            cmd.CommandType = CommandType;
            //cmd.CommandTimeout = 180;

            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            if (parames != null)
                foreach (var item in parames)
                {
                    var param1 = new SqlParameter
                    {
                        ParameterName = item.Key,
                        SqlValue = item.Value
                    };

                    cmd.Parameters.Add(param1);
                }

            try
            {

                var reader = await cmd.ExecuteReaderAsync();

                if (reader == null)
                {
                    return null;
                }

                var table = new DataTable();
                table.Load(reader);

                return JsonConvert.DeserializeObject<List<TStored>>(DataTableToJSONWithJSONNet(table));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable CreateDataTable<E>(IEnumerable<E> list)
        {
            Type type = typeof(E);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (E entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        //public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        //{
        //    if (_repositories == null) _repositories = new Dictionary<Type, object>();

        //    var type = typeof(TEntity);
        //    if (!_repositories.ContainsKey(type))
        //        _repositories[type] = new Repository<TEntity>(context, false);

        //    return (IRepository<TEntity>)_repositories[type];
        //}

        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(context, false);
            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task<long> SaveAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (((SqlException)ex.InnerException).Number == 2627)//dublicate Key
                {
                    throw new Exception("dublicate Key");
                }
                else if (((SqlException)ex.InnerException).Number == 547)//// Foreign Key violation
                {
                    throw new Exception("Foreign Key violation");
                }

                else if (((SqlException)ex.InnerException).Number == 2601)//// Primary key violation
                {
                    throw new Exception("Primary key violation");
                }
            }
            throw new Exception("This Item Is Not Saved");
        }

        public async Task<long> SaveAsyncTransaction()
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var result = await context.SaveChangesAsync();

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public long SaveChanges()
        {

            try
            {
                //var list = 0;

                return context.SaveChanges();

            }
            catch (Exception ex)
            {
                if (((SqlException)ex.InnerException).Number == 2627)//dublicate Key
                {
                    throw new Exception("dublicate Key");
                }
                else if (((SqlException)ex.InnerException).Number == 547)//// Foreign Key violation
                {
                  
                    throw new Exception("Foreign Key violation");
                }

                else if (((SqlException)ex.InnerException).Number == 2601)//// Primary key violation
                {
                    throw new Exception("Primary key violation");
                }
            }
            throw new Exception("This Item Is Not Saved");
        }

        public long SaveChangesTransaction()
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var result = context.SaveChanges();

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        /**
         * remarks I need To Review This Method Again 
         */
        public DataTable Get(string sql, Dictionary<string, string> parameters)
        {

            SqlConnection conn = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
            }

            var dt = new DataTable();

            var da = new SqlDataAdapter(cmd);
            da.FillSchema(dt, SchemaType.Source);
            da.Fill(dt);


            return dt;
        }

        public List<TResult> Get<TResult>(string sql)
        {
            List<TResult> data = new List<TResult>();
            using (SqlConnection connection = new SqlConnection(
               context.Database.GetDbConnection().ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    sql, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //TResult item = GetItem<TResult>(reader);
                        //data.Add(item);
                    }
                }
            }

            return data;
        }


        private TResult GetItem<TResult>(DataRow dr)
        {
            Type temp = typeof(TResult);
            TResult obj = Activator.CreateInstance<TResult>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }


    }

}
