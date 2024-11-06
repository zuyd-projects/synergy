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
                object value = field.Input();

                if (value is null) return null;
                values.Add(field.Name, value);
            }
            return values;
        }
    }

    internal class FormField
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public string Value { get; set; }
        public Dictionary<string, string> Options { get; set; }

        public FormField(string _name, string _text, string _type, bool _required, string _value = null, Dictionary<string, string> _options = null)
        {
            this.Name = _name;
            this.Text = _text;
            this.Type = _type;
            this.Required = _required;
            this.Value = _value;
            this.Options = _options;
        }

        public object Input()
        {
            string input = this.Prompt();

            // Parse the input based on the field type
            switch (this.Type)
            {
                case "number":
                    return ParseNumber(input);
                case "float":
                    return ParseFloat(input);
                case "boolean":
                    return ParseBool(input);
                case "date":
                    return ParseDate(input);
                case "polygonString":
                    return ParsePolygonString(input);
                default:
                    return input;
            }
        }

        private string Prompt(string _error = null)
        {
            Console.Clear();
            List<string> promptText = new List<string> { this.Text };
            if (this.Required) promptText[0] += " *";
            if (this.Type == "boolean") promptText[0] += " (waar/onwaar)";
            if (this.Type == "date")
            {
                promptText[0] += " (dd-mm-jjjj HH:MM)";
                promptText.Add("  >Voorbeeld: 01-01-2021 12:00");
                promptText.Add("  >Tijd is optioneel");
                promptText.Add($"  >Alleen een tijd invullen zal automatisch de datum van vandaag kiezen ({DateTime.Now.Date.ToString("d")})");
            }
            if (this.Value != null && this.Type != "password") promptText[0] += $" ({this.Value})";
            Console.WriteLine(promptText[0]);
            if (_error != null)
            {
                promptText.Add($" >{_error}");
                Console.WriteLine(promptText[1]);
            }
            string input;
            if (this.Type == "single_select")
                input = Helpers.MenuSelect(this.Options, false, promptText);
            else if (this.Type == "multi_select")
                input = string.Join(",", Helpers.MultiSelect(this.Options, false, null, promptText));
            else if (this.Type == "password")
                // Add the hidden boolean
                input = Helpers.ReadInputWithEsc(true);
            else
                input = Helpers.ReadInputWithEsc();
            // If ESC is pressed, set cancelled to true to return null from the main loop
            if (input == null)
            {
                return null;
            }
            // NOTE: this logic means that if a field is not required and there is an existing
            // value, there is no way to clear this value. A possible improvement could be made
            if (input == "")
            {
                // If the field already has a value (from an update form), use the existing value
                if (this.Value != null) input = this.Value;
                // Otherwise, if the field is required, print an error and ask for input again
                else if (this.Required)
                {
                    Console.WriteLine("  >This field is required!");
                    Input();
                }
            }
            // If the value was set and should be treated as a password, hash it
            else if (this.Type == "password")
            {
                input = Helpers.HashPassword(input);
            }
            return input;
        }

        private int ParseNumber(string _value)
        {
            if (!int.TryParse(_value, out int result))
            {
                return ParseNumber(Prompt("Ongeldig getal!"));
            }
            return result;
        }

        private float ParseFloat(string _value)
        {
            if (!float.TryParse(_value, out float result))
            {
                return ParseFloat(Prompt("Ongeldig getal!"));
            }
            return result;
        }

        private bool ParseBool(string _value)
        {
            if (_value.ToLower() != "waar" && _value.ToLower() != "onwaar")
            {
                return ParseBool(Prompt("Vul waar of onwaar in!"));
            }
            return _value.ToLower() == "waar";
        }

        private DateTime ParseDate(string _value)
        {
            if (!DateTime.TryParse(_value, out DateTime result))
            {
                return ParseDate(Prompt("Vul een geldige datum in!"));
            }
            return result;
        }

        private string ParsePolygonString(string _value)
        {
            if (Models.Area.ParsePolygonPoints(_value).Count == 0) // Check if the input is a polygon string
            {
                return ParsePolygonString(Prompt("Ongeldige invoer!"));
            }
            return _value;
        }
    }
}