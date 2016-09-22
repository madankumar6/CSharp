using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;
using TMS.Model;
using Tracker.Common;
using System.Data.Entity;

namespace TMS.DAL.Repositories.Concretes
{
    public class UserRepository<T> : Repository<T>, IUserRepository<T> where T : User 
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public T GetUserByUsername(string username)
        {
            return Get(user => user.Username == username);
        }

        public T Getuser(Guid userId)
        {
            return Get(user => user.UserId == userId);
        }

        public T GetUserByType(Type type)
        {
            throw new NotImplementedException();
        }

        public bool IsUserExists(string username)
        {
            return this.DbContext.Users.OfType<T>().Any(user => user.Username == username);
        }

        public override CollectionPage<T> GetMany(int page, int itemsPerPage)
        {
            return GetMany(null, page, itemsPerPage);
        }

        public override IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            IQueryable<T> queryBuilder = this.dbSet;
            return queryBuilder.Where(where).ToList();
        }

        public override CollectionPage<T> GetMany(Expression<Func<T, bool>> where, int page, int itemsPerPage)
        {
            IQueryable<T> queryBuilder = this.dbSet;

            //if (where != null)
            //{
            //    queryBuilder.Where(where);
            //}

            queryBuilder = ((IOrderedQueryable<T>)queryBuilder).OrderBy(item => (true));

            var pageOfResult = new CollectionPage<T>()
            {
                CurrentPage = page,
                TotalItems = where == null ? queryBuilder.Count() : queryBuilder.Count(where),
                ItemsPerPage = itemsPerPage,
                Items = where == null ? queryBuilder.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList() : queryBuilder.Where(where).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
            };
            return pageOfResult;
        }
    }
}
