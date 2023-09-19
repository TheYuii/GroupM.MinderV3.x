using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace GRM.UTL
{
    public class LogFileCreator
    {


        string m_ApplicationName = "";
        bool m_IsWriteLogToDataBase = false;
        SqlConnection m_DBConnection = null;
        public string ApplicationName { get { return m_ApplicationName; } set { m_ApplicationName = value; } }
        public bool IsWriteLogToDataBase { get { return m_IsWriteLogToDataBase; } set { m_IsWriteLogToDataBase = value; } }
        public SqlConnection DBConnection { get { return m_DBConnection; } set { m_DBConnection = value; } }

        public LogFileCreator()
        {
            ApplicationName = "File";
            IsWriteLogToDataBase = false;
        }
        public LogFileCreator(string strApplicationName)
        {
            ApplicationName = strApplicationName;
            IsWriteLogToDataBase = false;
        }
        
        public void WriteLog(string strStatus, string strLog)
        {
            WriteLog(string.Format(strStatus.ToUpper()+"|"+strLog));
        }
        public void WriteLog(string strLog)
        {
            string strDirectoryPath = Directory.GetCurrentDirectory() + "\\Log";
            string strFileLogPath = strDirectoryPath + "\\" + string.Format("Log_{0}_", ApplicationName) + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (!Directory.Exists(strDirectoryPath))
                Directory.CreateDirectory(strDirectoryPath);

            FileStream fs;
            if (File.Exists(strFileLogPath))
                fs = File.Open(strFileLogPath, FileMode.Append, FileAccess.Write);
            else
                fs = File.Open(strFileLogPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII);
            sw.WriteLine(System.DateTime.Now + " | " + strLog);
            sw.Flush();
            sw.Close();
            
            if (IsWriteLogToDataBase)
                WriteLogToDataBase(strLog);
        }
        
        public void ErrorLog(string strLog)
        {
            string strDirectoryPath = Directory.GetCurrentDirectory() + "\\Log";
            string strFileLogPath = strDirectoryPath + "\\" + string.Format("Error_{0}_", ApplicationName) + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            if (!Directory.Exists(strDirectoryPath))
                Directory.CreateDirectory(strDirectoryPath);

            FileStream fs;
            if (File.Exists(strFileLogPath))
                fs = File.Open(strFileLogPath, FileMode.Append, FileAccess.Write);
            else
                fs = File.Open(strFileLogPath, FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII);
            sw.WriteLine(System.DateTime.Now + Environment.NewLine + strLog);
            sw.Flush();
            sw.Close();

            if(IsWriteLogToDataBase)
                WriteErrorLogToDataBase(strLog);
        }

        private void WriteErrorLogToDataBase(string strLog)
        {
            WriteLogToDataBase("ERROR", strLog);
        }
        private void WriteLogToDataBase(string strLog)
        {
            WriteLogToDataBase("COMPLETED", strLog);
        }
        private void WriteLogToDataBase(string strStatus,string strLog)
        {
            string sqlInsert = string.Format(@"INSERT INTO dbo.LOG(logApplication,LogStatus,LogMessage) VALUES('{0}','{1}','{2}')", this.ApplicationName,strStatus, strLog);
            SqlCommand comm = new SqlCommand(sqlInsert, this.DBConnection);
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlInsert;
            comm.ExecuteNonQuery();      
        }
    }
}
