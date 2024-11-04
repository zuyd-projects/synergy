using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland
{
    internal class Form
    {
        private List<FormField> fields;
        private Dictionary<string, object> values;
        private bool cancelled = false;

        /// <summary>
        /// Create a new form
        /// </summary>
        /// <param name="_fields"></param>
        public Form(List<FormField> _fields)
        {
            this.fields = _fields;
        }

        /// <summary>
        /// Prompt the user to fill in the form, returns null on ESC
        /// </summary>
        /// <returns>Dictionary with key/value or null if cancelled</returns>
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

        /// <summary>
        /// Ask the user for input and allow to be cancelled with ESC
        /// </summary>
        /// <param name="field"></param>
        private void InputField(FormField field)
        {
            Console.Write(field.Text);
            if (field.Value != null && field.Type != "password") Console.Write($" [{field.Value}]");
            Console.WriteLine();
            string input;
            if (field.Type == "password") 
                // Add the hidden boolean
                input = Helpers.ReadInputWithEsc(true);
            else
                input = Helpers.ReadInputWithEsc();
            // If ESC is pressed, set cancelled to true to return null from the main loop
            if (input == null)
            {
                this.cancelled = true;
                return;
            }
            // NOTE: this logic means that if a field is not required and there is an existing
            // value, there is no way to clear this value. A possible improvement could be made
            if (input == "")
            {
                // If the field already has a value (from an update form), use the existing value
                if (field.Value != null) input = field.Value;
                // Otherwise, if the field is required, print an error and ask for input again
                else if (field.Required)
                {
                    Console.WriteLine("  >This field is required!");
                    InputField(field);
                }
            }
            // If the value was set and should be treated as a password, hash it
            else if (field.Type == "password")
            {
                input = Helpers.HashPassword(input);
            }
            // Parse the input based on the field type
            switch (field.Type)
            {
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
                default:
                    values.Add(field.Name, input);
                    break;
            }
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
