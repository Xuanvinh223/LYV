﻿using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LYV.BusinessTrip.PO
{
    internal class BusinessTripPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        private string conn = Training.Properties.Settings.Default.UOF.ToString();
        private string connectHRM = Training.Properties.Settings.Default.HRM.ToString();
        internal string GetLEV(string UserID, string groupID)
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmd = @"SELECT LEV 
                           FROM TB_EB_USER 
                           LEFT JOIN TB_EB_EMPL_DEP ON TB_EB_EMPL_DEP.USER_GUID=TB_EB_USER.USER_GUID 
                           LEFT JOIN TB_EB_GROUP ON TB_EB_GROUP.GROUP_ID=TB_EB_EMPL_DEP.GROUP_ID 
                           WHERE TB_EB_USER.USER_GUID = @UserID and TB_EB_GROUP.GROUP_ID=@groupID ";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@UserID", UserID);
            this.m_db.AddParameter("@groupID", groupID);
            dt.Load(this.m_db.ExecuteReader(cmd));
            this.m_db.Dispose();

            string LEV = "";
            if (dt.Rows.Count > 0)
            {
                LEV = dt.Rows[0][0].ToString(); //請假人工號
            }

            return LEV;

        }

        internal string GetMaPhieu(string Type)
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmd = "";
            if (Type == "1")
            {
                cmd = @"DECLARE @NO VARCHAR(50) = ''

                          SELECT @NO=CONVERT(VARCHAR,YEAR(GETDATE()))+
                                     RIGHT('0' + CONVERT(VARCHAR,MONTH(GETDATE())), 2)+
                                     RIGHT('0' + CONVERT(VARCHAR,DAY(GETDATE())), 2)

                          SELECT CASE 
                                    WHEN MAX(MaPhieu) IS NOT NULL THEN 
                                    'V' + CONVERT(VARCHAR, CONVERT(BIGINT, SUBSTRING(MAX(MaPhieu), 2, LEN(MAX(MaPhieu)))) + 1) 
                                    ELSE 
                                    'V' + @NO + '001' 
                                 END AS MaPhieu 
                          FROM LYN_BusinessTrip
                          WHERE MaPhieu LIKE 'V' + @NO + '%'";
            }
            else
            {
                cmd = @"DECLARE @NO VARCHAR(50) = ''

                          SELECT @NO=CONVERT(VARCHAR,YEAR(GETDATE()))+
                                     RIGHT('0' + CONVERT(VARCHAR,MONTH(GETDATE())), 2)+
                                     RIGHT('0' + CONVERT(VARCHAR,DAY(GETDATE())), 2)

                          SELECT CASE 
                                    WHEN MAX(MaPhieu) IS NOT NULL THEN 
                                    'F' + CONVERT(VARCHAR, CONVERT(BIGINT, SUBSTRING(MAX(MaPhieu), 2, LEN(MAX(MaPhieu)))) + 1) 
                                    ELSE 
                                    'F' + @NO + '001' 
                                 END AS MaPhieu 
                          FROM LYN_BusinessTrip
                          WHERE MaPhieu LIKE 'F' + @NO + '%'";
            }

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(cmd));
            this.m_db.Dispose();

            string MaPhieu = "";
            if (dt.Rows.Count > 0)
            {
                MaPhieu = dt.Rows[0][0].ToString(); //請假人工號
            }

            return MaPhieu;
        }
        internal DataTable GetDep()
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectHRM);
            string cmdTxt = @"SELECT ST_NHANVIEN.DV_MA,DV_TEN
                            FROM ST_DONVI left JOIN dbo.ST_NHANVIEN ON ST_NHANVIEN.DV_MA = ST_DONVI.DV_MA";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal string GetEmployee(string UserID)
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectHRM);

            DataTable dt = new DataTable();
            string Department = "";
            string USERNAME = "";
            string expert = "";

            string cmdTxt = @"SELECT ST_NHANVIEN.NV_Ma, ST_NHANVIEN.NV_Ten, ST_NHANVIEN.DV_MA_, QT_MA,ISNULL(ST_NHANVIENTHOIVIEC.NV_Ma, 'Employee') AS Flag FROM ST_NHANVIEN
                              LEFT JOIN ST_NHANVIENTHOIVIEC ON ST_NHANVIENTHOIVIEC.NV_Ma = ST_NHANVIEN.NV_Ma 
                              LEFT JOIN ST_DONVI ON ST_DONVI.DV_MA = ST_NHANVIEN.DV_MA 
                              WHERE ST_NHANVIEN.NV_Ma = @UserID";

            this.m_db.AddParameter("@UserID", UserID);

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][3].ToString() == "VIE")
                {
                    USERNAME = dt.Rows[0][1].ToString();
                    Department = dt.Rows[0][2].ToString();
                    expert = "N";
                }
                else
                {
                    this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connectHRM);
                    DataTable dt1 = new DataTable();

                    string selectSql = @"
                        SELECT ST_NHANVIEN.NV_Ma, 
                               ST_NHANVIEN.NV_Ten, 
                               ST_NHANVIEN.DV_MA_, 
                               ISNULL(ST_NHANVIENTHOIVIEC.NV_Ma, 'Employee') AS Flag, 
                               ST_DONVI.KHU 
                        FROM ST_NHANVIEN 
                        LEFT JOIN ST_NHANVIENTHOIVIEC ON ST_NHANVIENTHOIVIEC.NV_Ma = ST_NHANVIEN.NV_Ma 
                        LEFT JOIN ST_DONVI ON ST_DONVI.DV_MA = ST_NHANVIEN.DV_MA 
                        WHERE ST_NHANVIEN.NV_Ma =  '" + UserID + "'";

                    dt1.Load(this.m_db.ExecuteReader(selectSql));
                    this.m_db.Dispose();

                    if (dt1.Rows.Count > 0)
                    {
                        USERNAME = dt1.Rows[0][1].ToString();
                        Department = dt1.Rows[0][2].ToString();
                        expert = "Y";
                    }
                }
            }

            string result = USERNAME + ";" + Department + ";" + expert;

            return result;
        }
        internal void InsertBusinessTripFormData(string LYV, string Area, string MaPhieu,string EmployeeType, string DepID, string UserID, XElement xE)
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string Expert = xE.Attribute("Expert").Value;
            string Factory = xE.Attribute("Factory").Value;
            string Work = xE.Attribute("Work").Value;
            string Name_ID = xE.Attribute("Name_ID").Value;
            string Name = xE.Attribute("Name").Value;
            string Name_DepID = xE.Attribute("Name_DepID").Value;
            string Name_DepName = xE.Attribute("Name_DepName").Value;
            string Agent_ID = xE.Attribute("Agent_ID").Value;
            string Agent = xE.Attribute("Agent").Value;
            string Purpose = xE.Attribute("Purpose").Value;
            string FLocation = xE.Attribute("FLocation").Value;
            string Journey = xE.Attribute("Journey").Value;
            string BTime = xE.Attribute("BTime").Value;
            string ETime = xE.Attribute("ETime").Value;
            string Days = xE.Attribute("Days").Value;
            string TransportType = xE.Attribute("TransportType").Value;
            string ApplyCar = xE.Attribute("ApplyCar").Value;
            string Remark = xE.Attribute("Remark").Value;
            string Departurejourney = xE.Attribute("Departurejourney").Value;
            string Journeyends = xE.Attribute("Journeyends").Value;

            string cmdTxt = @"  INSERT INTO LYV_BusinessTrip
                                (	    [LYV_TDC],
                                        [Area],
                                        [MaPhieu],
                                        [Expert],
                                        [Factory],
                                        [EmployeeType],
                                        [Work]
                                       ,[Name_ID]
                                       ,[Name]
                                       ,[Name_DepID]
                                       ,[Name_DepName]
                                       ,[Agent_ID]
                                       ,[Agent]
                                       ,[Purpose]
                                       ,[FLocation]
                                       ,[Journey]
                                       ,[BTime]
                                       ,[ETime]
                                       ,[Days]
                                       ,[TransportType]
                                       ,[ApplyCar]
                                       ,[Remark]
                                       ,[Departurejourney]
                                       ,[Journeyends]
                                       ,[flowflag]
                                       ,[USERID]
                                       ,[DepID]
                                       ,[USERDATE]) 
                                 VALUES 
                                 (	
                                     @LYV,
                                     @Area,
                                     @MaPhieu,
                                     @Expert,
                                     @Factory,
                                     @EmployeeType,
                                     @Work,
                                     @Name_ID,
                                     @Name,
                                     @Name_DepID,
                                     @Name_DepName,
                                     @Agent_ID,
                                     @Agent,
                                     @Purpose,
                                     @FLocation,
                                     @Journey,
                                     @BTime,
                                     " + (string.IsNullOrEmpty(ETime) ? "NULL" : "@ETime") + @",
                                     " + (string.IsNullOrEmpty(Days) ? "NULL" : "@Days") + @", 
                                     " + (string.IsNullOrEmpty(TransportType) ? "NULL" : "@TransportType") + @", 
                                     @ApplyCar,
                                     @Remark,
                                     @Departurejourney,
                                     @Journeyends,
                                     @flowflag,
                                     @USERID,
                                     @DepID,
                                     getdate()
                                )";

            this.m_db.AddParameter("@LYV", LYV);
            this.m_db.AddParameter("@Area", Area);
            this.m_db.AddParameter("@MaPhieu", MaPhieu);
            this.m_db.AddParameter("@Expert", Expert);
            this.m_db.AddParameter("@Factory", Factory);
            this.m_db.AddParameter("@EmployeeType", EmployeeType);
            this.m_db.AddParameter("@Work", Work);
            this.m_db.AddParameter("@Name_ID", Name_ID);
            this.m_db.AddParameter("@Name", Name);
            this.m_db.AddParameter("@Name_DepID", Name_DepID);
            this.m_db.AddParameter("@Name_DepName", Name_DepName);
            this.m_db.AddParameter("@Agent_ID", Agent_ID);
            this.m_db.AddParameter("@Agent", Agent);
            this.m_db.AddParameter("@Purpose", Purpose);
            this.m_db.AddParameter("@FLocation", FLocation);
            this.m_db.AddParameter("@Journey", Journey);
            this.m_db.AddParameter("@BTime", BTime);
            this.m_db.AddParameter("@ETime", ETime);
            this.m_db.AddParameter("@Days", Days);
            this.m_db.AddParameter("@TransportType", TransportType);
            this.m_db.AddParameter("@ApplyCar", ApplyCar);
            this.m_db.AddParameter("@Remark", Remark);
            this.m_db.AddParameter("@Departurejourney", Departurejourney);
            this.m_db.AddParameter("@Journeyends", Journeyends);
            this.m_db.AddParameter("@flowflag", "N");
            this.m_db.AddParameter("@USERID", UserID);
            this.m_db.AddParameter("@DepID", DepID);
            this.m_db.ExecuteNonQuery(cmdTxt);

            this.m_db.Dispose();
        }
        internal void UpdateFormStatus(string LYV, string Area, string MaPhieu,string EmployeeType, string SiteCode, string signStatus, XElement xE)
        {
            DataTable dt = new DataTable();

            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string cmdflowflag = @"SELECT flowflag FROM LYV_BusinessTrip WHERE LYV_TDC = @LYV";

            this.m_db.AddParameter("@LYV", LYV);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            string flowflag = dt.Rows[0][0].ToString(); //請假人工號

            if ((flowflag == "NP" || flowflag == "N") && SiteCode != "ReturnToApplicant")
            {
                string Expert = xE.Attribute("Expert").Value;
                string Factory = xE.Attribute("Factory").Value;
                string Work = xE.Attribute("Work").Value;
                string Name_ID = xE.Attribute("Name_ID").Value;
                string Name = xE.Attribute("Name").Value;
                string Name_DepID = xE.Attribute("Name_DepID").Value;
                string Name_DepName = xE.Attribute("Name_DepName").Value;
                string Agent_ID = xE.Attribute("Agent_ID").Value;
                string Agent = xE.Attribute("Agent").Value;
                string Purpose = xE.Attribute("Purpose").Value;
                string FLocation = xE.Attribute("FLocation").Value;
                string Journey = xE.Attribute("Journey").Value;
                string BTime = xE.Attribute("BTime").Value;
                string ETime = xE.Attribute("ETime").Value;
                string Days = xE.Attribute("Days").Value;
                string TransportType = xE.Attribute("TransportType").Value;
                string ApplyCar = xE.Attribute("ApplyCar").Value;
                string Remark = xE.Attribute("Remark").Value;
                string Departurejourney = xE.Attribute("Departurejourney").Value;
                string Journeyends = xE.Attribute("Journeyends").Value;

                string cmdTxt = @"  UPDATE LYV_BusinessTrip SET
                                     [Area] = @Area,
                                     [MaPhieu] = @MaPhieu,
                                     [Expert] = @Expert,
                                     [Factory] = @Factory,
                                     [EmployeeType] = @EmployeeType,
                                     [Work] = @Work,
                                     [Name_ID] = @Name_ID,
                                     [Name] = @Name,
                                     [Name_DepID] = @Name_DepID,
                                     [Name_DepName] = @Name_DepName,
                                     [Agent_ID] = @Agent_ID,
                                     [Purpose] = @Purpose,
                                     [FLocation] = @FLocation,
                                     [Journey] = @Journey,
                                     [BTime] = @BTime,
                                     [ETime] = " + (string.IsNullOrEmpty(ETime) ? "NULL" : "@ETime") + @",
                                     [Days] = " + (string.IsNullOrEmpty(Days) ? "NULL" : "@Days") + @",
                                     [TransportType] = " + (string.IsNullOrEmpty(TransportType) ? "NULL" : "@TransportType") + @",
                                     [ApplyCar] = @ApplyCar,
                                     [Remark] = @Remark,
                                     [Departurejourney] = @Departurejourney,
                                     [Journeyends] = @Journeyends,
                                     [flowflag] = @flowflag
                                 WHERE
                                     LYV_TDC=@LYV
                                     ";

                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.AddParameter("@Area", Area);
                this.m_db.AddParameter("@MaPhieu", MaPhieu);
                this.m_db.AddParameter("@Expert", Expert);
                this.m_db.AddParameter("@Factory", Factory);
                this.m_db.AddParameter("@EmployeeType", EmployeeType);
                this.m_db.AddParameter("@Work", Work);
                this.m_db.AddParameter("@Name_ID", Name_ID);
                this.m_db.AddParameter("@Name", Name);
                this.m_db.AddParameter("@Name_DepID", Name_DepID);
                this.m_db.AddParameter("@Name_DepName", Name_DepName);
                this.m_db.AddParameter("@Agent_ID", Agent_ID);
                this.m_db.AddParameter("@Agent", Agent);
                this.m_db.AddParameter("@Purpose", Purpose);
                this.m_db.AddParameter("@FLocation", FLocation);
                this.m_db.AddParameter("@Journey", Journey);
                this.m_db.AddParameter("@BTime", BTime);
                this.m_db.AddParameter("@ETime", ETime);
                this.m_db.AddParameter("@Days", Days);
                this.m_db.AddParameter("@TransportType", TransportType);
                this.m_db.AddParameter("@ApplyCar", ApplyCar);
                this.m_db.AddParameter("@Remark", Remark);
                this.m_db.AddParameter("@Departurejourney", Departurejourney);
                this.m_db.AddParameter("@Journeyends", Journeyends);
                this.m_db.AddParameter("@flowflag", "N");

                this.m_db.ExecuteNonQuery(cmdTxt);

                if (!string.IsNullOrEmpty(SiteCode))
                {
                    string cmdTxt1 = "UPDATE LYV_BusinessTrip SET flowflag = 'P' WHERE LYV_TDC = @LYV ";
                    this.m_db.AddParameter("@LYV", LYV);
                    this.m_db.ExecuteNonQuery(cmdTxt1);
                }
            }
            if (SiteCode == "ReturnToApplicant")
            {
                string cmdTxt = "UPDATE LYV_BusinessTrip SET flowflag = 'NP' WHERE LYV_TDC = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (SiteCode == "GD" && signStatus == "Approve" || SiteCode == "CNBP" && signStatus == "Approve")
            {
                string cmdTxt = @"UPDATE LYV_BusinessTrip SET flowflag='Z' WHERE LYV_TDC = @LYV AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (signStatus == "Disapprove")
            {
                string cmdTxt = @"UPDATE LYV_BusinessTrip SET flowflag='X' WHERE LYV_TDC = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }
        internal void UpdateFormResult(string LYV, string formResult)
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            if (formResult == "Adopt")
            {
                string cmdTxt = @"UPDATE LYV_BusinessTrip SET flowflag='Z' WHERE LYV_TDC = @LYV AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (formResult == "Reject" || formResult == "Cancel")
            {
                string cmdTxt = @"UPDATE LYV_BusinessTrip SET flowflag='X' WHERE LYV_TDC = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }
        internal DataTable getWSSignNextInfo(string docNbr, string UserGUID)
        {
            DataTable dt = new DataTable();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT TB_WKF_TASK.TASK_ID, TB_WKF_TASK_NODE.SITE_ID, TB_WKF_TASK_NODE.NODE_SEQ, TB_WKF_TASK_NODE.ORIGINAL_SIGNER
                            FROM TB_WKF_TASK INNER JOIN TB_WKF_TASK_NODE ON TB_WKF_TASK.TASK_ID = TB_WKF_TASK_NODE.TASK_ID
                            WHERE DOC_NBR=@DOC_NBR AND TB_WKF_TASK_NODE.ORIGINAL_SIGNER=@ORIGINAL_SIGNER AND TB_WKF_TASK_NODE.FINISH_TIME IS NULL";
            m_db.AddParameter("@DOC_NBR", docNbr);
            m_db.AddParameter("@ORIGINAL_SIGNER", UserGUID);

            dt.Load(m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal DataTable GetListBT(string LYV, string Type, string RLYV, string Name, string Name_ID, string BTime1, string BTime2)
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string where = "";
            if (Type != "ALL")
            {
                where += " and Work = N'" + Type + "' ";
            }
            if (LYV != "") where += " and LOWER(LYV_TDC) like LOWER('%" + LYV + "%') ";
            if (RLYV != "") where += " and LOWER(RLYV) like LOWER('%" + RLYV + "%') ";
            if (Name != "") where += " and LOWER(Name) like LOWER(N'%" + Name + "%') ";
            if (Name_ID != "") where += " and Name_ID like '" + Name_ID + "%' ";
            if (BTime1 != "") where += " and BTime >= '" + BTime1 + "' ";
            if (BTime2 != "") where += " and BTime <= '" + BTime2 + "' ";

            string SQL1 = @" SELECT * FROM( 
                        SELECT LYV_BusinessTrip.LYV_TDC, LYV_BusinessTripReport.LYV RLYV, LYV_BusinessTrip.Name, Name_ID, Purpose, FLocation, 
                        CONVERT(varchar,BTime,120) BTime, CONVERT(varchar,ETime,120) ETime, LYV_BusinessTrip.USERID, CONVERT(varchar,LYV_BusinessTrip.USERDATE,120) USERDATE, LYV_BusinessTrip.flowflag, 
                        case when Type=N'Trong nước' THEN 'V' else 'F' end as Type, isnull(Days,2) Days, TB_WKF_TASK.TASK_ID, TB_WKF_TASK_Report.TASK_ID RTASK_ID 
                        FROM LYV_BusinessTrip 
                        LEFT JOIN TB_WKF_TASK on LYV_BusinessTrip.LYV_TDC=TB_WKF_TASK.DOC_NBR 
                        LEFT JOIN LYV_BusinessTripReport on LYV_BusinessTrip.LYV_TDC=LYV_BusinessTripReport.RLYV 
                        LEFT JOIN TB_WKF_TASK TB_WKF_TASK_Report on LYV_BusinessTripReport.LYV=TB_WKF_TASK_Report.DOC_NBR 
                        where isnull(LYV_BusinessTripReport.Cancel,0) <> 1
                        /*
                        union all 
                        SELECT LYV, '' RLYV, Name, Name_ID, Purpose, FLocation, 
                        CONVERT(varchar,CAST(CONVERT(varchar, Time, 23) + ' ' + isnull(STime,'00:00') AS smalldatetime),120)  AS BTime, 
                        CONVERT(varchar,CAST(CONVERT(varchar, Time, 23) + ' ' + isnull(ETime,'00:00') AS smalldatetime),120) AS ETime, 
                        USERID, CONVERT(varchar,USERDATE,120) USERDATE, flowflag, 'O' Type, Days, TASK_ID, '' RTASK_ID 
                        FROM LYV_BusinessTripOD 
                        LEFT JOIN TB_WKF_TASK on LYV_BusinessTripOD.LYV_TSS=TB_WKF_TASK.DOC_NBR 
                        */
                    )AS BT 
                    WHERE 1=1" + where + @"
                    ORDER BY BT.LYV desc ";

            string SQL = @" SELECT * FROM( 
                        SELECT LYV_BusinessTrip.LYV_TDC, LYV_BusinessTrip.Name, Name_ID, Purpose, FLocation,TB_WKF_TASK.TASK_ID,
                        CONVERT(varchar,BTime,120) BTime, CONVERT(varchar,ETime,120) ETime, LYV_BusinessTrip.USERID, CONVERT(varchar,LYV_BusinessTrip.USERDATE,120) USERDATE, LYV_BusinessTrip.flowflag, 
                        Work, isnull(Days,2) Days
                        FROM LYV_BusinessTrip 
                        LEFT JOIN TB_WKF_TASK on LYV_BusinessTrip.LYV_TDC=TB_WKF_TASK.DOC_NBR
                        LEFT JOIN TB_WKF_TASK TB_WKF_TASK_Report on LYV_BusinessTrip.LYV_TDC=TB_WKF_TASK_Report.DOC_NBR
                    )AS BT 
                    WHERE 1=1" + where + @"
                    ORDER BY BT.LYV_TDC desc ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));

            this.m_db.Dispose();

            return dt;
        }

        internal string GetType(string LYV)
        {
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string SQL = @"SELECT Work FROM dbo.LYV_BusinessTrip WHERE LYV_TDC = @LYV";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LYV", LYV);
            dt.Load(this.m_db.ExecuteReader(SQL));
            this.m_db.Dispose();

            string Type = "";
            if (dt.Rows.Count > 0)
            {
                Type = dt.Rows[0][0].ToString(); //請假人工號
            }
            return Type;
          
        }
        internal string getFlowflag(string LYV)
        {

            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdflowflag = @"SELECT flowflag FROM dbo.LYV_BusinessTrip WHERE LYV_TDC = @LYV";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LYV", LYV);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            this.m_db.Dispose();

            string flowflag = dt.Rows[0][0].ToString(); //請假人工號

            return flowflag;

        }

    }
}
