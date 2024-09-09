using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Data;
using System.Xml.Linq;

namespace Training.BusinessTripExpert.PO
{
    internal class BusinessTripExpertPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal string GetLEV(string UserID, string groupID)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
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
        internal DataTable GetDep()
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT ST_NHANVIEN.DV_MA,DV_TEN
                            FROM ST_DONVI left JOIN dbo.ST_NHANVIEN ON ST_NHANVIEN.DV_MA = ST_DONVI.DV_MA";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal string GetEmployee(string UserID)
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

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
                    string conn1 = Training.Properties.Settings.Default.LYV_ERP.ToString();
                    this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                    DataTable dt1 = new DataTable();

                    string selectSql = "";
                    selectSql += "Select Certificate.ID, Certificate.Name,Directory.DID, Directory_Department.Name as DepName, Resigned from [LIY_TYXUAN].[dbo].[Certificate] Certificate";
                    selectSql += "	left join [LIY_TYXUAN].[dbo].[Directory] Directory on Directory.ID = Certificate.ID ";
                    selectSql += "  left join [LIY_TYXUAN].[dbo].[Directory_Department] Directory_Department on Directory_Department.DID = Directory.DID ";
                    selectSql += " where Certificate.ID = '" + UserID + "'";

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
        internal void InsertBusinessTripExpertFormData(string LNO, string UserID, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string expert = xE.Attribute("expert").Value;
            string Factory = xE.Attribute("Factory").Value;
            string Type = xE.Attribute("Type").Value;
            string Name_ID = xE.Attribute("Name_ID").Value;
            string Name = xE.Attribute("Name").Value;
            string Name_DepID = xE.Attribute("Name_DepID").Value;
            string Name_DepName = xE.Attribute("Name_DepName").Value;
            string Agent_ID = xE.Attribute("Agent_ID").Value;
            string Agent = xE.Attribute("Agent").Value;
            string Purpose = xE.Attribute("Purpose").Value;
            string FLocation = xE.Attribute("FLocation").Value;
            string Journey = xE.Attribute("Journey").Value;
            string Time = xE.Attribute("Time").Value;
            string Days = xE.Attribute("Days").Value;
            string TransportType = xE.Attribute("TransportType").Value;
            string ApplyCar = xE.Attribute("ApplyCar").Value;
            string Remark = xE.Attribute("Remark").Value;

            string cmdTxt = @"  INSERT INTO LYN_BusinessTripExpert
                                (	 [LNO] ,  
                                     [expert] , 
                                     [Factory] ,
                                     [Type] ,
                                     [Name_ID] ,
                                     [Name] ,
                                     [Name_DepID] ,
                                     [Name_DepName] ,
                                     [Agent_ID] ,
                                     [Agent] ,
                                     [Purpose] ,
                                     [FLocation] ,
                                     [Journey] ,
                                     [Time] ,
                                     [Days] ,
                                     [TransportType] ,
                                     [ApplyCar] ,
                                     [Remark] ,
                                     [flowflag] ,  
                                     [USERID] ,             
                                     [USERDATE] 
                                ) 
                                 VALUES 
                                 (	
                                     @LNO,
                                     @expert,
                                     @Factory,
                                     @Type,
                                     @Name_ID,
                                     @Name,
                                     @Name_DepID,
                                     @Name_DepName,
                                     @Agent_ID,
                                     @Agent,
                                     @Purpose,
                                     @FLocation,
                                     @Journey,
                                     @Time,
                                     @Days, 
                                     " + (string.IsNullOrEmpty(TransportType) ? "NULL" : "@TransportType") + @", 
                                     @ApplyCar,
                                     @Remark,
                                     @flowflag,
                                     @USERID,
                                     getdate()
                                )";

            this.m_db.AddParameter("@LNO", LNO);
            this.m_db.AddParameter("@expert", expert);
            this.m_db.AddParameter("@Factory", Factory);
            this.m_db.AddParameter("@Type", Type);
            this.m_db.AddParameter("@Name_ID", Name_ID);
            this.m_db.AddParameter("@Name", Name);
            this.m_db.AddParameter("@Name_DepID", Name_DepID);
            this.m_db.AddParameter("@Name_DepName", Name_DepName);
            this.m_db.AddParameter("@Agent_ID", Agent_ID);
            this.m_db.AddParameter("@Agent", Agent);
            this.m_db.AddParameter("@Purpose", Purpose);
            this.m_db.AddParameter("@FLocation", FLocation);
            this.m_db.AddParameter("@Journey", Journey);
            this.m_db.AddParameter("@Time", Time);
            this.m_db.AddParameter("@Days", Days);
            this.m_db.AddParameter("@TransportType", TransportType);
            this.m_db.AddParameter("@ApplyCar", ApplyCar);
            this.m_db.AddParameter("@Remark", Remark);
            this.m_db.AddParameter("@flowflag", "N");
            this.m_db.AddParameter("@USERID", UserID);

            this.m_db.ExecuteNonQuery(cmdTxt);

            this.m_db.Dispose();
        }
        internal void UpdateFormStatus(string LNO, string SiteCode, string signStatus, string MaPhieu, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string cmdflowflag = @"SELECT flowflag FROM LYN_BusinessTripExpert WHERE LNO = @LNO";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LNO", LNO);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            string flowflag = dt.Rows[0][0].ToString(); //請假人工號

            if (flowflag == "NP" || flowflag == "N")
            {
                string expert = xE.Attribute("expert").Value;
                string Factory = xE.Attribute("Factory").Value;
                string Type = xE.Attribute("Type").Value;
                string Name_ID = xE.Attribute("Name_ID").Value;
                string Name = xE.Attribute("Name").Value;
                string Name_DepID = xE.Attribute("Name_DepID").Value;
                string Name_DepName = xE.Attribute("Name_DepName").Value;
                string Agent_ID = xE.Attribute("Agent_ID").Value;
                string Agent = xE.Attribute("Agent").Value;
                string Purpose = xE.Attribute("Purpose").Value;
                string FLocation = xE.Attribute("FLocation").Value;
                string Journey = xE.Attribute("Journey").Value;
                string Time = xE.Attribute("Time").Value;
                string Days = xE.Attribute("Days").Value;
                string TransportType = xE.Attribute("TransportType").Value;
                string ApplyCar = xE.Attribute("ApplyCar").Value;
                string Remark = xE.Attribute("Remark").Value;

                string cmdTxt = @"  UPDATE LYN_BusinessTripExpert SET
                                     [expert] = @expert,
                                     [Factory] = @Factory,
                                     [Type] = @Type,
                                     [Name_ID] = @Name_ID,
                                     [Name] = @Name,
                                     [Name_DepID] = @Name_DepID,
                                     [Name_DepName] = @Name_DepName,
                                     [Agent_ID] = @Agent_ID,
                                     [Purpose] = @Purpose,
                                     [FLocation] = @FLocation,
                                     [Journey] = @Journey,
                                     [Time] = @Time,
                                     [Days] = @Days,
                                     [TransportType] = " + (string.IsNullOrEmpty(TransportType) ? "NULL" : "@TransportType") + @",
                                     [ApplyCar] = @ApplyCar,
                                     [Remark] = @Remark,
                                     [flowflag] = @flowflag
                                 WHERE
                                     LNO=@LNO
                                     ";

                this.m_db.AddParameter("@LNO", LNO);
                //this.m_db.AddParameter("@MaPhieu", MaPhieu);
                this.m_db.AddParameter("@expert", expert);
                this.m_db.AddParameter("@Factory", Factory);
                this.m_db.AddParameter("@Type", Type);
                this.m_db.AddParameter("@Name_ID", Name_ID);
                this.m_db.AddParameter("@Name", Name);
                this.m_db.AddParameter("@Name_DepID", Name_DepID);
                this.m_db.AddParameter("@Name_DepName", Name_DepName);
                this.m_db.AddParameter("@Agent_ID", Agent_ID);
                this.m_db.AddParameter("@Agent", Agent);
                this.m_db.AddParameter("@Purpose", Purpose);
                this.m_db.AddParameter("@FLocation", FLocation);
                this.m_db.AddParameter("@Journey", Journey);
                this.m_db.AddParameter("@Time", Time);
                this.m_db.AddParameter("@Days", Days);
                this.m_db.AddParameter("@TransportType", TransportType);
                this.m_db.AddParameter("@ApplyCar", ApplyCar);
                this.m_db.AddParameter("@Remark", Remark);
                this.m_db.AddParameter("@flowflag", "P");

                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (flowflag == "P")
            {
                string cmdTxt = @"  UPDATE LYN_BusinessTripExpert SET
                                     [MaPhieu] = @MaPhieu
                                 WHERE
                                     LNO=@LNO
                                     ";

                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.AddParameter("@MaPhieu", MaPhieu);

                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            if (SiteCode == "ReturnToApplicant")
            {
                string cmdTxt = "UPDATE LYN_BusinessTripExpert SET flowflag = 'NP' WHERE LNO = @LNO ";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (SiteCode == "SE" && signStatus == "Approve")
            {
                string cmdTxt = @"UPDATE LYN_BusinessTripExpert SET flowflag='Z' WHERE LNO = @LNO AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (signStatus == "Disapprove")
            {
                string cmdTxt = @"UPDATE LYN_BusinessTripExpert SET flowflag='X' WHERE LNO = @LNO ";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }
        internal void UpdateFormResult(string LNO, string formResult)
        {
            string conn1 = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);

            if (formResult == "Adopt")
            {
                string cmdTxt = @"UPDATE LYN_BusinessTripExpert SET flowflag='Z' WHERE LNO = @LNO AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (formResult == "Reject" || formResult == "Cancel")
            {
                string cmdTxt = @"UPDATE LYN_BusinessTripExpert SET flowflag='X' WHERE LNO = @LNO ";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }
        internal DataTable getWSSignNextInfo(string docNbr, string UserGUID)
        {
            DataTable dt = new DataTable();
            string conn = Training.Properties.Settings.Default.UOF.ToString();
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
        internal DataTable GetListBT(string LNO, string Name, string Name_ID, string Time1, string Time2, string expert)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string where = " and expert = '" + expert + "' ";
            if (LNO != "") where += " and LOWER(LNO) like LOWER('%" + LNO + "%') ";
            if (Name != "") where += " and LOWER(Name) like LOWER(N'%" + Name + "%') ";
            if (Name_ID != "") where += " and Name_ID like '" + Name_ID + "%' ";
            if (Time1 != "") where += " and Time >= '" + Time1 + "' ";
            if (Time2 != "") where += " and Time <= '" + Time2 + "' ";

            string SQL = "";
            SQL += "SELECT LNO, Name, Name_ID, Purpose, FLocation, Time, USERID, USERDATE, flowflag, TASK_ID ";
            SQL += "FROM LYN_BusinessTripExpert LEFT JOIN TB_WKF_TASK on LYN_BusinessTripExpert.LNO=TB_WKF_TASK.DOC_NBR ";
            SQL += "WHERE 1=1 " + where + "  ";
            SQL += "ORDER BY LYN_BusinessTripExpert.LNO desc ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));

            this.m_db.Dispose();

            return dt;
        }
    }
}
