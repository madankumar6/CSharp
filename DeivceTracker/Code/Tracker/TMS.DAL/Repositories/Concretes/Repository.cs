using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TMS.DAL.Repositories.Interfaces;
using Tracker.Common;

namespace TMS.DAL.Repositories.Concretes
{
    public abstract class Repository<T>  where T : class 
    {
        #region Properties
        private TMSEntities dataContext;
        protected internal readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected TMSEntities DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        protected Repository(IDbFactory dbFactory)
        {
            this.DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual CollectionPage<T> GetMany(int page, int itemsPerPage)
        {
            return GetMany(null, page, itemsPerPage);
        }
        
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public virtual CollectionPage<T> GetMany(Expression<Func<T, bool>> where, int page, int itemsPerPage)
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
                TotalItems = queryBuilder.Count(where),
                ItemsPerPage = itemsPerPage,
                Items = queryBuilder.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
            };
            return pageOfResult;
        }

        #endregion
    }
}
