using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Model;
using Tracker.Common;

namespace TMS.BusinessRule.Interfaces
{
    public interface IAlertService
    {
        bool SaveAlert(Dealer dealer);
        Dealer GetAlerts(Guid dealerId);
        bool DeleteAlert(string[] dealerIds);
    }
}
