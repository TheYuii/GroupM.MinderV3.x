using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace GroupM.UTL
{
    public class DataControl
    {
        private SqlConnection export_sqlConn;
        public DataControl()
        {
            //Set connection to OMI
            export_sqlConn = new SqlConnection(AccessConstant.MPA);
            OpenConnection();
        }
        private void OpenConnection()
        {
            if (export_sqlConn.State != System.Data.ConnectionState.Open)
                export_sqlConn.Open();
        }

        public DataTable GetSummaryCD(string MonthYear, string Agency)
        {
            MonthYear = "2014/09";
            Agency = "";

            string strSQL = string.Format(@"select sumCD.*
                                                ,inputSum.RVNo
		                                        ,inputSum.PayInDate [Pay In Date]
		                                        ,isnull(inputSum.AmountReceive, 0) [Amount Receive]
		                                        ,(isnull(inputSum.AmountReceive, 0) - sumCD.[Sum of Total CD]) [Over Paid or shart]
		                                        ,inputSum.Remark
                                            from (
	                                            SELECT ImportYearMonth, Agency, CH, VendorName [Vendor Name], Sum(TotalCD) [Sum Of Spending], Sum(Net) [Sum of Total CD]
	                                            FROM [dbo].[SummaryCD]
	                                            GROUP BY CH, VendorName, Agency, ImportYearMonth
                                            ) sumCD
                                                LEFT JOIN dbo.SummaryCD_MS inputSum
	                                            ON 
		                                        sumCD.[Vendor Name] = inputSum.VendorName
		                                        and sumCD.CH = inputSum.CH
		                                        and sumCD.Agency	= inputSum.Agency 
		                                        and sumCD.ImportYearMonth = inputSum.ImportYearMonth
                                            WHERE {0} sumCD.ImportYearMonth = @MonthYear
                                            ORDER BY sumCD.CH DESC
                                            ", (Agency != "" ? "sumCD.Agency = @Agency and " : ""));

            SqlDataAdapter connSummary = new SqlDataAdapter(strSQL, export_sqlConn);
            connSummary.SelectCommand.Parameters.Add("@Agency", SqlDbType.NVarChar).Value = Agency;
            connSummary.SelectCommand.Parameters.Add("@MonthYear", SqlDbType.NVarChar).Value = MonthYear.Replace("/", "");

            DataSet ds = new DataSet();
            connSummary.Fill(ds);
            return ds.Tables[0];
        }
    }
}
