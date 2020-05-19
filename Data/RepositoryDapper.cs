namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
    using Dommel;
    using Infrastructure.Data;

    public class RepositoryDapper<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private string _connectionString;

        private SqlConnection _connection;

        public RepositoryDapper()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["BookStore"].ConnectionString;
        }

        protected SqlConnection ConnectionProvider()
        {
            if(_connection == null)
            {
                return new SqlConnection(_connectionString);
            }
            else
            {
                return _connection;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.GetAll<TEntity>();
            }
        }

        public TEntity Get(TKey id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Get<TEntity>(id);
            }
        }

        public void Delete(TKey id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var entity = db.Get<TEntity>(id);
                db.Delete<TEntity>(entity);
            }
        }

        public long Insert(TEntity entity)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var result = db.Insert<TEntity>(entity);
                return result != null ? long.Parse(result.ToString()) : default(long);
            }
        }

        public void Update(TEntity entity)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Update<TEntity>(entity);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
