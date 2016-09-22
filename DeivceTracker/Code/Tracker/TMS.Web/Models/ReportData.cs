using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TMS.Web.Models.ViewModels;
//using Tracker.Common;
using Tracker.Common.Model;
using System.Linq;
using _DAL = DAL;
using TMS.DAL;
using Tracker.Common;
using System.Web.Mvc;

namespace TMS.Web.Models
{
    public class ReportData
    {
        public ReportViewModel GetSummaryReport(ReportViewModel reportModel)
        {
            List<ReportResultViewModel> rData = new List<ReportResultViewModel>();

            try
            {
                DataTable dt = _DAL.Data.GetData(_DAL.DataBase.Api, System.Data.CommandType.StoredProcedure,
                    "Report_GetASummaryReport", new List<SqlParameter>() {
                    new SqlParameter("StartDate", reportModel.Parameter.StartDate),
                    new SqlParameter("EndDate", reportModel.Parameter.EndDate)
                }.ToArray());

                if (dt != null || dt.Rows.Count > 0)
                {
                    rData = dt.AsEnumerable().Select(m =>
                        new ReportResultViewModel()
                        {
                            SNo = Convert.ToInt32(m["SNo"]),
                            StartDate = Convert.ToDateTime(m["StartDate"]),
                            EndDate = Convert.ToDateTime(m["EndDate"]),
                            Lat = Convert.ToString(m["Lat"]),
                            Lang = Convert.ToString(m["Lang"]),
                            OnAcc = Convert.ToBoolean(m["OnAcc"])
                        }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            reportModel.Results = rData;

            return reportModel;
        }

        public ReportViewModel GetSpeedReport(ReportViewModel reportModel)
        {
            List<ReportResultViewModel> rData = new List<ReportResultViewModel>();

            try
            { 
                DataTable dt = _DAL.Data.GetData(_DAL.DataBase.Api, System.Data.CommandType.StoredProcedure,
                    "Report_SpeedReport", new List<SqlParameter>() {
                    new SqlParameter("StartDate", reportModel.Parameter.StartDate),
                    new SqlParameter("EndDate", reportModel.Parameter.EndDate),
                    new SqlParameter("Speed", reportModel.Parameter.Speed),
                    new SqlParameter("DeviceId",reportModel.Parameter.DeviceId)
                }.ToArray());

                if (dt != null || dt.Rows.Count > 0)
                {
                    rData = dt.AsEnumerable().Select(m =>
                        new ReportResultViewModel()
                        {
                            Speed = Convert.ToInt32(m["Speed"]),
                            //StartDate = Convert.ToDateTime(m["StartDate"]),
                            //EndDate = Convert.ToDateTime(m["EndDate"]),
                            Lat= Convert.ToString(m["Latitude"]),
                            Lang = Convert.ToString(m["Longitude"]),
                            DeviceId = Convert.ToString(m["DeviceId"]),
                            DeviceDataTime = Convert.ToDateTime(m["DeviceDataTime"])
                        }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            reportModel.Results = rData;

            return reportModel;
        }

    }
}