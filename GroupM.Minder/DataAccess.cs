using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace  GroupM.Minder
{
    class DataAccess
    {
        private SqlConnection sqlConn;
        public DataAccess()
        {
            sqlConn = new SqlConnection(Connection.ConnectionStringMinder);
            OpenConnection();
        }

        protected void OpenConnection()
        {
            if (sqlConn.State != System.Data.ConnectionState.Open)
                sqlConn.Open();
        }

        private int ImportCSV(string strSQL, List<string> listChannel, List<string> listDayPart, List<string> listProgType,
            List<string> listProgramme, List<string> listDayOfWeekVariables, List<string> listPerSpot, string FileName)
        {
            try
            {
                string sqlDelete = @"DELETE FROM CSVData";
                SqlCommand commDel = new SqlCommand(sqlDelete, sqlConn);
                commDel.ExecuteNonQuery();

                for (int i = 0; i < listChannel.Count() - 1; i++)
                {
                    if (i != 0)
                    {
                        string perSpot = listPerSpot[i].Replace("\", '", "");

                        SqlCommand comm = new SqlCommand(strSQL, sqlConn);
                        comm.Parameters.Add("@Channel", SqlDbType.NVarChar).Value = listChannel[i].Replace("\"", "");
                        comm.Parameters.Add("@DayPart", SqlDbType.NVarChar).Value = listDayPart[i].Replace("\"", "");
                        comm.Parameters.Add("@ProgType", SqlDbType.NVarChar).Value = listProgType[i].Replace("\"", "");
                        comm.Parameters.Add("@Programme", SqlDbType.NVarChar).Value = listProgramme[i].Replace("\"", "");
                        comm.Parameters.Add("@DayOfWeekVariables", SqlDbType.NVarChar).Value = listDayOfWeekVariables[i].Replace("\"", "");
                        comm.Parameters.Add("@PerSpot", SqlDbType.Int).Value = Convert.ToInt32(perSpot.Replace("'", ""));
                        comm.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
                        comm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = Connection.USERID;
                        comm.ExecuteNonQuery();

                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int ImportCSVConnString(List<string> listChannel, List<string> listDayPart, List<string> listProgType,
            List<string> listProgramme, List<string> listDayOfWeekVariables, List<string> listPerSpot, string FileName)
        {
            string strSQL = @"  INSERT INTO CSVData
	                                (Channel, DayPart, ProgType, Programme, DayOfWeekVariables, PerSpot, FileName, UserName)
                                VALUES
	                                (@Channel, @DayPart, @ProgType, @Programme, @DayOfWeekVariables, @PerSpot, @FileName, @UserName)
                            ";

            return ImportCSV(strSQL, listChannel, listDayPart, listProgType, listProgramme, listDayOfWeekVariables, listPerSpot, FileName);
        }

        public DataTable GetCSV()
        {
            string strSQL = @"  SELECT [Channel]
                                    ,[DayPart]
                                    ,[ProgType]
                                    ,[Programme]
                                    ,[DayOfWeekVariables]
                                    ,[PerSpot]
                                FROM CSVData";
            return LoadCSV(strSQL);
        }

        private DataTable LoadCSV(string strSQL)
        {
            SqlDataAdapter connSummary = new SqlDataAdapter(strSQL, sqlConn);
            DataSet ds = new DataSet();
            connSummary.Fill(ds);
            return ds.Tables[0];
        }

        public DataTable GetCSVByChannel(string channel)
        {
//            string strSQL = @"  SELECT DISTINCT DayPart, Programme, SUM(PerSpot)
//                                FROM [MPA].[dbo].[CSVData]
//                                WHERE Channel = @Channel
//                                AND DayOfWeekVariables != 'Sat'
//                                GROUP BY DayPart, Programme
//                                UNION 
//                                SELECT DISTINCT DayPart, Programme + ' - Sat', SUM(PerSpot)
//                                FROM [MPA].[dbo].[CSVData]
//                                WHERE Channel = @Channel
//                                AND DayOfWeekVariables = 'Sat'
//                                GROUP BY DayPart, Programme
//                                ORDER BY DayPart
//                            ";

            string strSQL = @"
                            SELECT DISTINCT DayPart, 

                            CASE WHEN (CHARINDEX('.',Programme) > 0) THEN 
	                            SUBSTRING(Programme,1, CHARINDEX('.',Programme) - 1)
                            ELSE
	                            Programme
                            END AS Programme_

                            , SUM(PerSpot)
                            FROM [MPA].[dbo].[CSVData]
                            WHERE Channel = @Channel
                            AND DayOfWeekVariables != 'Sat'
                            GROUP BY DayPart, 

CASE WHEN (CHARINDEX('.',Programme) > 0) THEN 
	SUBSTRING(Programme,1, CHARINDEX('.',Programme) - 1)
ELSE
	Programme
END 

                            UNION 

                            SELECT DISTINCT DayPart, CASE WHEN (CHARINDEX('.',Programme) > 0) THEN 
	                            SUBSTRING(Programme,1, CHARINDEX('.',Programme) - 1)
                            ELSE
	                            Programme
                            END  + ' - Sat' AS Programme_
                            , SUM(PerSpot)
                            FROM [MPA].[dbo].[CSVData]
                            WHERE Channel = @Channel
                            AND DayOfWeekVariables = 'Sat'
                            GROUP BY DayPart, 

                            CASE WHEN (CHARINDEX('.',Programme) > 0) THEN 
	                            SUBSTRING(Programme,1, CHARINDEX('.',Programme) - 1)
                            ELSE
	                            Programme
                            END

                            ORDER BY DayPart

                            ";




            return LoadCSVByChannel(strSQL, channel);
        }

        private DataTable LoadCSVByChannel(string strSQL, string channel)
        {
            SqlDataAdapter connQuery = new SqlDataAdapter(strSQL, sqlConn);
            connQuery.SelectCommand.Parameters.Add("@Channel", SqlDbType.NVarChar).Value = channel;
            DataSet ds = new DataSet();
            connQuery.Fill(ds);
            return ds.Tables[0];
        }
    }
}
