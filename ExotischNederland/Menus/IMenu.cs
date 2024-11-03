using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal interface IMenu
    {
        void Show();
        Dictionary<string, string> GetMenuItems();
    }
}
