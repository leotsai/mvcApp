using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using MvcApp.Core.Extensions;
using MvcApp.Core.Paging;

namespace MvcApp.Core.Helpers
{
    public class SqlHelper
    {
        private readonly string _connectionString;

        public SqlHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static SqlHelper Main { get; }

        static SqlHelper()
        {
            Main = new SqlHelper(AppContext.ConnectionString);
        }
        
        public void BulkCopy(DataTable table)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();
                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        sqlBulkCopy.BatchSize = table.Rows.Count;
                        sqlBulkCopy.DestinationTableName = table.TableName;
                        sqlBulkCopy.MapColumns(table);
                        sqlBulkCopy.WriteToServer(table);
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                    throw;
                }
            }
        }

        private List<TEntity> GetList<TEntity>(string sql, Func<DataRow, TEntity> getEntity, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        cmd.Parameters.Clear();

                        var list = new List<TEntity>();
                        foreach (DataRow row in table.Rows)
                        {
                            list.Add(getEntity(row));
                        }
                        return list;
                    }
                }
            }
        }

        private List<TEntity> GetList<TEntity>(string sql, Func<DataRow, TEntity> getEntity, params object[] parameterValues)
        {
            if (parameterValues == null)
            {
                return GetList(sql, getEntity, null);
            }
            var paras = new SqlParameter[parameterValues.Length];
            for (var i = 0; i < parameterValues.Length; i++)
            {
                paras[i] = new SqlParameter("p" + i, parameterValues[i]);
            }
            return GetList(sql, getEntity, paras);
        }

        public List<TEntity> List<TEntity>(string sql, string connectionString, params SqlParameter[] parameters) where TEntity : IQueryEntity, new()
        {
            return GetList(sql, row =>
            {
                var entity = new TEntity();
                entity.Fill(row);
                return entity;
            }, connectionString, parameters);
        }

        public List<TEntity> List<TEntity>(string sql, params object[] parameterValues) where TEntity : IQueryEntity, new()
        {
            return GetList(sql, row =>
            {
                var entity = new TEntity();
                entity.Fill(row);
                return entity;
            }, parameterValues);
        }

        public List<T> GetValues<T>(string sql, string connectionString, params SqlParameter[] parameters)
        {
            return GetList(sql, row =>
            {
                var value = row[0];
                return value is DBNull ? default(T) : (T)value;
            }, parameters);
        }

        public List<T> GetValues<T>(string sql, params object[] parameterValues)
        {
            return GetList(sql, row =>
            {
                var value = row[0];
                return value is DBNull ? default(T) : (T)value;
            }, parameterValues);
        }

        public int GetCount(string sql, params SqlParameter[] parameters)
        {
            return GetValue<int>(sql, parameters);
        }

        public int GetCount(string sql, params object[] parameterValues)
        {
            return GetValue<int>(sql, parameterValues);
        }

        public TEntity FirstOrDefault<TEntity>(string sql, params SqlParameter[] parameters) where TEntity : IQueryEntity, new()
        {
            return List<TEntity>(sql, parameters).FirstOrDefault();
        }

        public TEntity FirstOrDefault<TEntity>(string sql, params object[] parameterValues) where TEntity : IQueryEntity, new()
        {
            return List<TEntity>(sql, parameterValues).FirstOrDefault();
        }

        public T GetValue<T>(string sql, params SqlParameter[] parameters)
        {
            return GetValues<T>(sql, parameters).FirstOrDefault();
        }

        public T GetValue<T>(string sql, params object[] parameterValues)
        {
            return GetValues<T>(sql, parameterValues).FirstOrDefault();
        }

        public PageResult<T> GetPageResult<T>(string sql, string sqlCount, ISqlParameters criteria, PageRequest request)
            where T : IQueryEntity, new()
        {
            var parameters = criteria.GetParameters();
            return GetPageResult<T>(sql, () => sqlCount, request, parameters);
        }

        public PageResult<T> GetPageResult<T>(string sql, Func<string> sqlCount, ISqlParameters criteria, PageRequest request)
            where T : IQueryEntity, new()
        {
            var parameters = criteria.GetParameters();
            return GetPageResult<T>(sql, sqlCount, request, parameters);
        }

        public PageResult<T> GetPageResult<T>(string sql, Func<string> sqlCount, PageRequest request, params SqlParameter[] parameters)
            where T : IQueryEntity, new()
        {
            var list = List<T>(request.SqlPaging(sql), parameters);
            var total = request.TotalCount ?? GetCount(sqlCount(), parameters);
            return new PageResult<T>(list, request, total);
        }

        public PageResult<T> GetPageResult<T>(string sql, Func<string> sqlCount, PageRequest request, params object[] parameterValues)
            where T : IQueryEntity, new()
        {
            if (parameterValues == null)
            {
                return GetPageResult<T>(sql, sqlCount, request, null);
            }
            var paras = new SqlParameter[parameterValues.Length];
            for (var i = 0; i < parameterValues.Length; i++)
            {
                paras[i] = new SqlParameter("p" + i, parameterValues[i]);
            }
            return GetPageResult<T>(sql, sqlCount, request, paras);
        }

        public TransactionContext BeginTransaction()
        {
            return new TransactionContext(new SqlConnection(_connectionString));
        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();
                    var executed = 0;
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        executed = cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return executed;
                }
                catch
                {
                    transaction?.Rollback();
                    throw;
                }
            }
        }

        public int ExecuteNonQuery(string sql, params object[] parameterValues)
        {
            if (parameterValues == null)
            {
                return ExecuteNonQuery(sql, null);
            }
            var paras = new SqlParameter[parameterValues.Length];
            for (var i = 0; i < parameterValues.Length; i++)
            {
                paras[i] = new SqlParameter("p" + i, parameterValues[i]);
            }
            return ExecuteNonQuery(sql, paras);
        }

    }
}
