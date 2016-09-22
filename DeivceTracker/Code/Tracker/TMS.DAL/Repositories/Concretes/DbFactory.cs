using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.DAL.Repositories.Interfaces;

namespace TMS.DAL.Repositories.Concretes
{
    public class DbFactory : Disposable, IDbFactory
    {
        private TMSEntities dbContext;

        public TMSEntities Init()
        {
            return dbContext ?? (dbContext = new TMSEntities());
        }

        protected override void DisposeObjects()
        {
            if (dbContext != null)
                dbContext.Dispose();
            base.DisposeObjects();
        }
    }
}
