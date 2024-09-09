using System;
using System.Data;
using System.Xml.Linq;
namespace Training.LYVOut.PO
{
    internal class OutPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal string GetEmployee(string UserID)
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            DataTable dt = new DataTable();
            string Department = "";
            string USERNAME = "";
            string Flag = "";
            string Factory = "";

            string cmdTxt = @"SELECT ST_NHANVIEN.NV_Ma, ST_NHANVIEN.NV_Ten, ST_NHANVIEN.DV_MA_, ISNULL(ST_NHANVIENTHOIVIEC.NV_Ma, 'Employee') AS Flag, ST_DONVI.KHU 
                              FROM ST_NHANVIEN 
                              LEFT JOIN ST_NHANVIENTHOIVIEC ON ST_NHANVIENTHOIVIEC.NV_Ma = ST_NHANVIEN.NV_Ma 
                              LEFT JOIN ST_DONVI ON ST_DONVI.DV_MA = ST_NHANVIEN.DV_MA 
                              WHERE ST_NHANVIEN.NV_Ma = @UserID";

            this.m_db.AddParameter("@UserID", UserID);
            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            if (dt.Rows.Count > 0)
            {
                USERNAME = dt.Rows[0][1].ToString();
                Department = dt.Rows[0][2].ToString();
                Flag = dt.Rows[0][3].ToString();
                Factory = dt.Rows[0][4].ToString();
            }

            string result = USERNAME + ";" + Department + ";" + Flag + ";" + Factory;

            this.m_db.Dispose();

            return result;

        }
        internal void InsertOutData(string LNO, string UserID, string DepID, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string LeaveTime = xE.Attribute("LeaveTime").Value;
            string ReturnTime = xE.Attribute("ReturnTime").Value;
            string Type = xE.Attribute("Type").Value;
            string Category = xE.Attribute("Category").Value;
            string Item = xE.Attribute("Item").Value;
            string Reason = xE.Attribute("Reason").Value;

            int Person = Convert.ToInt32(xE.Attribute("Person").Value);

            string cmdTxt = @"  INSERT INTO WF_Out
                                (	 [LNO] ,  
                                     [Department] , 
                                     [LeaveTime] ,
                                     [ReturnTime] ,
                                     [Type] ,
                                     [Category] ,
                                     [Item] ,
                                     [Reason] , 
                                     [UserID] ,
                                     [UserDate] ,
                                     [flowflag]
                                ) 
                                 VALUES 
                                 (	
                                     @LNO,
                                     @Department,
                                     " + (string.IsNullOrEmpty(LeaveTime) ? "NULL" : "@LeaveTime") + @",
                                     " + (string.IsNullOrEmpty(ReturnTime) ? "NULL" : "@ReturnTime") + @",
                                     " + (string.IsNullOrEmpty(Type) ? "NULL" : "@Type") + @",
                                     " + (string.IsNullOrEmpty(Category) ? "NULL" : "@Category") + @",
                                     " + (string.IsNullOrEmpty(Item) ? "NULL" : "@Item") + @",
                                     " + (string.IsNullOrEmpty(Reason) ? "NULL" : "@Reason") + @",
                                     @UserID,
                                     getdate(),
                                     @flowflag
                                )  ";

            this.m_db.AddParameter("@LNO", LNO);
            this.m_db.AddParameter("@Department", DepID);
            this.m_db.AddParameter("@LeaveTime", LeaveTime);
            this.m_db.AddParameter("@ReturnTime", ReturnTime);
            this.m_db.AddParameter("@Type", Type);
            this.m_db.AddParameter("@Category", Category);
            this.m_db.AddParameter("@Item", Item);
            this.m_db.AddParameter("@Reason", Reason);
            this.m_db.AddParameter("@UserID", UserID);
            this.m_db.AddParameter("@flowflag", "Z");

            for (int i = 0; i < Person; i++)
            {
                cmdTxt += @"  
                           INSERT INTO WF_OutS
                           (	[LNO] ,  
                                [ID] , 
                                [Name] ,
                                [Factory] ,
                                [Department] 
                           ) 
                           VALUES 
                           (	
                                @LNO,
                                @ID" + i.ToString() + @",
                                @Name" + i.ToString() + @",
                                @Factory" + i.ToString() + @",
                                @Department" + i.ToString() + @"
                           )  ";
                XElement xE_1 = xE.Element("LYN_Out_" + i.ToString());
                this.m_db.AddParameter("@ID" + i.ToString(), xE_1.Attribute("ID").Value);
                this.m_db.AddParameter("@Name" + i.ToString(), xE_1.Attribute("Name").Value);
                this.m_db.AddParameter("@Factory" + i.ToString(), xE_1.Attribute("Factory").Value);
                this.m_db.AddParameter("@Department" + i.ToString(), xE_1.Attribute("Department").Value);

            }
            this.m_db.ExecuteNonQuery(cmdTxt);

            this.m_db.Dispose();
        }
        internal string GetOut(string LNO)
        {
            string Status = "";
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT CASE WHEN WF_Out.CheckDate IS NOT NULL THEN 'out' ELSE CASE WHEN Cancel = 1 THEN 'cancel' ELSE '' END END AS Status FROM WF_Out
                              WHERE LNO = '" + LNO + "' ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            if (dt.Rows.Count > 0)
                Status = dt.Rows[0][0].ToString();

            return Status;

        }
        public void UpdateCancelReason(string LNO, string CancelReason)
        {
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"UPDATE WF_Out SET Cancel = 1, CancelReason = '" + CancelReason + "' WHERE LNO = '" + LNO + "' ";
            this.m_db.ExecuteNonQuery(cmdTxt);
            this.m_db.Dispose();
        }
        internal DataTable GetListOut(string D_STEP_DESC, string Type, string LNO, string OutID, string LeaveTime, string ReturnTime, string UserDate1, string UserDate2, string OutCheck, string UserID)
        {

            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string where = "";
            if (D_STEP_DESC == "ALL")
            {
                if (OutID != "")
                {
                    where += " and WF_OutS1.ID = '" + OutID + "' ";
                    if (LNO != "") where += " and LOWER(WF_Out.LNO) like LOWER('%" + LNO + "%') ";
                    if (LeaveTime != "") where += " and WF_Out.LeaveTime >= '" + LeaveTime + "' ";
                    if (ReturnTime != "") where += " and WF_Out.ReturnTime <= '" + ReturnTime + "' ";
                }
                else
                {
                    if (Type != "") where += " and LOWER(WF_Out.Type) = LOWER(N'" + Type + "') ";
                    if (LNO != "") where += " and LOWER(WF_Out.LNO) like LOWER('%" + LNO + "%') ";
                    if (LeaveTime != "") where += " and WF_Out.LeaveTime >= '" + LeaveTime + "' ";
                    if (ReturnTime != "") where += " and WF_Out.ReturnTime <= '" + ReturnTime + "' ";
                    if (UserDate1 != "") where += " and WF_Out.UserDate >= '" + UserDate1 + "' ";
                    if (UserDate2 != "") where += " and WF_Out.UserDate <= '" + UserDate2 + "' ";
                    if (OutCheck != "") where += " and " + OutCheck;
                }
            }
            else
            {
                where += " and WF_Out.UserID = '" + UserID + "' ";
                if (Type != "") where += " and LOWER(WF_Out.Type) = LOWER(N'" + Type + "') ";
                if (LeaveTime != "") where += " and WF_Out.LeaveTime >= '" + LeaveTime + "' ";
                if (ReturnTime != "") where += " and WF_Out.ReturnTime <= '" + ReturnTime + "' ";
                if (UserDate1 != "") where += " and WF_Out.UserDate >= '" + UserDate1 + "' ";
                if (UserDate2 != "") where += " and WF_Out.UserDate <= '" + UserDate2 + "' ";
            }
            string SQL = @" SELECT CASE WHEN WF_Out.CheckDate IS NOT NULL THEN '已外出' ELSE CASE WHEN WF_Out.Cancel = 1 THEN '已註銷' ELSE '未外出' END END AS Status, WF_Out.CancelReason, WF_Out.LNO, WF_Out.Department, WF_OutS.People, 
                                   CONVERT(VARCHAR,WF_Out.LeaveTime,120) LeaveTime, CONVERT(VARCHAR,WF_Out.ReturnTime,120) ReturnTime, WF_Out.Type, WF_Out.Category, WF_Out.Item, WF_Out.Reason, WF_Out.UserID, 
                                   CONVERT(VARCHAR,WF_Out.UserDate,120) UserDate, BPM.TASK_ID 
                            FROM WF_Out 
                            LEFT JOIN (SELECT LNO AS ListNo, COUNT(ID) AS People FROM WF_OutS GROUP BY LNO) AS WF_OutS ON WF_OutS.ListNo = WF_Out.LNO 
                            LEFT JOIN (SELECT LNO AS ListNo,ID FROM WF_OutS) AS WF_OutS1 ON WF_OutS1.ListNo = WF_Out.LNO 
                            LEFT JOIN (SELECT TASK_ID, DOC_NBR FROM OPENQUERY([UOFWEB],'SELECT TASK_ID, DOC_NBR FROM [UOF].[dbo].[TB_WKF_TASK] TB_WKF_TASK ') AS SYS_TODOHIS 
                                        )AS BPM on BPM.DOC_NBR COLLATE Chinese_Taiwan_Stroke_CS_AS=WF_Out.LNO COLLATE Chinese_Taiwan_Stroke_CS_AS 
                            LEFT JOIN BUSERS ON BUSERS.UserID = WF_Out.UserID 
                            WHERE 1=1 " + where + @" 
                            ORDER BY WF_Out.LNO desc ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));

            this.m_db.Dispose();

            return dt;
        }
    }
}
