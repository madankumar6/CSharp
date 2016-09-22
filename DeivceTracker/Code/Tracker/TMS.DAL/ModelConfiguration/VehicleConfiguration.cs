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
    public class VehicleConfiguration : EntityTypeConfiguration<Vehicle>
    {
        public VehicleConfiguration()
        {
            ToTable("Vehicle");
            HasKey(prop => prop.VehicleId);
            HasRequired(i => i.Customer).WithMany(i => i.Vehicles).HasForeignKey(i => i.CustomerId).WillCascadeOnDelete(false);

            //Property(prop => prop.DealerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnOrder(0);
            //Property(prop => prop.UserId).HasColumnOrder(1);
            //Property(prop => prop.DistributorId).HasColumnOrder(2);
            //Property(prop => prop.Logo).HasColumnType("image");
            //HasRequired<Distributor>(prop => prop.Distributor).WithMany(prop => prop.Dealers).HasForeignKey(prop => prop.DistributorId);
        }   
    }
}
