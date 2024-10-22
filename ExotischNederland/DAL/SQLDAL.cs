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
                            // Use the fillable array to only fill the properties that are allowed
                            var values = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                values.Add(reader.GetName(i), reader.GetValue(i));
                            return (T)Activator.CreateInstance(typeof(T), values);
                        }
                        return default(T);
                    }
                }
            }
        }

        public int Insert(string _table, Dictionary<string, object> _values)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                // Create a new query using the QueryBuilder and the values
                var query = new QueryBuilder(_table).Insert(_values).Build();
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the values as parameters to the command
                    foreach (var kvp in _values)
                    {
                        command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }
                    // Execute the command and return the Id of the new row
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT CAST(@@Identity as INT);";
                    return (int)command.ExecuteScalar();
                }

            }
        }
    }
}
