using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ExotischNederland
{
    internal class Helpers
    {
        /// <summary>
        /// Create a menu with a list of items to select from
        /// </summary>
        /// <param name="_items"></param>
        /// <param name="_indexes"></param>
        /// <param name="_text"></param>
        /// <returns>The key of the selected item</returns>
        // TODO: Rename this method to SelectMenu
        public static string MenuSelect(Dictionary<string, string> _items, bool _indexes = false, List<string> _text = null)
        {
            if (_text is null) _text = new List<string>();
            int selected = 0;
            while (true)
            {
                Console.Clear();
                foreach (var item in _text)
                {
                    if (_text != null) Console.WriteLine(item);
                }
                foreach (var item in _items)
                {
                    if (_items.Keys.ToList().IndexOf(item.Key) == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (_indexes) Console.Write(_items.Keys.ToList().IndexOf(item.Key) + 1 + ". ");
                    Console.WriteLine(item.Value);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                ConsoleKey input = Console.ReadKey().Key;
                if (input == ConsoleKey.Escape) return null;
                if (input == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected < 0)
                    {
                        selected = _items.Count - 1;
                    }
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected > _items.Count - 1)
                    {
                        selected = 0;
                    }
                }
                else if (input == ConsoleKey.Enter)
                {
                    return _items.Keys.ToList()[selected];
                }
            }
        }

        /// <summary>
        /// Create a menu with a list of items to select from
        /// </summary>
        /// <param name="_items"></param>
        /// <param name="_indexes"></param>
        /// <param name="_selection"></param>
        /// <param name="_text"></param>
        /// <returns>List of strings of selected items</returns>
        // TODO: Rename this method to MultiSelectMenu
        public static List<string> MultiSelect(Dictionary<string, string> _items, bool _indexes = false, List<string> _selection = null, List<string> _text = null)
        {
            int selected = 0;
            List<string> selection = _selection ?? new List<string>();
            List<string> text = _text ?? new List<string>();
            text.Add("Druk op spatie om een item te selecteren of deselecteren");
            text.Add("Druk op enter om de selectie op te slaan");
            while (true)
            {
                Console.Clear();
                foreach (var item in text)
                {
                    if (text != null) Console.WriteLine(item);
                }
                foreach (var item in _items)
                {
                    if (_items.Keys.ToList().IndexOf(item.Key) == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (_indexes) Console.Write(_items.Keys.ToList().IndexOf(item.Key) + 1 + ". ");
                    if (selection.Contains(item.Key))
                    {
                        Console.Write("[X] ");
                    }
                    else
                    {
                        Console.Write("[ ] ");
                    }
                    Console.WriteLine(item.Value);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                ConsoleKey input = Console.ReadKey().Key;
                if (input == ConsoleKey.Escape) return null;
                if (input == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected < 0)
                    {
                        selected = _items.Count - 1;
                    }
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected > _items.Count - 1)
                    {
                        selected = 0;
                    }
                }
                else if (input == ConsoleKey.Spacebar)
                {
                    if (selection.Contains(_items.Keys.ToList()[selected]))
                    {
                        selection.Remove(_items.Keys.ToList()[selected]);
                    }
                    else
                    {
                        selection.Add(_items.Keys.ToList()[selected]);
                    }
                }
                else if (input == ConsoleKey.Enter)
                {
                    return selection;
                }
            }
        }

        /// <summary>
        /// Method to hash a password using SHA256
        /// </summary>
        /// <param name="_password">Input password</param>
        /// <returns>SHA256 hash of the input password</returns>
        public static string HashPassword(string _password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(_password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Load settings from a .env file
        /// </summary>
        /// <param name="filePath"></param>
        public static Dictionary<string, string> LoadSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            var file = Path.Combine(Directory.GetCurrentDirectory(), "../../../ExotischNederland/.env");
            if (!File.Exists(file))
                return settings;

            foreach (var line in File.ReadAllLines(file))
            {
                var parts = line.Split(
                    '=',
                    (char)StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                settings.Add(parts[0], parts[1]);
            }
            return settings;
        }

        /// <summary>
        /// Get input for the user but cancel on ESC
        /// </summary>
        /// <returns>null for ESC or otherwise a string with the user input</returns>
        public static string ReadInputWithEsc(bool _hidden = false)
        {
            StringBuilder input = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (keyInfo.Key == ConsoleKey.Escape) return null; // Return null if Esc is pressed
                else if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b"); // Handle backspace
                }
                else if (keyInfo.Key != ConsoleKey.Backspace)
                {
                    input.Append(keyInfo.KeyChar);
                    if (_hidden)
                    {
                        Console.Write("*"); // Mask the input character
                    }
                    else
                    {
                        Console.Write(keyInfo.KeyChar);
                    }
                }
            }

            Console.WriteLine();
            return input.ToString();
        }

        /// <summary>
        /// Wait for J or N key
        /// </summary>
        /// <returns>true or false respectively</returns>
        public static bool ConfirmPrompt()
        {
            ConsoleKey key = ConsoleKey.NoName;
            while (key != ConsoleKey.N && key != ConsoleKey.J)
            {
                // true hides the pressed key from the output
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.N) return false;
                if (key == ConsoleKey.J) return true;
            }
            return false; // Default return value to satisfy all code paths
        }

        /// <summary>
        /// Get a path to the AppData folder and block path traversal
        /// Creates the folder if it doesn't exist
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_extension"></param>
        /// <param name="_subFolder"></param>
        /// <returns>The full path to the file</returns>
        public static string GetSafeFilePath(string _name, string _extension, string _subFolder = null)
        {
            string fileName = _name;
            // Replace invalid characters with an underscore
            foreach (char c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c, '_');
            // Block path traversal
            fileName = fileName.Replace("..", "_");
            // Remove / or \ from the filename
            fileName = fileName.Replace("/", "_").Replace("\\", "_");
            // Add the extension if it's not already there
            if (!_extension.StartsWith("."))
            {
                _extension = "." + _extension;
                if (!fileName.EndsWith(_extension))
                {
                    fileName += _extension;
                }
            }
            
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ExotischNederland", _subFolder);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            

            return Path.Combine(folderPath, fileName);
        }
    }
}