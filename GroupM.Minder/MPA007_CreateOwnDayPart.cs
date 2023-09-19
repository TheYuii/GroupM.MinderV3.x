using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using GroupM.UTL;

namespace  GroupM.Minder
{
    public partial class MPA007_CreateOwnDayPart : Form
    {
        string m_strConnection = Connection.ConnectionStringMinder;
        enum eCol { Part_Name,Start_Time,End_Time,M,T,W,H,F,A,S}
        public MPA007_CreateOwnDayPart()
        {
            InitializeComponent();
        }

        private void Dataloading()
        {
            SqlConnection conn = new SqlConnection(m_strConnection);
            conn.Open();
            string strSQL = @"SELECT [Part_Name]
      ,[Start_Time]
      ,[End_Time]
      ,CASE WHEN CHARINDEX('M',Weekdaylimit) > 0 THEN 1 ELSE 0 END AS M
      ,CASE WHEN CHARINDEX('T',Weekdaylimit) > 0 THEN 1 ELSE 0 END AS T
      ,CASE WHEN CHARINDEX('W',Weekdaylimit) > 0 THEN 1 ELSE 0 END AS W
      ,CASE WHEN CHARINDEX('H',Weekdaylimit) > 0 THEN 1 ELSE 0 END AS H
      ,CASE WHEN CHARINDEX('F',Weekdaylimit) > 0 THEN 1 ELSE 0 END AS F
      ,CASE WHEN CHARINDEX('A',Weekdaylimit) > 0 THEN 1 ELSE 0 END AS A
      ,CASE WHEN CHARINDEX('S',Weekdaylimit) > 0 THEN 1 ELSE 0 END AS S
    FROM [Minder_Thai].[dbo].[MPA_OwnDayPart]
    WHERE CreateBy = @CreateBy";

            SqlDataAdapter sda = new SqlDataAdapter(strSQL, conn);
            sda.SelectCommand.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = Connection.USERID;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvDetail.DataSource = dt;
        }
        private void MPA007_CreateOwnDayPart_Load(object sender, EventArgs e)
        {
            Dataloading();
        }
        private bool CheckDataBeforeSave()
        {
            for (int i = 0; i < gvDetail.Rows.Count -1; i++)
            {
                if (gvDetail.Rows[i].Cells[(int)eCol.Part_Name].Value.ToString().Trim() == "")
                {
                    GMessage.MessageWarning("Day Part Name can not be empty.");
                    return false;
                }
                if (gvDetail.Rows[i].Cells[(int)eCol.Start_Time].Value.ToString().Trim() == "")
                {
                    GMessage.MessageWarning("Start Time Name can not be empty.");
                    return false;
                }
                if (gvDetail.Rows[i].Cells[(int)eCol.End_Time].Value.ToString().Trim() == "")
                {
                    GMessage.MessageWarning("End Time can not be empty.");
                    return false;
                }
                
            }

            return true;
        }
        private string GetWeekdaylimit(int i)
        {
            string strResult = "";
            strResult += gvDetail.Rows[i].Cells[(int)eCol.M].Value.ToString() == "1" ? "M" : "."; 
            strResult += gvDetail.Rows[i].Cells[(int)eCol.T].Value.ToString() == "1" ? "T" : ".";
            strResult += gvDetail.Rows[i].Cells[(int)eCol.W].Value.ToString() == "1" ? "W" : ".";
            strResult += gvDetail.Rows[i].Cells[(int)eCol.H].Value.ToString() == "1" ? "H" : ".";
            strResult += gvDetail.Rows[i].Cells[(int)eCol.F].Value.ToString() == "1" ? "F" : ".";
            strResult += gvDetail.Rows[i].Cells[(int)eCol.A].Value.ToString() == "1" ? "A" : ".";
            strResult += gvDetail.Rows[i].Cells[(int)eCol.S].Value.ToString() == "1" ? "S" : ".";
            return strResult;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckDataBeforeSave())
                return;
            SqlConnection conn = new SqlConnection(m_strConnection);
            conn.Open();
            SqlCommand comm = new SqlCommand("DELETE FROM MPA_OwnDayPart WHERE CreateBy = @CreateBy", conn);
            comm.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = Connection.USERID;
            comm.ExecuteNonQuery();

            comm.CommandText ="INSERT INTO MPA_OwnDayPart(Part_Name,Start_Time,End_Time,Weekdaylimit,CreateBy) VALUES(@Part_Name,@Start_Time,@End_Time,@Weekdaylimit,@CreateBy)";
            comm.Parameters.Add("@Part_Name", SqlDbType.VarChar).Value = "";
            comm.Parameters.Add("@Start_Time", SqlDbType.VarChar).Value = "";
            comm.Parameters.Add("@End_Time", SqlDbType.VarChar).Value = "";
            comm.Parameters.Add("@Weekdaylimit", SqlDbType.VarChar).Value = "";
            for (int i = 0; i < gvDetail.Rows.Count - 1; i++)
            {
                comm.Parameters["@Part_Name"].Value = gvDetail.Rows[i].Cells[(int)eCol.Part_Name].Value.ToString().Trim();
                comm.Parameters["@Start_Time"].Value = gvDetail.Rows[i].Cells[(int)eCol.Start_Time].Value.ToString().Trim();
                comm.Parameters["@End_Time"].Value = gvDetail.Rows[i].Cells[(int)eCol.End_Time].Value.ToString().Trim();
                comm.Parameters["@Weekdaylimit"].Value = GetWeekdaylimit(i) ;

                comm.ExecuteNonQuery();
            }
            conn.Close();
            GMessage.MessageInfo("Save Completed.");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Dataloading();
        }
    }
}