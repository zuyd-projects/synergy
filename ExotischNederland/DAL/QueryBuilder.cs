using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.DAL
{
    internal class QueryBuilder
    {
        // Define the table to use in the query
        private string table;
        // Define the base SQL query (SELECT, INSERT, UPDATE, DELETE)
        private string baseSQL;
        // Define the WHERE clauses to use in the query as a List
        private List<string> whereClauses = new List<string>();
        // Define the ORDER BY clauses to use in the query as a List
        private List<string> orderByClauses = new List<string>();

        public QueryBuilder(string table)
        {
            this.table = table;
        }

        // Method for building the base Insert query
        public QueryBuilder Insert(Dictionary<string, object> _values)
        {
            // Get the columns and parameters from the values and join them into a comma separated string
            var columns = string.Join(", ", _values.Keys);
            // Set the base SQL query to the INSERT statement and return the QueryBuilder instance
            var parameters = string.Join(", ", _values.Keys.Select(k => "@" + k));
            this.baseSQL = $"INSERT INTO [{this.table}] ({columns}) VALUES ({parameters})";
            return this;
        }

        // Method for building the base Select query, optionally allowing specific columns to be selected
        public QueryBuilder Select(params string[] columns)
        {
            // If no columns are provided, default to selecting all
            string columnList = columns.Length > 0
            ? string.Join(", ", columns.Select(c => $"[{c}]"))
            : "*";
            this.baseSQL = $"SELECT {columnList} FROM [{this.table}]";
            return this;
        }

        // Method for building the base Update query
        public QueryBuilder Update(Dictionary<string, object> _values)
        {
            // Get the set values from the values and join them into a comma separated string
            var setValues = string.Join(", ", _values.Select(kvp => $"{kvp.Key} = @{kvp.Key}"));
            // Set the base SQL query to the UPDATE statement and return the QueryBuilder instance
            this.baseSQL = $"UPDATE [{this.table}] SET {setValues}";
            return this;
        }

        // Method for building the base Delete query
        public QueryBuilder Delete()
        {
            // Set the base SQL query to the DELETE statement and return the QueryBuilder instance
            this.baseSQL = $"DELETE FROM [{this.table}]";
            return this;
        }

        // Add WHERE clause using parameters to avoid SQL injection
        public QueryBuilder Where(string _column, string _operator, object _value)
        {
            string parameterValue = _value is string ? $"'{_value}'" : _value.ToString();
            this.whereClauses.Add($"[{_column}] {_operator} {parameterValue}");
            return this;
        }

        // ORDER BY clause support
        public QueryBuilder OrderBy(string _column, bool _ascending = true)
        {
            this.orderByClauses.Add($"{_column} {(_ascending ? "ASC" : "DESC")}");
            return this;
        }

        // Build the final query string with WHERE and ORDER BY clauses
        public string Build()
        {
            var query = new StringBuilder(this.baseSQL);

            if (whereClauses.Any())
            {
                query.Append(" WHERE " + string.Join(" AND ", whereClauses));
            }

            if (orderByClauses.Any())
            {
                query.Append(" ORDER BY " + string.Join(", ", orderByClauses));
            }

            return query.ToString();
        }
    }
}