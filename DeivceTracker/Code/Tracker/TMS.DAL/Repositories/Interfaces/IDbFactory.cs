using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.DAL.Repositories.Interfaces
{
    public interface IDbFactory : IDisposable
    {
        TMSEntities Init();
    }
}
