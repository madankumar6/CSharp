using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TMS.DAL.ModelConfiguration
{
    public class MenuItemConfiguration : EntityTypeConfiguration<TMS.Model.MenuItem>
    {
        public MenuItemConfiguration()
        {
            ToTable("MenuItem");
            HasKey(prop => prop.MenuItemId);
            Property(prop => prop.MenuItemId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnOrder(0);
            HasRequired(prop => prop.Menu).WithMany(prop => prop.MenuItems).HasForeignKey(prop => prop.MenuId).WillCascadeOnDelete(true);
            HasOptional(prop => prop.ParentMenu).WithMany(prop => prop.Children).HasForeignKey(prop => prop.ParentMenuId).WillCascadeOnDelete(false);
        }
    }
}
