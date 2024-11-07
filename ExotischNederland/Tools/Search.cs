using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ExotischNederland.Tools
{
    internal class Search<T>
    {
        private List<T> list;
        //private IOrderedEnumerable<T> sorted = null;

        public Search(List<T> _list)
        {
            this.list = _list;
        }

        public List<T> Query(string _property, string _value)
        {
            if (this.list == null || this.list.Count == 0) return new List<T>();

            return this.list.Where(x => GetPropertyValue(x, _property).ToString().ToLower().Contains(_value.ToLower())).ToList();
        }

        private static object GetPropertyValue(object _obj, string _propertyPath)
        {
            var property = GetNestedProperty(_obj.GetType(), _propertyPath);
            return property != null ? GetNestedObject(_obj, _propertyPath) : null;
        }

        private static PropertyInfo GetNestedProperty(Type _type, string _propertyPath)
        {
            string[] properties = _propertyPath.Split('.');
            PropertyInfo property = null;

            foreach (var prop in properties)
            {
                property = _type.GetProperty(prop);
                if (property == null) return null;
                _type = property.PropertyType;
            }

            return property;
        }

        private static object GetNestedObject(object _obj, string _propertyPath)
        {
            string[] properties = _propertyPath.Split('.');
            object obj = _obj;

            foreach (var prop in properties)
            {
                obj = obj.GetType().GetProperty(prop).GetValue(obj, null);
            }

            return obj;
        }
    }
}
