using Chirpel.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class DAL<TEntity> : IDAL<TEntity> where TEntity : class
    {
        protected readonly DatabaseQuery _databaseQuery;
        public void Add(TEntity entity)
        {
            _databaseQuery.Insert(entity, typeof(TEntity).Name);
        }

        public TEntity Get(string id)
        {
            PropertyInfo[] propInfo = typeof(TEntity).GetProperties();
            foreach(PropertyInfo prop in propInfo)
            {
                if (prop.Name == "Id")
                    return _databaseQuery.SelectFirst<TEntity>(typeof(TEntity).Name, "id=@value1", new string[] { id });
            }
            return null;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _databaseQuery.Select<TEntity>(typeof(TEntity).Name);
        }

        public void Remove(TEntity entity)
        {
            string query = "";
            
            PropertyInfo[] propInfo = typeof(TEntity).GetProperties();
            string[] values = new string[propInfo.Length];
            int i = 0;
            foreach (PropertyInfo prop in propInfo)
            {
                if(i != 0)
                {
                    query += " and ";
                }
                query += $"{prop.Name}=@Value{i}";
                values[i] = prop.GetValue(entity).ToString();
                i++;
            }
            _databaseQuery.Delete(typeof(TEntity).Name, query, values);
        }

        public void Update(TEntity entity)//TODO Implement method
        {
            throw new NotImplementedException();
        }

        public DAL(DatabaseQuery databaseQuery)
        {
            _databaseQuery = databaseQuery;
        }
    }
}
