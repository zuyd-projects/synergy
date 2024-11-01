﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland
{
    internal class Form
    {
        private List<FormField> fields;
        private Dictionary<string, object> values;
        private bool cancelled = false;

        public Form(List<FormField> _fields)
        {
            this.fields = _fields;
        }

        public Dictionary<string, object> Prompt()
        {
            Console.Clear();
            Console.WriteLine("Press ESC to cancel");

            values = new Dictionary<string, object>();

            foreach (FormField field in this.fields)
            {
                InputField(field);
                
                if (this.cancelled) return null;
            }
            return values;
        }

        private void InputField(FormField field)
        {
            Console.WriteLine(field.Text);
            if (field.Value != null) Console.Write($" [{field.Value}]");
            Console.WriteLine();
            string input = Helpers.ReadInputWithEsc();
            if (input == null)
            {
                this.cancelled = true;
                return;
            }
            if (field.Required && input == "")
            {
                if (field.Value != null) values.Add(field.Name, field.Value);
                else
                {
                    Console.WriteLine("  >This field is required!");
                    InputField(field);
                }
            }
            switch (field.Type)
            {
                case "string":
                    values.Add(field.Name, input);
                    break;
                case "number":
                    ParseNumber(field, input);
                    break;
                case "float":
                    ParseFloat(field, input);
                    break;
                case "bool":
                    ParseBool(field, input);
                    break;
                case "date":
                    ParseDate(field, input);
                    break;
                case "polygonString":
                    ParsePolygonString(field, input);
                    break;
            }
        }

        private void InputString(FormField field)
        {
            
        }

        private void ParseNumber(FormField _field, string _value)
        {
            if (!int.TryParse(_value, out int result))
            {
                Console.WriteLine("  >This field must be a number!");
                InputField(_field);
            }
            else
            {
                values.Add(_field.Name, result);
            }
        }

        private void ParseFloat(FormField _field, string _value)
        {
            if (!float.TryParse(_value, out float result))
            {
                Console.WriteLine("  >This field must be a float!");
                InputField(_field);
            }
            else
            {
                values.Add(_field.Name, result);
            }
        }

        private void ParseBool(FormField _field, string _value)
        {
            if (_value.ToLower() != "true" && _value.ToLower() != "false")
            {
                Console.WriteLine("  >This field must be a boolean!");
                InputField(_field);
            }
            else
            {
                values.Add(_field.Name, _value.ToLower() == "true");
            }
        }

        private void ParseDate(FormField _field, string _value)
        {
            if (!DateTime.TryParse(_value, out DateTime result))
            {
                Console.WriteLine("  >This field must be a date!");
                InputField(_field);
            }
            else
            {
                values.Add(_field.Name, result);
            }
        }

        private void ParsePolygonString(FormField _field, string _value)
        {
            if (Models.Area.ParsePolygonPoints(_value).Count == 0) // Check if the input is a polygon string
            {
                Console.WriteLine("  >This field must be a polygon string!");
                InputField(_field);
            }
            else
            {
                values.Add(_field.Name, _value);
            }
        }
    }

    internal class FormField
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public string Value { get; set; }

        public FormField(string _name, string _text, string _type, bool _required, string _value = null)
        {
            this.Name = _name;
            this.Text = _text;
            this.Type = _type;
            this.Required = _required;
            this.Value = _value;
        }
    }
}