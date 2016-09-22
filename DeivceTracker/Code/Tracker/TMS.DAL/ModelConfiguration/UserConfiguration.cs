using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;

namespace TMS.DAL.ModelConfiguration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("User");
            HasKey(prop => prop.UserId);
            Property(prop => prop.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(prop => prop.FirstName).HasMaxLength(50);
            Property(prop => prop.LastName).HasMaxLength(50);
            Property(prop => prop.RowVersion).IsRowVersion();
        }
    }
}
