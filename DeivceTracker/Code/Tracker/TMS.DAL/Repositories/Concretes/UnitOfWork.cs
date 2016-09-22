using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;

namespace TMS.DAL.Repositories.Concretes
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbFactory dbFactory;
        private TMSEntities dbContext;

        public UnitOfWork()
        {
            
        }

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public TMSEntities DbContext
        {
            get { return this.dbContext ?? (this.dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.Commit();
        }
    }
}
