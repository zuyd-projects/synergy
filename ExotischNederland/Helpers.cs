using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland
{
    internal class Helpers
    {
        /// <summary>
        /// Create a menu with a list of items to select from
        /// </summary>
        /// <param name="items"></param>
        /// <param name="indexes"></param>
        /// <returns>The key of the selected item</returns>
        public static string MenuSelect(Dictionary<string, string> _items, bool _indexes = false)
        {
            int selected = 0;
            while (true)
            {
                Console.Clear();
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
    }
}
