using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using TMS.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.DAL.ModelConfiguration
{
    public class DeviceConfiguration : EntityTypeConfiguration<Device>
    {
        public DeviceConfiguration()
        {
            ToTable("Device");
            HasKey(prop => prop.DeviceId);
            HasRequired(i => i.Customer).WithMany(i => i.Devices).HasForeignKey(i => i.CustomerId);
            HasRequired(i => i.Vehicle).WithMany(i => i.Devices).HasForeignKey(i => i.VehicleId);

            //Property(prop => prop.CustomerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnOrder(0);
            //Property(prop => prop.UserId).HasColumnOrder(1);
            //Property(prop => prop.DealerId).HasColumnOrder(2);
            //HasRequired<Dealer>(prop => prop.Dealer).WithMany(prop => prop.Customers).HasForeignKey(prop => prop.DealerId);
        }
    }
}
