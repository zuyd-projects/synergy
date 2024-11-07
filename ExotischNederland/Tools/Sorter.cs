using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Tools
{
    internal class Sorter<T>
    {
        private List<T> list;
        private IOrderedEnumerable<T> sorted = null;
        private bool first = true;

        public Sorter(List<T> _list)
        {
            this.list = _list;
        }

        public Sorter<T> Sort(string _property, bool _ascending = true)
        {
            if (this.first)
            {
                this.sorted = _ascending
                    ? this.list.OrderBy(x => GetPropertyValue(x, _property))
                    : this.list.OrderByDescending(x => GetPropertyValue(x, _property));
                this.first = false;
            }
            else
            {
                 this.sorted = _ascending
                    ? this.sorted.ThenBy(x => GetPropertyValue(x, _property))
                    : this.sorted.ThenByDescending(x => GetPropertyValue(x, _property));
            }
            
            return this;
        }

        public List<T> ToList()
        {
            if (this.list == null || this.list.Count == 0) return new List<T>();

            return this.sorted?.ToList() ?? this.list;
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
            foreach (var prop in properties)
            {
                if (_obj == null) return null;
                var property = _obj.GetType().GetProperty(prop);
                if (property == null) return null;
                _obj = property.GetValue(_obj);
            }

            return _obj;
        }
    }
}
