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
        private readonly string connectionString;

        private SqlConnection connection;

        private static SQLDAL instance;

        private static readonly object lockObject = new object();

        private SQLDAL()
        {
            Dictionary<string, string> settings = Helpers.LoadSettings();
            this.connectionString = $"Server={settings["DB_HOST"]},{settings["DB_PORT"]};Database={settings["DB_DATABASE"]};User ID={settings["DB_USERNAME"]};Password={settings["DB_PASSWORD"]}";
            this.connection = new SqlConnection(this.connectionString);
        }

        //
        public static SQLDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        instance = new SQLDAL();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Method to find a specific row in a table by a field and value
        /// </summary>
        /// <typeparam name="T">Class to return (also used as the tablename)</typeparam>
        /// <param name="_field">Column name</param>
        /// <param name="_value">Value (should be unique)</param>
        /// <returns>Object of specified type containing the values from the database</returns>
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
                            // Create a new instance of the specified class and return it
                            return (T)Activator.CreateInstance(typeof(T), values);
                        }
                        return default(T);
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert a new row into the table
        /// </summary>
        /// <param name="_table"></param>
        /// <param name="_values">Dictionary with key (column name) and value</param>
        /// <returns>The value of the identity column of the newly created row. 0 if no identity column was found</returns>
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

        /// <summary>
        /// Method to update a row in the table
        /// </summary>
        /// <param name="_table"></param>
        /// <param name="_id"></param>
        /// <param name="_values"></param>
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

        /// <summary>
        /// Method to delete a row in the table by ID
        /// </summary>
        /// <param name="_table"></param>
        /// <param name="_id"></param>
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

        /// <summary>
        /// Method to delete rows in the table by WHERE clause
        /// </summary>
        /// <param name="_table"></param>
        /// <param name="_queryBuilder"></param>
        /// <exception cref="ArgumentNullException">If no QueryBuilder is supplied an error is thrown. Without QueryBuilder all rows in the database would be deleted</exception>
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

        /// <summary>
        /// Select rows from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_queryBuilder"></param>
        /// <returns>List with objects of the specified type</returns>
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