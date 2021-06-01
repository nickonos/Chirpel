using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
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
            _databaseQuery.Insert(entity);
        }

        public TEntity Get(string id)
        {
            PropertyInfo[] propInfo = typeof(TEntity).GetProperties();
            foreach(PropertyInfo prop in propInfo)
            {
                if (prop.Name == "Id")
                    return _databaseQuery.SelectFirst<TEntity>( "id=@value1", new string[] { id });
            }
            return default(TEntity);
        }

        public List<TEntity> GetAll()
        {
            return _databaseQuery.Select<TEntity>();
        }

        public void Remove(TEntity entity)
        {
            string query = "";
            
            PropertyInfo[] propInfo = typeof(TEntity).GetProperties();
            string[] tempValues = new string[propInfo.Length];
            int i = 0;
            foreach (PropertyInfo prop in propInfo)
            {
                if(prop.GetValue(entity) != null)
                {
                    if(i != 0)
                        query += " and ";

                    query += $"{prop.Name}=@Value{i + 1}";
                    tempValues[i] = prop.GetValue(entity).ToString();
                    i++;
                }

            }
            string[] values = new string[i];
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = tempValues[j];
            }
            _databaseQuery.Delete(typeof(TEntity).Name, query, values);
        }

        public void Update(TEntity entity)
        {
            PropertyInfo[] propInfo = typeof(TEntity).GetProperties();
            bool exec = false;
            string[] tempValues = new string[propInfo.Length];
            int i = 0;
            string query = "";
            string idval = "";
            foreach (PropertyInfo prop in propInfo)
            {
                if (prop.Name == "Id")
                {
                    idval = prop.GetValue(entity).ToString();
                    exec = true;
                }
                    
                if(prop.GetValue(entity) != null)
                {
                    if (i != 0)
                        query += ", ";

                    query += $"{prop.Name}=@Value{i + 1}";
                    tempValues[i] = prop.GetValue(entity).ToString();
                    i++;
                }
                
            }
            tempValues[i] = idval;
            string[] values = new string[i+1];
            for(int j =0; j < values.Length; j++)
            {
                values[j] = tempValues[j];
            }
            if(exec)
                _databaseQuery.Update(typeof(TEntity).Name, query,$"id=@value{i+1}", values);
        }

        public DAL(DatabaseQuery databaseQuery)
        {
            _databaseQuery = databaseQuery;
        }
    }
}
