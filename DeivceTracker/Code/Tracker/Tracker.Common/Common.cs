using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Tracker.Common
{
    public static class Common
    {
        public static Dictionary<string, object> GetValues(object obj)
        {
            Dictionary<string, object> rtn = new Dictionary<string, object>();

            try
            {
                foreach (var prop in obj.GetType().GetProperties())
                {
                    rtn.Add(prop.Name, prop.GetValue(obj, null));
                }
            }
            catch (Exception ex)
            {
            }

            return rtn;
        }

        public static SqlParameter[] ToSqlParameterList(this Dictionary<string, object> dictionaryCollection)
        {
            var sqlParameterCollection = new List<SqlParameter>();
            foreach (var parameter in dictionaryCollection)
            {
                sqlParameterCollection.Add(new SqlParameter(parameter.Key, parameter.Value));
            }
            return sqlParameterCollection.ToArray();
        }
    }
}
