using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.DAL
{
    internal class SQLDAL
    {
        private readonly string connectionString = "Server=db.rickokkersen.nl;Database=synergy;User ID=synergy;Password=!1q@2w#3e";

        private SqlConnection connection;

        public SQLDAL()
        {
            this.connection = new SqlConnection(this.connectionString);
        }

        // Method to find a specific row in a table
        public T Find<T>(string _field, string _value)
        {
            string table = typeof(T).Name;
            var query = new QueryBuilder(table).Select().Where(_field, "=", _value).Build();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var values = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                values.Add(reader.GetName(i), reader.GetValue(i));

                            reader.Close();
                            connection.Close();
                            return (T)Activator.CreateInstance(typeof(T), values);
                        }
                        return default(T);
                    }
                }
            }
        }

        // Method to insert a new row into the table
        public int Insert(string _table, Dictionary<string, object> _values)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                var query = new QueryBuilder(_table).Insert(_values).Build();
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    foreach (var kvp in _values)
                    {
                        command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT CAST(@@Identity as INT);";
                    try
                    {
                        object result = (int)command.ExecuteScalar();
                        return (int)result;
                    }
                    catch (Exception e)
                    {
                        return 0;
                    }
                }
            }
        }

        // Method to update a row in the table
        public void Update(string _table, int _id, Dictionary<string, object> _values)
        {
            var query = new QueryBuilder(_table).Update(_values).Where("Id", "=", _id.ToString()).Build();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    foreach (var kvp in _values)
                    {
                        command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to delete a row in the table by Id
        public void Delete(string _table, int _id)
        {
            var query = new QueryBuilder(_table).Delete().Where("Id", "=", _id.ToString()).Build();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to delete a row in the table by WHERE clause
        public void Delete(string _table, Func<QueryBuilder, QueryBuilder> _queryBuilder = null)
        {
            if (_queryBuilder == null) throw new ArgumentNullException("QueryBuilder cannot be null when deleting rows by WHERE clause.");

            var query = _queryBuilder(new QueryBuilder(_table).Delete()).Build();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to select all rows from a table
        public List<T> Select<T>(Func<QueryBuilder, QueryBuilder> _queryBuilder = null)
        {
            string table = typeof(T).Name;
            var queryBuilder = new QueryBuilder(table).Select();
            var query = _queryBuilder != null ? _queryBuilder(queryBuilder).Build() : queryBuilder.Build();
            List<T> results = new List<T>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var values = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                values.Add(reader.GetName(i), reader.GetValue(i));
                            results.Add((T)Activator.CreateInstance(typeof(T), values));
                        }
                    }
                }
            }

            return results;
        }
    }
}