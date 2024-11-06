using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        // Define the columns to use in the query as a List
        private List<string> columns = new List<string>();

        public QueryBuilder(string table)
        {
            this.table = table;
        }

        // Method for building the base Insert query
        public QueryBuilder Insert(Dictionary<string, object> _values)
        {
            // Get the columns and parameters from the values and join them into a comma separated string
            var columns = string.Join(", ", _values.Keys.Select(k => $"[{k}]")); // Wrap each key in square brackets
            // Set the base SQL query to the INSERT statement and return the QueryBuilder instance
            var parameters = string.Join(", ", _values.Keys.Select(k => "@" + k));
            this.baseSQL = $"INSERT INTO [{this.table}] ({columns}) VALUES ({parameters})";
            return this;
        }

        // Method for building the base Select query, optionally allowing specific columns to be selected
        public QueryBuilder Select()
        {
            // Use a placeholder for the columns to select to be replaced in the Build method
            this.baseSQL = $"SELECT $columns$ FROM [{this.table}]";
            return this;
        }

        // Method for specifying the columns to select in the query
        // if no columns are specified, all columns will be selected in the Build method
        public QueryBuilder Columns(params string[] _columns)
        {
            // Add the columns to the list of columns and return the QueryBuilder instance
            this.columns.AddRange(_columns);
            return this;
        }

        // Method for building the base Update query
        public QueryBuilder Update(Dictionary<string, object> _values)
        {
            // Wrap each key in square brackets to avoid issues with reserved keywords
            var setValues = string.Join(", ", _values.Select(kvp => $"[{kvp.Key}] = @{kvp.Key}"));
    
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
            if (_value is DateTime) _value = ((DateTime)_value).ToString("yyyy-MM-dd HH:mm:ss");
            string parameterValue = _value is string ? $"'{_value}'" : _value.ToString();
            // If the value contains a . it contains the table name, so we need to split it (e.g. "User.Id" -> [User].[Id])
            this.whereClauses.Add($"[{string.Join("].[", _column.Split('.'))}] {_operator} {parameterValue}");
            return this;
        }

        // ORDER BY clause support
        public QueryBuilder OrderBy(string _column, bool _ascending = true)
        {
            // If the value contains a . it contains the table name, so we need to split it (e.g. "User.Id" -> [User].[Id])
            this.orderByClauses.Add($"{string.Join("].[", _column.Split('.'))} {(_ascending ? "ASC" : "DESC")}");
            return this;
        }

        // JOIN clause support
        public QueryBuilder Join(string _table, string _column1, string _column2)
        {
            // If the value contains a . it contains the table name, so we need to split it (e.g. "User.Id" -> [User].[Id])
            this.baseSQL += $" JOIN [{_table}] ON [{string.Join("].[", _column1.Split('.'))}] = [{string.Join("].[", _column2.Split('.'))}]";
            return this;
        }

        // Build the final query string with WHERE and ORDER BY clauses
        public string Build()
        {
            // Start with the base SQL query
            var query = new StringBuilder(this.baseSQL);

            // If columns are specified, replace $columns$ with the columns list, otherwise use *
            string columnList = this.columns.Count > 0
            ? string.Join(", ", columns.Select(c => $"[{string.Join("].[", c.Split('.'))}]"))
            : "*";
            query.Replace("$columns$", columnList);

            if (whereClauses.Any())
            {
                query.Append(" WHERE " + string.Join(" AND ", whereClauses));
            }

            if (orderByClauses.Any())
            {
                query.Append(" ORDER BY " + string.Join(", ", orderByClauses));
            }

            // Replace [*] with * in the query string
            // this is done to allow for example User.* which would be turned into [User].[*] by the other methods
            query.Replace("[*]", "*");

            // Return the complete query string
            return query.ToString();
        }
    }
}