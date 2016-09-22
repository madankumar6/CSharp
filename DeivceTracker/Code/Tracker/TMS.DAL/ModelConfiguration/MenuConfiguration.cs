using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TMS.DAL.ModelConfiguration
{
    public class MenuConfiguration : EntityTypeConfiguration<TMS.Model.Menu>
    {
        public MenuConfiguration()
        {
            ToTable("Menu");
            HasKey(prop => prop.MenuId);
            Property(prop => prop.MenuId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnOrder(0);
        }
    }
}
