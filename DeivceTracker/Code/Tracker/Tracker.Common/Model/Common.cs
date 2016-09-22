using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Tracker.Common.Model
{
    public class CustomSqlParameterGroup
    {
        public SqlParameter[] Parameters { get; set; }
        public TVParameter[] TableValuedParameters { get; set; }        
        public CustomSqlParameterGroup ChildParameters { get; set; }
    }
}
