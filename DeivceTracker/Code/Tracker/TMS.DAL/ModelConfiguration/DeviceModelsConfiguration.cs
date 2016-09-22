using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;

namespace TMS.DAL.ModelConfiguration
{
   public class DeviceModelsConfiguration : EntityTypeConfiguration<DeviceModels>
    {
        public DeviceModelsConfiguration()
        {
            ToTable("DeviceModels");
            HasKey(prop => prop.DeviceId);
        }
    }
}
