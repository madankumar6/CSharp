using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public enum DataBase
    {
        TcpServer,
        Api,
        Alert
    }

    public class TVParameter
    {
        public string ParameterName;
        public string ParameterTypeName;
        public SqlDbType SqlDbType;
        public object ParameterValue;
    }

    public static class Data
    {
        private static string _con;
        private static string GetConnectionStaring(DataBase dataBaseType)
        {
            if (_con == null)
            {
                _con = ConfigurationManager.ConnectionStrings[dataBaseType.ToString()].ConnectionString;
            }
            return _con;
        }

        public static DataTable GetData(DataBase dataBaseType, CommandType commandType, string query, SqlParameter[] parameters = null)
        {
            SqlConnection conn = new SqlConnection(GetConnectionStaring(dataBaseType));
            SqlCommand cmd = conn.CreateCommand();
            SqlDataReader reader = null;

            try
            {
                cmd.CommandType = commandType;
                cmd.CommandText = query;
                if (parameters != null)
                    parameters.ToList().ForEach(s =>
                    {
                        cmd.Parameters.Add(s);
                    });

                DataTable dt = new DataTable();

                conn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(reader);

                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (cmd != null)
                {
                    if (cmd.Parameters != null)
                    {
                        cmd.Parameters.Clear();
                    }
                    cmd.Dispose();
                }
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static int StoreData_ExecuteNonQuery(DataBase dataBaseType, CommandType commandType, string query, SqlParameter[] parameters = null)
        {
            return StoreData_ExecuteNonQuery(dataBaseType, commandType, query, parameters, null);
        }

        public static int StoreData_ExecuteNonQuery(DataBase dataBaseType, CommandType commandType, string query, SqlParameter[] parameters = null, List<TVParameter> TVParameters = null)
        {
            SqlConnection conn = new SqlConnection(GetConnectionStaring(dataBaseType));
            SqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandType = commandType;
                cmd.CommandText = query;
                if (parameters != null)
                    parameters.ToList().ForEach(s =>
                    {
                        cmd.Parameters.Add(s);
                    });
                if (TVParameters != null)
                    TVParameters.ToList().ForEach(s =>
                    {
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue(s.ParameterName, s.ParameterValue);
                        tvpParam.SqlDbType = s.SqlDbType;
                        tvpParam.TypeName = s.ParameterTypeName;
                    });
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    if (cmd.Parameters != null)
                    {
                        cmd.Parameters.Clear();
                    }
                    cmd.Dispose();
                }
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public static string StoreData_ExecuteScalar(DataBase dataBaseType, CommandType commandType, string query, SqlParameter[] parameters = null, List<TVParameter> TVParameters = null)
        {
            SqlConnection conn = new SqlConnection(GetConnectionStaring(dataBaseType));
            SqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandType = commandType;
                cmd.CommandText = query;
                if (parameters != null)
                    parameters.ToList().ForEach(s =>
                    {
                        cmd.Parameters.Add(s);
                    });
                if (TVParameters != null)
                    TVParameters.ToList().ForEach(s =>
                    {
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue(s.ParameterName, s.ParameterValue);
                        tvpParam.SqlDbType = s.SqlDbType;
                        tvpParam.TypeName = s.ParameterTypeName;
                    });
                conn.Open();
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    if (cmd.Parameters != null)
                    {
                        cmd.Parameters.Clear();
                    }
                    cmd.Dispose();
                }
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }


        public static void StoreData_ExecuteNonQuery_ParamList(DataBase dataBaseType, CommandType commandType, string query, List<SqlParameter[]> parametersList = null)
        {
            if (parametersList == null || parametersList.Count == 0)
            {
                StoreData_ExecuteNonQuery(dataBaseType, commandType, query, null);
                return;
            }
            else
            {
                parametersList.ForEach(pL =>
                {
                    StoreData_ExecuteNonQuery(dataBaseType, commandType, query, pL);
                });
            }
        }

    }
}
