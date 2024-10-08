using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland
{
    internal class Helpers
    {
        static string MenuSelect(Dictionary<string, string> items, bool indexes = false)
        {
            int selected = 0;
            while (true)
            {
                Console.Clear();
                foreach (var item in items)
                {
                    if (items.Keys.ToList().IndexOf(item.Key) == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (indexes) Console.Write(items.Keys.ToList().IndexOf(item.Key) + 1 + ". ");
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
                        selected = items.Count - 1;
                    }
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected > items.Count - 1)
                    {
                        selected = 0;
                    }
                }
                else if (input == ConsoleKey.Enter)
                {
                    return items.Keys.ToList()[selected];
                }
            }
        }
    }
}
