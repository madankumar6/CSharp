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
    public class DeviceInfoHistoryConfiguration : EntityTypeConfiguration<DeviceInfoHistory>
    {
        public DeviceInfoHistoryConfiguration()
        {
            ToTable("DeviceInfoHistory");
            HasKey(prop => prop.DeviceInfoHistoryId);
            Property(prop => prop.IMEI).HasMaxLength(50);
            Property(prop => prop.CommandType).HasMaxLength(50);
            Property(prop => prop.StatusCode).HasMaxLength(50);
            Property(prop => prop.Latitude).HasMaxLength(50);
            Property(prop => prop.Longitude).HasMaxLength(50);
            Property(prop => prop.Altitude).HasMaxLength(50);
            Property(prop => prop.Speed).HasMaxLength(50);
            Property(prop => prop.Direction).HasMaxLength(50);
            Property(prop => prop.Mileage).HasMaxLength(50);
            Property(prop => prop.ValidData).HasMaxLength(50);
            Property(prop => prop.FullAddress).HasMaxLength(4000);
            Property(prop => prop.PayLoad).HasColumnType("TEXT");
            Property(prop => prop.UnparsedPayLoad).HasColumnType("TEXT");
            Property(prop => prop.Distance).HasMaxLength(50);
            Property(prop => prop.Odometer).HasMaxLength(50);
            Property(prop => prop.OnBattery);
            Property(prop => prop.OnIgnition);
            Property(prop => prop.OnAc);
            Property(prop => prop.OnGps);
            Property(prop => prop.UnKnown).HasMaxLength(50);
            Property(prop => prop.GeoZoneIndex).HasMaxLength(50);
            Property(prop => prop.GeoZoneId).HasMaxLength(50);
            Property(prop => prop.TrackerIp).HasMaxLength(50);
        }
    }
}
