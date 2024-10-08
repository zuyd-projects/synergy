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
        private readonly string connectionString = "Server=db.rickokkersen.nl;Database=synergy;User Name=synergy;Password=!1q@2w#3e";


        private SqlConnection connection;

        public SQLDAL()
        {
            this.connection = new SqlConnection(this.connectionString);
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
