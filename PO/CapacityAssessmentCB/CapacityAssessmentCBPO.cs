using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Data;
using System.Xml.Linq;
using Quartz.Collection;

namespace Training.CapacityAssessmentCB.PO
{
    internal class CapacityAssessmentCBPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal string GetEmployee(string UserID)
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            DataTable dt = new DataTable();
            string Department = "";
            string USERNAME = "";
            string Flag = "";

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
            }

            string result = USERNAME + ";" + Department + ";" + Flag;

            this.m_db.Dispose();

            return result;
        }
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
        internal void InsertCapacityAssessmentCBData(string CNO, string UserID, string DepID, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            
            string ID = xE.Attribute("ID").Value;
            string Name = xE.Attribute("Name").Value;
            string Dep = xE.Attribute("Dep").Value;

            string GQ1 = xE.Attribute("GQ1").Value;
            string GQ2 = xE.Attribute("GQ2").Value;
            string GQ3 = xE.Attribute("GQ3").Value;
            
            string ST1 = xE.Attribute("ST1").Value;
            string ST2 = xE.Attribute("ST2").Value;
            string ST3 = xE.Attribute("ST3").Value;

            string GT1 = xE.Attribute("GT1").Value;
            string GT2 = xE.Attribute("GT2").Value;
            string GT3 = xE.Attribute("GT3").Value;

            string HT1 = xE.Attribute("HT1").Value;
            string HT2 = xE.Attribute("HT2").Value;
            string HT3 = xE.Attribute("HT3").Value;

            string CV1 = xE.Attribute("CV1").Value;
            string CV2 = xE.Attribute("CV2").Value;
            string CV3 = xE.Attribute("CV3").Value;

            string TN1 = xE.Attribute("TN1").Value;
            string TN2 = xE.Attribute("TN2").Value;
            string TN3 = xE.Attribute("TN3").Value;

            string CP1 = xE.Attribute("CP1").Value;
            string CP2 = xE.Attribute("CP2").Value;
            string CP3 = xE.Attribute("CP3").Value;

            string TC1 = xE.Attribute("TC1").Value;
            string TC2 = xE.Attribute("TC2").Value;
            string TC3 = xE.Attribute("TC3").Value;

            string cmdTxt = @"  INSERT INTO LYN_CapacityAssessment
                                (	 [CNO] ,  
                                     [UserID] , 
                                     [UserDate] ,
                                     [DepID] ,
                                     [flowflag] ,
                                     [ID],
                                     [Name],
                                     [Dep],
                                     [GQ1],
                                     [GQ2],
                                     [GQ3],
                                     [ST1],
                                     [ST2],
                                     [ST3],
                                     [GT1],
                                     [GT2],
                                     [GT3],
                                     [HT1],
                                     [HT2],
                                     [HT3],
                                     [CV1],
                                     [CV2],
                                     [CV3],
                                     [TN1],
                                     [TN2],
                                     [TN3],
                                     [CP1],
                                     [CP2],
                                     [CP3],
                                     [TC1],
                                     [TC2],
                                     [TC3]
                                ) 
                                 VALUES 
                                 (	
                                     @CNO,
                                     @UserID,
                                     getdate(),
                                     @DepID,
                                     @flowflag,
                                     @ID,
                                     @Name,
                                     @Dep,
                                     @GQ1,
                                     @GQ2,
                                     @GQ3,
                                     @ST1,
                                     @ST2,
                                     @ST3,
                                     @GT1,
                                     @GT2,
                                     @GT3,
                                     @HT1,
                                     @HT2,
                                     @HT3,
                                     @CV1,
                                     @CV2,
                                     @CV3,
                                     @TN1,
                                     @TN2,
                                     @TN3,
                                     @CP1,
                                     @CP2,
                                     @CP3,
                                     @TC1,
                                     @TC2,
                                     @TC3
                                )";

            this.m_db.AddParameter("@CNO", CNO);
            this.m_db.AddParameter("@UserID", UserID);
            this.m_db.AddParameter("@DepID", DepID);
            this.m_db.AddParameter("@flowflag", "N");
            this.m_db.AddParameter("@ID", ID);
            this.m_db.AddParameter("@Name", Name);
            this.m_db.AddParameter("@Dep", Dep);
            this.m_db.AddParameter("@GQ1", GQ1);
            this.m_db.AddParameter("@GQ2", GQ2);
            this.m_db.AddParameter("@GQ3", GQ3);
            this.m_db.AddParameter("@ST1", ST1);
            this.m_db.AddParameter("@ST2", ST2);
            this.m_db.AddParameter("@ST3", ST3);
            this.m_db.AddParameter("@GT1", GT1);
            this.m_db.AddParameter("@GT2", GT2);
            this.m_db.AddParameter("@GT3", GT3);
            this.m_db.AddParameter("@HT1", HT1);
            this.m_db.AddParameter("@HT2", HT2);
            this.m_db.AddParameter("@HT3", HT3);
            this.m_db.AddParameter("@CV1", CV1);
            this.m_db.AddParameter("@CV2", CV2);
            this.m_db.AddParameter("@CV3", CV3);
            this.m_db.AddParameter("@TN1", TN1);
            this.m_db.AddParameter("@TN2", TN2);
            this.m_db.AddParameter("@TN3", TN3);
            this.m_db.AddParameter("@CP1", CP1);
            this.m_db.AddParameter("@CP2", CP2);
            this.m_db.AddParameter("@CP3", CP3);
            this.m_db.AddParameter("@TC1", TC1);
            this.m_db.AddParameter("@TC2", TC2);
            this.m_db.AddParameter("@TC3", TC3);

            this.m_db.ExecuteNonQuery(cmdTxt);
            this.m_db.Dispose();
        }
        internal void UpdateFormStatus(string CNO, string UserID, string DepID, string SiteCode, string signStatus, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string cmdflowflag = @"SELECT flowflag FROM LYN_CapacityAssessment WHERE CNO = @CNO";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@CNO", CNO);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            string flowflag = dt.Rows[0][0].ToString(); //請假人工號
            if ((flowflag == "NP" || flowflag == "N") && SiteCode != "ReturnToApplicant")
            {
                string ID = xE.Attribute("ID").Value;
                string Name = xE.Attribute("Name").Value;
                string Dep = xE.Attribute("Dep").Value;

                string GQ1 = xE.Attribute("GQ1").Value;
                string GQ2 = xE.Attribute("GQ2").Value;
                string GQ3 = xE.Attribute("GQ3").Value;

                string ST1 = xE.Attribute("ST1").Value;
                string ST2 = xE.Attribute("ST2").Value;
                string ST3 = xE.Attribute("ST3").Value;

                string GT1 = xE.Attribute("GT1").Value;
                string GT2 = xE.Attribute("GT2").Value;
                string GT3 = xE.Attribute("GT3").Value;

                string HT1 = xE.Attribute("HT1").Value;
                string HT2 = xE.Attribute("HT2").Value;
                string HT3 = xE.Attribute("HT3").Value;

                string CV1 = xE.Attribute("CV1").Value;
                string CV2 = xE.Attribute("CV2").Value;
                string CV3 = xE.Attribute("CV3").Value;

                string TN1 = xE.Attribute("TN1").Value;
                string TN2 = xE.Attribute("TN2").Value;
                string TN3 = xE.Attribute("TN3").Value;

                string CP1 = xE.Attribute("CP1").Value;
                string CP2 = xE.Attribute("CP2").Value;
                string CP3 = xE.Attribute("CP3").Value;

                string TC1 = xE.Attribute("TC1").Value;
                string TC2 = xE.Attribute("TC2").Value;
                string TC3 = xE.Attribute("TC3").Value;

                string cmdTxt = @"  UPDATE LYN_CapacityAssessment SET
                                     [flowflag] = @flowflag,
                                     [ID] = @ID ,
                                     [Name] = @Name ,
                                     [Dep] = @Dep ,
                                     [GQ1] = @GQ1 ,
                                     [GQ2] = @GQ2 ,
                                     [GQ3] = @GQ3 ,
                                     [ST1] = @ST1 ,
                                     [ST2] = @ST2 ,
                                     [ST3] = @ST3 ,
                                     [GT1] = @GT1 ,
                                     [GT2] = @GT2 ,
                                     [GT3] = @GT3 ,
                                     [HT1] = @HT1 ,
                                     [HT2] = @HT2 ,
                                     [HT3] = @HT3 ,
                                     [CV1] = @CV1 ,
                                     [CV2] = @CV2 ,
                                     [CV3] = @CV3 ,
                                     [TN1] = @TN1 ,
                                     [TN2] = @TN2 ,
                                     [TN3] = @TN3 ,
                                     [CP1] = @CP1 ,
                                     [CP2] = @CP2 ,
                                     [CP3] = @CP3 ,
                                     [TC1] = @TC1 ,
                                     [TC2] = @TC2 ,
                                     [TC3] = @TC3
                                    WHERE
                                     CNO=@CNO
                                     ";

                this.m_db.AddParameter("@CNO", CNO);
                this.m_db.AddParameter("@flowflag", "N");
                this.m_db.AddParameter("@ID", ID);
                this.m_db.AddParameter("@Name", Name);
                this.m_db.AddParameter("@Dep", Dep);
                this.m_db.AddParameter("@GQ1", GQ1);
                this.m_db.AddParameter("@GQ2", GQ2);
                this.m_db.AddParameter("@GQ3", GQ3);
                this.m_db.AddParameter("@ST1", ST1);
                this.m_db.AddParameter("@ST2", ST2);
                this.m_db.AddParameter("@ST3", ST3);
                this.m_db.AddParameter("@GT1", GT1);
                this.m_db.AddParameter("@GT2", GT2);
                this.m_db.AddParameter("@GT3", GT3);
                this.m_db.AddParameter("@HT1", HT1);
                this.m_db.AddParameter("@HT2", HT2);
                this.m_db.AddParameter("@HT3", HT3);
                this.m_db.AddParameter("@CV1", CV1);
                this.m_db.AddParameter("@CV2", CV2);
                this.m_db.AddParameter("@CV3", CV3);
                this.m_db.AddParameter("@TN1", TN1);
                this.m_db.AddParameter("@TN2", TN2);
                this.m_db.AddParameter("@TN3", TN3);
                this.m_db.AddParameter("@CP1", CP1);
                this.m_db.AddParameter("@CP2", CP2);
                this.m_db.AddParameter("@CP3", CP3);
                this.m_db.AddParameter("@TC1", TC1);
                this.m_db.AddParameter("@TC2", TC2);
                this.m_db.AddParameter("@TC3", TC3);

                this.m_db.ExecuteNonQuery(cmdTxt);

                if (!string.IsNullOrEmpty(SiteCode))
                {
                    string cmdTxt1 = "UPDATE LYN_CapacityAssessment SET flowflag = 'P' WHERE CNO=@CNO";
                    this.m_db.AddParameter("@CNO", CNO);
                    this.m_db.ExecuteNonQuery(cmdTxt1);
                }
            }

            if (SiteCode == "ReturnToApplicant")
            {
                string cmdTxt = "UPDATE LYN_CapacityAssessment SET flowflag = 'NP' WHERE CNO=@CNO";
                this.m_db.AddParameter("@CNO", CNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (SiteCode == "SE" && signStatus == "Approve")
            {
                string cmdTxt = "UPDATE LYN_CapacityAssessment SET flowflag = 'Z' WHERE CNO=@CNO AND flowflag IN ('N','P') ";
                this.m_db.AddParameter("@CNO", CNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (signStatus == "Disapprove")
            {
                string cmdTxt = "UPDATE LYN_CapacityAssessment SET flowflag = 'X' WHERE CNO=@CNO ";
                this.m_db.AddParameter("@CNO", CNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }

        internal void UpdateFormResult(string CNO, string formResult)
        {
            string conn1 = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
            if (formResult == "Adopt")
            {
                string cmdTxt = "UPDATE LYN_CapacityAssessment SET flowflag = 'Z' WHERE CNO=@CNO AND flowflag IN ('N','P') ";
                this.m_db.AddParameter("@CNO", CNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (formResult == "Reject" || formResult == "Cancel")
            {
                string cmdTxt = "UPDATE LYN_CapacityAssessment SET flowflag = 'X' WHERE CNO=@CNO ";
                this.m_db.AddParameter("@CNO", CNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }
        internal DataTable GetListPR(string Factory, string Department, string Year, string Month1, string Month2, string flowflag, string CFMID)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string where = " AND (LYN_CapacityAssessmentCB_1.CFMID_1='" + CFMID + "' OR LYN_CapacityAssessmentCB_2.CFMID_2='" + CFMID + "') ";

            if (Factory != "") where += " and LOWER(GROUPS.Factory) like LOWER(N'%" + Factory + "%') ";
            if (Department != "") where += " and LOWER(GROUPS.Department) like LOWER(N'%" + Department + "%') ";
            if (Year != "") where += " and LYN_CapacityAssessmentCB.Year = '" + Year + "' ";
            if (Month1 != "" && Month2 != "")
            {
                where += " and LYN_CapacityAssessmentCB.Month >= " + Month1 + " and LYN_CapacityAssessmentCB.Month <= " + Month2;
            }
            else if (Month1 != "" || Month2 != "")
            {
                where += " and LYN_CapacityAssessmentCB.Month = '" + Month1+ Month2 + "' ";
            }
            if (flowflag != "ALL")
            {
                if (flowflag == "Z")
                {
                    where += " and flowflag = 'Z' ";
                }
                else if (flowflag == "X")
                {
                    where += " and flowflag = 'X' ";
                }
                else
                {
                    where += " and flowflag not in ('X','Z') ";
                }
            }

            string SQL = @"  select ROW_NUMBER() OVER(ORDER BY LYN_CapacityAssessmentCB.PNO DESC) AS Seq,  GROUPS.Factory, GROUPS.Department,
                                    GROUPS.Position, LYN_CapacityAssessmentCB.UserID, GROUPS.UserNameTW + ' ' + GROUPS.UserName as UserName, CONVERT(VARCHAR,GROUPS.JoinDate,120) JoinDate,
	                                LYN_CapacityAssessmentCB_1.TMT_1, LYN_CapacityAssessmentCB_1.TNL_1, LYN_CapacityAssessmentCB_1.TPD_1, LYN_CapacityAssessmentCB_1.TKT_1,
	                                (LYN_CapacityAssessmentCB_1.TMT_1 + LYN_CapacityAssessmentCB_1.TNL_1 + LYN_CapacityAssessmentCB_1.TPD_1 + LYN_CapacityAssessmentCB_1.TKT_1) as TD_1,
	                                '(' +LYN_CapacityAssessmentCB_1.CFMID_1 + ') ' + GROUPS1.NAME as CFM_1,
	                                (LYN_CapacityAssessmentCB_2.TMT_2 + LYN_CapacityAssessmentCB_2.TNL_2 + LYN_CapacityAssessmentCB_2.TPD_2 + LYN_CapacityAssessmentCB_2.TKT_2) as TD_2,
	                                '(' +LYN_CapacityAssessmentCB_2.CFMID_2 + ') ' + GROUPS2.NAME as CFM_2, LYN_CapacityAssessmentCB.Month + '年' + LYN_CapacityAssessmentCB.Year + '月份' TimePR, 
                                    LYN_CapacityAssessmentCB.PNO, TB_WKF_TASK.TASK_ID
                            from LYN_CapacityAssessmentCB LEFT JOIN TB_WKF_TASK on LYN_CapacityAssessmentCB.PNO=TB_WKF_TASK.DOC_NBR
                            left join LYN_CapacityAssessmentCB_1 on LYN_CapacityAssessmentCB_1.PNO=LYN_CapacityAssessmentCB.PNO
                            left join LYN_CapacityAssessmentCB_2 on LYN_CapacityAssessmentCB_2.PNO=LYN_CapacityAssessmentCB.PNO
                            left join LYN_USERGROUPS GROUPS on GROUPS.UserID=LYN_CapacityAssessmentCB.UserID
							LEFT JOIN TB_EB_USER GROUPS1 ON SUBSTRING(GROUPS1.ACCOUNT, PATINDEX('%[0-9]%', GROUPS1.ACCOUNT), LEN(GROUPS1.ACCOUNT))=LYN_CapacityAssessmentCB_1.CFMID_1
							LEFT JOIN TB_EB_USER GROUPS2 ON SUBSTRING(GROUPS2.ACCOUNT, PATINDEX('%[0-9]%', GROUPS2.ACCOUNT), LEN(GROUPS2.ACCOUNT))=LYN_CapacityAssessmentCB_2.CFMID_2
                            WHERE 1=1 " + where;

            DataTable dt = new DataTable();
            dt.Load(this.m_db.ExecuteReader(SQL));
            this.m_db.Dispose();
            return dt;
        }
        public string retrieveActualSigner(string siteID, string taskID, string docNbr, string signStatus = "S")
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            //string cmdTxt = @"SELECT CURRENT_SIGNER FROM TB_WKF_TASK WHERE TASK_ID = @task_GUID; ";
            string cmdTxt = "";
            /*
            * Trường hợp SiteID là "" hoặc null thì đây là lần ký đầu tiên, nếu khác ""/null thì form này đã được ký xong. Sử dụng dữ liệu này để nhận biết form bị lỗi cần được xử lý dữ liệu lại khi vì
            * một lý do nào đó bị lỗi không thể hoàn thành tất cả các chức năng/công việc cần có.
            * signStatus = "E" ---[E viết tắt của End] nghĩa là kết thúc ký, [S <--> Signing] nghĩa là đang trong quá trình ký
            *
            */
            //start sign

            //siteID là trạng thái đang ký hoặc ký kết thúc
            //taskID là mã đơn
            if (string.IsNullOrWhiteSpace(siteID))
            {
                //CURRENT_SIGNER người tạo phiếu
                //cmdTxt = @"SELECT CURRENT_DOC.value('(/Form/Applicant/@userGuid)[1]', 'varchar(max)') AS CURRENT_SIGNER FROM TB_WKF_TASK WHERE TASK_ID = @task_GUID; ";
                cmdTxt = @"SELECT CURRENT_SIGNER FROM TB_WKF_TASK WHERE TASK_ID = @task_GUID;";
            }
            else
            {
                //kết thúc ký siteID khác null và signStatus = E
                //end sign, if siteID other null and signStatus=E then
                if (string.Equals(signStatus, "E", StringComparison.CurrentCultureIgnoreCase))
                {
                    //ACTUAL_SIGNER người ký kết thức
                    cmdTxt = @"SELECT TOP 1 ACTUAL_SIGNER FROM TB_WKF_TASK_NODE WHERE TASK_ID = @task_GUID ORDER BY FINISH_TIME DESC; ";
                }
                else //gửi ký lại khi có lỗi, retrieve action if error
                {
                    //ORIGINAL_SIGNER người đang ký
                    cmdTxt = @"SELECT ORIGINAL_SIGNER
                            FROM  TB_WKF_TASK_NODE
                            WHERE SITE_ID = @SITE_ID
                            ORDER BY FINISH_TIME DESC";
                    m_db.AddParameter("@SITE_ID", siteID);
                }
            }

            m_db.AddParameter("@task_GUID", taskID);

            object obj = m_db.ExecuteScalar(cmdTxt);

            if (obj != null && obj != DBNull.Value)
            {
                return obj.ToString();
            }
            else
            {
                return DBNull.Value.ToString();
            }
        }
    }
}
