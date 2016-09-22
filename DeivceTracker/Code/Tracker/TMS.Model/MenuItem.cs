using System.Collections.Generic;

namespace TMS.Model
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Text { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string URL { get; set; }
        public int? MenuOrder { get; set; }
        public string CSSClass { get; set; }
        public bool Enabled { get; set; }
        
        public int? ParentMenuId { get; set; }
        public virtual MenuItem ParentMenu { get; set; }
        public virtual ICollection<MenuItem> Children { get; set; }
        public int? MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
