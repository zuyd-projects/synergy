using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class Model
    {
        /// <summary>
        /// Helper function to cast a value to a nullable object (if it is DBNull)
        /// </summary>
        /// <param name="_value">Value from the database</param>
        /// <returns>Nullable object that can be further casted</returns>
        public static object CastNullable(object _value)
        {
            if (_value is DBNull)
            {
                return null;
            }
            return _value;
        }
    }
}
