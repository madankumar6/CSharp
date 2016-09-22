using System.Collections.Generic;

namespace TMS.Model
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
