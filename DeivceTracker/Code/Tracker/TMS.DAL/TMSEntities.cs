using TMS.DAL.ModelConfiguration;

namespace TMS.DAL
{
    using System.Data.Entity;
    using Model;

    public class TMSEntities : DbContext
    {
        public TMSEntities()
            : base("name=TMSEntities")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = true;
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<Dealer> Dealers { get; set; } 
        public DbSet<Customer> Customers { get; set; } 
        public DbSet<Vehicle> Vehicles { get; set; }
      //  public DbSet<DeviceModels> DeviceModels { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<DeviceModels> DeviceModels { get; set; }
      //  public DbSet<ProtocolServer> ProtocolServers { get; set; }
        //public DbSet<DeviceInfo> DeviceInfos { get; set; }
        //public DbSet<DeviceInfoHistory> DeviceInfoHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Address>();
            modelBuilder.Configurations.Add(new MenuConfiguration());
            modelBuilder.Configurations.Add(new MenuItemConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            //modelBuilder.Entity<Admin>().HasMany(prop => prop.Distributors).WithRequired();
            //modelBuilder.Entity<Distributor>().HasMany(prop => prop.Dealers).WithRequired();
            //modelBuilder.Entity<Dealer>().HasMany(prop => prop.Customers).WithRequired();
            modelBuilder.Configurations.Add(new VehicleConfiguration());
            modelBuilder.Configurations.Add(new DeviceConfiguration());
            modelBuilder.Configurations.Add(new DeviceModelsConfiguration());
            modelBuilder.Configurations.Add(new DeviceTypeConfiguration());
           // modelBuilder.Configurations.Add(new ProtocolServerConfiguration());
            //modelBuilder.Configurations.Add(new DeviceInfoHistoryConfiguration());
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
