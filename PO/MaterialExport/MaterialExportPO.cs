using System.Data;
using System.Xml.Linq;

namespace LYV.MaterialExport.PO
{
    internal class MaterialExportPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
   
        internal DataTable GetKCLL_X(string UserID, string LLNO, string DepID)
        {
            string ID= UserID.Replace("LYV","");
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db=new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string sql = @"SELECT KCLL.*, Bdepartment.DepName 
		                        FROM KCLL
		                        LEFT JOIN Bdepartment on Bdepartment.ID = KCLL.DepID
		                        WHERE KCLL.SCBH = 'XXXXXXXXXX'
		                        AND KCLL.USERID = @ID
		                        AND ISNULL(KCLL.flowflag, '') = '' 
		                        AND KCLL.CFMID = 'NO'
		                        AND (ISNULL(KCLL.LLNO, '') = '' OR KCLL.LLNO LIKE '%' + @LLNO + '%')
		                        AND (ISNULL(KCLL.DepID, '') = '' OR KCLL.DepID LIKE '%' + @DepID + '%')
		                        ORDER BY LLNO DESC";

            this.m_db.AddParameter("@ID", ID);
            this.m_db.AddParameter("@LLNO", LLNO);
            this.m_db.AddParameter("@DepID", DepID);

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(sql));

            this.m_db.Dispose();

            return dt;
        }
        internal DataTable GetKCLL_X_LLNO(string LLNO)
        {
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT KCLL.*, Bdepartment.DepName FROM KCLL
                              LEFT JOIN Bdepartment on Bdepartment.ID = KCLL.DepID
                              WHERE KCLL.SCBH = 'XXXXXXXXXX' and KCLL.LLNO in (" + LLNO + ") ORDER BY LLNO DESC ";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        public void UpdateFormStatus(XElement xE, string status, string signStatus, string FNO, string UserID,string DepID, string type)
        {
            string key = xE.Attribute("LLNO").Value;
            string LLNO = "''";
            if (key != "")
            {
                string[] dsLNO = key.Split(',');
                if (dsLNO.Length > 0)
                {
                    for (int i = 0; i < dsLNO.Length; i++)
                    {
                        LLNO += ", '" + dsLNO[i] + "'";
                    }
                }
            }
            if (status == "N")
            {
                string conn = Training.Properties.Settings.Default.UOF.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

                string cmdTxt1 = @"  INSERT INTO LYV_MaterialExport
                                (	 [FNO] ,  
                                     [TNO] ,  
                                     [TableName] , 
                                     [Type] ,
                                     [UserID] ,
                                     [DepID] ,             
                                     [UserDate] 
                                ) 
                                 VALUES 
                                 (	
                                     @FNO,
                                     @TNO,
                                     @TableName,
                                     @Type,
                                     @USERID,
                                     @DepID,
                                     getdate()
                                )";

                this.m_db.AddParameter("@FNO", FNO);
                this.m_db.AddParameter("@TNO", key);
                this.m_db.AddParameter("@TableName", "KCLL");
                this.m_db.AddParameter("@Type", type);
                this.m_db.AddParameter("@USERID", UserID);
                this.m_db.AddParameter("@DepID", DepID);

                this.m_db.ExecuteNonQuery(cmdTxt1);

                string conn1 = Training.Properties.Settings.Default.LYV_ERP.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = @"UPDATE KCLL SET flowflag='N' WHERE LLNO in ("+ LLNO + ") AND LLNO <> '' ";
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if ((status == "Adopt") || (status == "SE" && signStatus == "Approve"))
            {
                
                string conn1 = Training.Properties.Settings.Default.LYV_ERP.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = @"UPDATE KCLL SET flowflag='Z' WHERE LLNO in (" + LLNO + ") AND LLNO <> '' AND flowflag IN ('N','P')";
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if ((status == "Reject" || status == "Cancel") || (signStatus == "Disapprove"))
            {
                string conn1 = Training.Properties.Settings.Default.LYV_ERP.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = @"UPDATE KCLL SET flowflag='X' WHERE LLNO in (" + LLNO + ") AND LLNO <> '' ";
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else
            {
                string conn1 = Training.Properties.Settings.Default.LYV_ERP.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = @"UPDATE KCLL SET flowflag='P' WHERE LLNO in (" + LLNO + ") AND LLNO <> '' AND flowflag IN ('N','P')";
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }
        internal DataTable GetKCLL(string UserID, string LLNO, string DepID)
        {
            string ID = UserID.Replace("LYN", "");
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT KCLL.LLNO,KCLL.GSBH,KCLL.CKBH,KCLL.SCBH,KCLL.DepID,Bdepartment.depname,KCLL.USERID,KCLL.USERDATE,KCLL.CFMID,KCLL.CFMDate,KCLL.YN,KCLL.JGNO, 
                                     KCLL.SEASON,KCLL.PURPOSE,KCLL.DEVTYPE,KCLL.MEMO,KCLL.flowflag 
                              FROM KCLL
                              LEFT JOIN Bdepartment on Bdepartment.ID = KCLL.DepID
                              WHERE KCLL.SCBH <> 'XXXXXXXXXX' and KCLL.USERID = '" + ID + "' AND ISNULL(KCLL.flowflag,'')='' AND KCLL.USERDATE>GetDate()-60 " +
                              "     and KCLL.LLNO like '" + LLNO + "%' and KCLL.DepID like '%" + DepID + "%' ORDER BY LLNO DESC ";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal DataTable GetKCLL_LLNO(string LLNO)
        {
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT KCLL.LLNO,KCLL.GSBH,KCLL.CKBH,KCLL.SCBH,KCLL.DepID,Bdepartment.depname,KCLL.USERID,KCLL.USERDATE,KCLL.CFMID,KCLL.CFMDate,KCLL.YN,KCLL.JGNO, 
                                     KCLL.SEASON,KCLL.PURPOSE,KCLL.DEVTYPE,KCLL.MEMO,KCLL.flowflag 
                              FROM KCLL
                              LEFT JOIN Bdepartment on Bdepartment.ID = KCLL.DepID
                              WHERE KCLL.SCBH <> 'XXXXXXXXXX' and KCLL.LLNO in (" + LLNO + ") ORDER BY LLNO DESC ";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
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

        internal DataTable GetListME(string LNO, string LLNO, string Table_Name, string Type)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string where = "";

            if (LNO != "") where += " and LOWER(FNO) like LOWER('%" + LNO + "%') ";
            if (LLNO != "") where += " and LOWER(TNO) like LOWER(N'%" + LLNO + "%') ";
            if (Table_Name != "") where += " and LOWER(TableName) like LOWER(N'%" + Table_Name + "%') ";
            if (Type != "") where += " and LOWER(Type) like LOWER(N'%" + Type + "%') ";

            string SQL = @"SELECT LYV_MaterialExport.*, TASK_ID 
                           FROM LYV_MaterialExport LEFT JOIN TB_WKF_TASK on LYV_MaterialExport.FNO=TB_WKF_TASK.DOC_NBR 
                           WHERE 1=1" + where + @"
                           ORDER BY LYV_MaterialExport.FNO desc ";

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));

            this.m_db.Dispose();

            return dt;
        }

    }
}
