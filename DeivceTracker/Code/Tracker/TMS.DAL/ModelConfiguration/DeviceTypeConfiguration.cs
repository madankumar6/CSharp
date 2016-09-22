using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using TMS.Model;

namespace TMS.DAL.ModelConfiguration
{
  public  class DeviceTypeConfiguration: EntityTypeConfiguration<DeviceType>
    {
        public DeviceTypeConfiguration()
        {
            ToTable("DeviceType");
            HasKey(prop => prop.DeviceTypeId);
            Property(prop => prop.DeviceTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }

    public class ProtocolServerConfiguration: EntityTypeConfiguration<ProtocolServer>
    {
        public ProtocolServerConfiguration()
        {
            ToTable("ProtocolServer");
        }
    }
}
