using Chirpel.Common.Models;
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

        public bool Delete(string table, string condition, string[] values)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"DELETE from [{table}] Where {condition}", conn);
                for (int i = 0; i < values.Length; i++)
                    query.Parameters.AddWithValue($"@Value{i+1}", values[i]);

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
                        if (p.PropertyType == typeof(string)) p.SetValue(o, ReadString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, ReadInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, ReadBool(Reader, p.Name));
                    }
                    list.Add((T)o);
                }
            }
            return list;
        }

        public List<T> Select<T>(string table, string condition, string[] values)
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand query = new SqlCommand($"Select * from {table} Where {condition}", conn);
                for (int i = 0; i < values.Length; i++)
                    query.Parameters.AddWithValue($"@Value{i+1}", values[i]);


                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach(PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, ReadString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, ReadInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, ReadBool(Reader, p.Name));
                    }
                    list.Add((T)o);   
                }
            }
            return list;
        }

        public List<T> Select<T>(string table, string condition, string selecter, string[] values)
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"Select {selecter} from [{table}] Where {condition}", conn);
                for (int i = 0; i < values.Length; i++)
                    query.Parameters.AddWithValue($"@Value{i+1}", values[i]);

                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach (PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, ReadString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, ReadInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, ReadBool(Reader, p.Name));
                        if (p.PropertyType == typeof(DateTime)) p.SetValue(o, ReadTime(Reader, p.Name));
                    }
                    list.Add((T)o);
                }
            }
            return list;
        }

        public T SelectFirst<T>(string table)
        {
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"Select TOP 1 * from [{table}] LIMIT 1 ", conn);
                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach (PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, ReadString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, ReadInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, ReadBool(Reader, p.Name));
                        if (p.PropertyType == typeof(Guid)) p.SetValue(o, ReadGuid(Reader, p.Name));
                        if (p.PropertyType == typeof(DateTime)) p.SetValue(o, ReadTime(Reader, p.Name));
                    }
                    return (T)o;
                }
            }
            return default(T);
        }

        public T SelectFirst<T>(string table, string condition, string[] values)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"Select TOP 1 * from [{table}] Where {condition} ", conn);
                for (int i = 0; i < values.Length; i++)
                    query.Parameters.AddWithValue($"@Value{i+1}", values[i]);

                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach (PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, ReadString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, ReadInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, ReadBool(Reader, p.Name));
                        if (p.PropertyType == typeof(DateTime)) p.SetValue(o, ReadTime(Reader, p.Name));
                        if (p.PropertyType == typeof(Guid)) p.SetValue(o, ReadGuid(Reader, p.Name));
                    }
                    return (T)o;
                }
            }
            return default(T);
        }

        public T SelectFirst<T>(string table, string condition, string selecter, string[] values)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand($"Select TOP 1 {selecter} from [{table}] Where {condition}", conn);
                for (int i = 0; i < values.Length; i++)
                    query.Parameters.AddWithValue($"@Value{i+1}", values[i]);

                conn.Open();

                var Reader = query.ExecuteReader();
                while (Reader.Read())
                {
                    object o = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propInfoList = typeof(T).GetProperties();
                    foreach (PropertyInfo p in propInfoList)
                    {
                        if (p.PropertyType == typeof(string)) p.SetValue(o, ReadString(Reader, p.Name));
                        if (p.PropertyType == typeof(Int32)) p.SetValue(o, ReadInt32(Reader, p.Name));
                        if (p.PropertyType == typeof(Boolean)) p.SetValue(o, ReadBool(Reader, p.Name));
                        if (p.PropertyType == typeof(DateTime)) p.SetValue(o, ReadTime(Reader, p.Name));
                        if (p.PropertyType == typeof(Guid)) p.SetValue(o, ReadGuid(Reader, p.Name));
                    }
                    return (T)o;
                }
            }
            return default(T);
        }

        public bool Insert<T>(T data, string table)
        {

            string QueryString = $"Insert into [{table}] (";
            PropertyInfo[] propInfoList = typeof(T).GetProperties();
            for (int i = 0; i < propInfoList.Length; i++)
            {
                if (i == propInfoList.Length - 1)
                    QueryString += $"{propInfoList[i].Name} ) VALUES (";
                else
                    QueryString += $"{propInfoList[i].Name}, ";
            }

            for (int i = 0; i < propInfoList.Length; i++)
            {
                if (i == propInfoList.Length - 1)
                    QueryString += $"@{propInfoList[i].Name} )";
                else
                    QueryString += $"@{propInfoList[i].Name}, ";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand query = new SqlCommand(QueryString, conn);
                foreach (PropertyInfo property in propInfoList)
                {
                    query.Parameters.AddWithValue($"@{property.Name}", property.GetValue(data));
                }

                conn.Open();

                int result = query.ExecuteNonQuery();

                if (result < 0)
                    return false;

                return true;
            }
        }

        private Int32? ReadInt32(SqlDataReader reader, string columnName)
        {
            Int32? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetInt32(reader.GetOrdinal(columnName));

            return result;
        }

        private string? ReadString(SqlDataReader reader, string columnName)
        {
            string? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetString(reader.GetOrdinal(columnName));

            return result;
        }
        
        private bool? ReadBool(SqlDataReader reader, string columnName)
        {
            bool? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetBoolean(reader.GetOrdinal(columnName));

            return result;
        }
        
        private DateTime? ReadTime(SqlDataReader reader, string columnName)
        {
            DateTime? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = reader.GetDateTime(reader.GetOrdinal(columnName));

            return result;
        }

        private Guid? ReadGuid(SqlDataReader reader, string columnName)
        {
            Guid? result = null;

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = Guid.Parse(reader.GetString(reader.GetOrdinal(columnName)));

            return result;
        }
    }
}
