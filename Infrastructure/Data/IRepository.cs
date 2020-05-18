using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Infrastructure.Data
{
    public interface IRepository<TEntity, Key> : IDisposable
        where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity Get(Key id);

        long Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(Key id);
    }
}
