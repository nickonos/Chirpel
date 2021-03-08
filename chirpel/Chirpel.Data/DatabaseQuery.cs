using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Chirpel.Data
{
    public class DatabaseQuery
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Chirpel;Integrated Security=True"; //Moeder

        //private readonly string connectionString = "Server=localhost;Database=Chirpel;Trusted_Connection=True;"; // Vader

        public bool Delete(string table)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"DELETE from [{table}]", conn);
                conn.Open();
                int res = query.ExecuteNonQuery();

                if (res < 0)
                    return false;
            }
            return true;
        }

        public bool Delete(string table, string condition)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"DELETE from [{table}] Where {condition}", conn);
                conn.Open();
                int res = query.ExecuteNonQuery();

                if (res < 0)
                    return false;
            }
            return true;
        }

        public List<T> Select<T>(string table)
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"Select * from [{table}]", conn);
                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach (PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, readString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, readInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, readBool(Reader, p.Name));
                    }
                    list.Add((T)o);
                }
            }
            return list;
        }

        public List<T> Select<T>(string table, string condition)
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"Select * from [{table}] Where {condition}", conn);
                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach(PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, readString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, readInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, readBool(Reader, p.Name));
                    }
                    list.Add((T)o);   
                }
            }
            return list;
        }

        public List<T> Select<T>(string table, string condition, string selecter)
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"Select {selecter} from [{table}] Where {condition}", conn);
                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach (PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, readString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, readInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, readBool(Reader, p.Name));
                        if (p.PropertyType == typeof(DateTime)) p.SetValue(o, readTime(Reader, p.Name));
                    }
                    list.Add((T)o);
                }
            }
            return list;
        }

        private Int32? readInt32(SqlDataReader reader, string columnName)
        {
            Int32? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetInt32(reader.GetOrdinal(columnName));


            return result;
        }

        private string? readString(SqlDataReader reader, string columnName)
        {
            string? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetString(reader.GetOrdinal(columnName));


            return result;
        }
        
        private bool? readBool(SqlDataReader reader, string columnName)
        {
            bool? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetBoolean(reader.GetOrdinal(columnName));

            return result;
        }
        
        private DateTime? readTime(SqlDataReader reader, string columnName)
        {
            DateTime? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetDateTime(reader.GetOrdinal(columnName));

            return result;
        }

    }
}
