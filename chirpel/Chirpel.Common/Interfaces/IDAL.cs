using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Interfaces
{
    public interface IDAL<TEntity> where TEntity : class
    {
        TEntity Get(string id);
        IEnumerable<TEntity> GetAll();

        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}
