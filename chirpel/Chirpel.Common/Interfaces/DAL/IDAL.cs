using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Interfaces.DAL
{
    public interface IDAL<TEntity> where TEntity : class
    {
        TEntity Get(string id);
        List<TEntity> GetAll();
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}
