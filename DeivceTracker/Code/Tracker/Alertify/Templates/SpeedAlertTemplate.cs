using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alertify.Templates.Email
{
    public partial class SpeedAlertTemplate
    {
        public int Model { get; set; }
        public SpeedAlertTemplate(int Model) {
            this.Model = Model;
        }
    }
}
