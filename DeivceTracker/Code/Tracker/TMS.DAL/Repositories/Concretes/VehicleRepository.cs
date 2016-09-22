using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;
using System.Linq.Expressions;

namespace TMS.DAL.Repositories.Concretes
{
   public class VehicleRepository : Repository<Vehicle>, IVehicleRepository 
    {

        public VehicleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public List<Vehicle> GetVehiclesByNumber(string vehicleNo)
        {
            return
                this.DbContext.Vehicles.OfType<Vehicle>()
                    .Where(vehicle => vehicle.VehicleNo == vehicleNo)
                    .ToList();
        }

        public bool IsVehicleExists(string VehicleNo)
        {
            return this.DbContext.Vehicles.OfType<Vehicle>().Any(user => user.VehicleNo == VehicleNo);
        }

        //public T GetVehicleByVehicleNo(string vehicleNo)
        //{
        //    return Get(vehicle => vehicle.VehicleNo == vehicleNo);
        //}

        //public T GetVehicle(Guid vehicleId)
        //{
        //    return Get(vehicle => vehicle.VehicleId == vehicleId);
        //}

        ////public T GetvehicleByType(Type type)
        ////{
        ////    throw new NotImplementedException();
        ////}

        //public bool IsVehicleExists(string vehicleNo)
        //{
        //    return this.DbContext.Vehicles.OfType<T>().Any(vehicle => vehicle.VehicleNo == vehicleNo);
        //}

        ////public override CollectionPage<T> GetMany(int page, int itemsPerPage)
        ////{
        ////    return GetMany(null, page, itemsPerPage);
        ////}

        //public override IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        //{
        //    IQueryable<T> queryBuilder = this.dbSet;
        //    return queryBuilder.Where(where).ToList();
        //}

        //public override CollectionPage<T> GetMany(Expression<Func<T, bool>> where, int page, int itemsPerPage)
        //{
        //    IQueryable<T> queryBuilder = this.dbSet;

        //    //if (where != null)
        //    //{
        //    //    queryBuilder.Where(where);
        //    //}

        //    queryBuilder = ((IOrderedQueryable<T>)queryBuilder).OrderBy(item => (true));

        //    var pageOfResult = new CollectionPage<T>()
        //    {
        //        CurrentPage = page,
        //        TotalItems = where == null ? queryBuilder.Count() : queryBuilder.Count(where),
        //        ItemsPerPage = itemsPerPage,
        //        Items = where == null ? queryBuilder.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList() : queryBuilder.Where(where).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
        //    };
        //    return pageOfResult;
        //}
    }
}
