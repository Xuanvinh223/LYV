using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Data;
using System.Xml.Linq;

namespace LYV.BusinessTripReport.PO
{
    internal class BusinessTripReportPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal DataTable GetListBT(string LYV, string Name, string Name_ID, string BTime1, string BTime2)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string where = " ";
            if (LYV != "") where += " and LOWER(LYV) like LOWER('%" + LYV + "%') ";
            if (Name != "") where += " and LOWER(Name) like LOWER(N'%" + Name + "%') ";
            if (Name_ID != "") where += " and Name_ID like '" + Name_ID + "%' ";
            if (BTime1 != "") where += " and BTime >= '" + BTime1 + "' ";
            if (BTime2 != "") where += " and BTime <= '" + BTime2 + "' ";

            string SQL = @"SELECT LYV, Name, Name_ID, Purpose, FLocation, BTime, ETime, USERID, USERDATE, TASK_ID 
                           FROM LYV_BusinessTrip LEFT JOIN TB_WKF_TASK on LYV_BusinessTrip.LYV=TB_WKF_TASK.DOC_NBR 
                           WHERE isnull(Days,2)>=2 and flowflag='Z' and LYV not in (select BLYV as LYV from LYV_BusinessTripReport where isnull(Cancel,0)<>1) " + where + @"
                           ORDER BY LYV_BusinessTrip.LYV desc ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));

            this.m_db.Dispose();

            return dt;
        }
        internal DataTable GetBusinessTripReport_BLYV(string BLYV)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string SQL = @"SELECT LYV, Name, Name_ID, Purpose, FLocation, BTime, ETime, USERID, USERDATE, TASK_ID 
                           FROM LYV_BusinessTrip LEFT JOIN TB_WKF_TASK on LYV_BusinessTrip.LYV=TB_WKF_TASK.DOC_NBR 
                           WHERE LYV = " + BLYV + @"
                           ORDER BY LYV_BusinessTrip.LYV desc ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));

            this.m_db.Dispose();

            return dt;
        }
        internal void InsertBusinessTripReportData(string LYV, string UserID, string Department, string Date, string Date1, string Name, string Destination, string Description, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string RLYV = xE.Attribute("BLYV").Value;

            string cmdTxt = @"  INSERT INTO LYV_BusinessTripReport
                                (	 [LYV] ,  
                                     [Department] , 
                                     [Date] ,
                                     [Date1] ,
                                     [Name] ,
                                     [Destination] ,
                                     [Description] ,
                                     [RLYV] ,
                                     [USERID] ,
                                     [USERDATE] ,
                                     [flowflag]
                                ) 
                                 VALUES 
                                 (	
                                     @LYV,
                                     @Department,
                                     @Date,
                                     @Date1,
                                     @Name,
                                     @Destination,
                                     @Description,
                                     @RLYV,
                                     @UserID,
                                     getdate(),
                                     @flowflag
                                )  ";

            this.m_db.AddParameter("@LYV", LYV);
            this.m_db.AddParameter("@Department", Department);
            this.m_db.AddParameter("@Date", Date);
            this.m_db.AddParameter("@Date1", Date1);
            this.m_db.AddParameter("@Name", Name);
            this.m_db.AddParameter("@Destination", Destination);
            this.m_db.AddParameter("@Description", Description);
            this.m_db.AddParameter("@RLYV", RLYV);
            this.m_db.AddParameter("@UserID", UserID);
            this.m_db.AddParameter("@flowflag", "Z");

            this.m_db.ExecuteNonQuery(cmdTxt);

            this.m_db.Dispose();
        }
        internal string GetBusinessTripReport(string LYV)
        {
            string Status = "";
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT CASE WHEN CFMID IS NOT NULL THEN 'cfm' ELSE CASE WHEN Cancel = 1 THEN 'cancel' ELSE '' END END AS Status FROM LYV_BusinessTripReport
                              WHERE LYV = '" + LYV + "' ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            if (dt.Rows.Count > 0)
            Status = dt.Rows[0][0].ToString();

            return Status;

        }
        public void UpdateCancelReason(string LYV, string CancelReason)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"UPDATE LYV_BusinessTripReport SET Cancel = 1, CancelReason = '" + CancelReason + "' WHERE LYV = '" + LYV + "' ";
            this.m_db.ExecuteNonQuery(cmdTxt);
            this.m_db.Dispose();
        }
        public void Confirm(string LYV, string CFMID)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"UPDATE LYV_BusinessTripReport SET CFMID = '" + CFMID + "', CFMDATE=getdate() WHERE LYV = '" + LYV + "' ";
            this.m_db.ExecuteNonQuery(cmdTxt);
            this.m_db.Dispose();
        }
    }
}
