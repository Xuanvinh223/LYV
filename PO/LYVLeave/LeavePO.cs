using System;
using System.Data;
using System.Xml.Linq;

namespace Training.LYVLeave.PO
{
    internal class LeavePO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal string getFlowflag(string LNO)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdflowflag = @"SELECT flowflag FROM LYN_Leave WHERE LNO = @LNO";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LNO", LNO);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            string flowflag = dt.Rows[0][0].ToString(); //請假人工號

            this.m_db.Dispose();

            return flowflag;
        }
        internal DataTable GetUser()
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT NV_Ma, NV_Ten
                            FROM ST_NHANVIEN 
                            WHERE NV_Ma NOT IN(SELECT NV_Ma FROM ST_NHANVIENTHOIVIEC)
                            ORDER BY NV_Ma ";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal DataTable GetDep()
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT DV_MA,DV_TEN
                            FROM ST_DONVI  
                            WHERE dv_ma not like 'NV%' and dv_ma NOT IN('BGD','','BGDX','A1MDT','A2MDT','KB') 
                            AND dv_ma not like 'DT%' AND DV_LOCBB = 0 order by dv_ma ";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal string GetLeaveData(string ID, string Year)
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string LastYD = "0";
            string ThisYD = "0";
            string FiveYear = "0";
            string DH = "0";
            string NNDH = "0";
            string LeaveYD = "0";
            string LeaveYD_No1 = "0";
            string LeaveYD_OM = "0";
            string LeaveYD_CO = "0";
            string LeaveYD_RO = "0";
            string OnBoardDate = "No Data";
            string LeaveMD = "0";
            string LeaveMD_RO = "0";
            string LeaveMD_OM = "0";
            string LeaveMD_CO = "0";
            string FactoryClosed = "0";

            string selectSql = string.Empty;
            selectSql += "SELECT PN_Nam AS Year, NV_Ma AS ID, ISNULL(PN_Songaynamtruoc,0) AS LastYD, ISNULL(PN_Songaynamnay,0) AS ThisYD, ISNULL(PN_PhepTren5Nam,0) AS FiveYear, ISNULL(PN_NangnhocDH,0) AS DH, ISNULL(PN_SothanglamNNDH,0) AS NNDH FROM ST_PHEPNAM WHERE PN_Nam = @Year AND NV_Ma = @ID; \r\n";

            selectSql += "SELECT ISNULL(SUM(CASE WHEN NP_Ma = 'P' THEN (DATEDIFF(DD, NP_TUNGAY, NP_DENNGAY)+1)- (DATEDIFF(wk, NP_TUNGAY, NP_DENNGAY) * 1) END), 0) AS P, \r\n";
            selectSql += "ISNULL(SUM(CASE WHEN NP_Ma = 'No1' THEN (DATEDIFF(DD, NP_TUNGAY, NP_DENNGAY)+1)- (DATEDIFF(wk, NP_TUNGAY, NP_DENNGAY) * 1) END), 0) AS No1, \r\n";
            selectSql += "ISNULL(SUM(CASE WHEN NP_Ma = 'OM' THEN (DATEDIFF(DD, NP_TUNGAY, NP_DENNGAY)+1)- (DATEDIFF(wk, NP_TUNGAY, NP_DENNGAY) * 1) END), 0) AS OM, \r\n";
            selectSql += "ISNULL(SUM(CASE WHEN NP_Ma = 'CO' THEN (DATEDIFF(DD, NP_TUNGAY, NP_DENNGAY)+1)- (DATEDIFF(wk, NP_TUNGAY, NP_DENNGAY) * 1) END), 0) AS CO, \r\n";
            selectSql += "ISNULL(SUM(CASE WHEN NP_Ma = 'RO' THEN (DATEDIFF(DD, NP_TUNGAY, NP_DENNGAY)+1)- (DATEDIFF(wk, NP_TUNGAY, NP_DENNGAY) * 1) END), 0) AS RO, \r\n";
            selectSql += "ISNULL(SUM(CASE WHEN NP_CONGTYDUYET = 1 THEN (DATEDIFF(DD, NP_TUNGAY, NP_DENNGAY)+1)- (DATEDIFF(wk, NP_TUNGAY, NP_DENNGAY) * 1) END), 0) AS FC FROM ST_NHANVIENNGHIPHEP \r\n";
            selectSql += "WHERE NV_Ma = @ID \r\n";
            selectSql += "AND (YEAR(NP_TuNgay) = YEAR(GETDATE()) OR YEAR(NP_DenNgay) = YEAR(GETDATE())); \r\n";
            selectSql += "SELECT NV_Ngayvao FROM ST_NHANVIEN WHERE NV_Ma = @ID \r\n";
            selectSql += @" 
                            SELECT 
                                ISNULL(SUM(CASE WHEN NP_Ma = 'P' THEN DATEDIFF(DAY, StartDate, EndDate) + 1 - DATEDIFF(WEEK, StartDate, EndDate) END), 0) AS MonthLeaveDay, 
                                ISNULL(SUM(CASE WHEN NP_Ma = 'RO' THEN DATEDIFF(DAY, StartDate, EndDate) + 1 - DATEDIFF(WEEK, StartDate, EndDate) END), 0) AS RO, 
                                ISNULL(SUM(CASE WHEN NP_Ma = 'OM' THEN DATEDIFF(DAY, StartDate, EndDate) + 1 - DATEDIFF(WEEK, StartDate, EndDate) END), 0) AS OM, 
                                ISNULL(SUM(CASE WHEN NP_Ma = 'CO' THEN DATEDIFF(DAY, StartDate, EndDate) + 1 - DATEDIFF(WEEK, StartDate, EndDate) END), 0) AS CO 
                            FROM (
                                SELECT 
                                    NP_Ma,
                                    CASE 
                                        WHEN MONTH(NP_TUNGAY) = MONTH(GETDATE()) AND YEAR(NP_TUNGAY) = YEAR(GETDATE()) THEN NP_TUNGAY
                                        ELSE DATEADD(DAY, 1 - DAY(GETDATE()), GETDATE())
                                    END AS StartDate,
                                    CASE 
                                        WHEN MONTH(NP_DENNGAY) = MONTH(GETDATE()) AND YEAR(NP_DENNGAY) = YEAR(GETDATE()) THEN NP_DENNGAY
                                        ELSE DATEADD(DAY, -DAY(DATEADD(MONTH, 1, GETDATE())), DATEADD(MONTH, 1, GETDATE()))
                                    END AS EndDate
                                FROM ST_NHANVIENNGHIPHEP
                                WHERE NV_Ma = '@ID'
                                AND (
                                    (YEAR(NP_TuNgay) = YEAR(GETDATE()) AND MONTH(NP_TuNgay) = MONTH(GETDATE())) OR 
                                    (YEAR(NP_DenNgay) = YEAR(GETDATE()) AND MONTH(NP_DenNgay) = MONTH(GETDATE())) 
                                )
                            ) Result;";
            DataSet ds = new DataSet();
            this.m_db.AddParameter("@Year", Year);
            this.m_db.AddParameter("@ID", ID);
            ds = this.m_db.ExecuteDataSet(selectSql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LastYD = ds.Tables[0].Rows[0][2].ToString();
                ThisYD = ds.Tables[0].Rows[0][3].ToString();
                FiveYear = ds.Tables[0].Rows[0][4].ToString();
                DH = ds.Tables[0].Rows[0][5].ToString();
                NNDH = ds.Tables[0].Rows[0][6].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                LeaveYD = ds.Tables[1].Rows[0][0].ToString();
                LeaveYD_No1 = ds.Tables[1].Rows[0][1].ToString();
                LeaveYD_OM = ds.Tables[1].Rows[0][2].ToString();
                LeaveYD_CO = ds.Tables[1].Rows[0][3].ToString();
                LeaveYD_RO = ds.Tables[1].Rows[0][4].ToString();
                FactoryClosed = ds.Tables[1].Rows[0][5].ToString();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                OnBoardDate = DateTime.Parse(ds.Tables[2].Rows[0][0].ToString()).ToString("yyyy/MM/dd");
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                LeaveMD = ds.Tables[3].Rows[0][0].ToString();
                LeaveMD_RO = ds.Tables[3].Rows[0][1].ToString();
                LeaveMD_OM = ds.Tables[3].Rows[0][2].ToString();
                LeaveMD_CO = ds.Tables[3].Rows[0][3].ToString();
            }

            string result = OnBoardDate + ";" + LastYD + ";" + ThisYD + ";" + FiveYear + ";" + DH + ";" + NNDH + ";" + LeaveYD + ";" + LeaveMD + ";" + LeaveMD_RO
                            + ";" + LeaveMD_OM + ";" + LeaveMD_CO + ";" + LeaveYD_No1 + ";" + LeaveYD_OM + ";" + LeaveYD_CO + ";" + LeaveYD_RO + ";" + FactoryClosed;

            this.m_db.Dispose();

            return result;
        }
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

        internal DataTable ST_NHANVIENNGHIPHEP(string UserID)
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = "select ST_NHANVIENNGHIPHEP.* from ST_NHANVIENNGHIPHEP WHERE NV_Ma = @UserID ORDER BY NP_TuNgay DESC ";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@UserID", UserID);
            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        public int CalSunday(string StartDate, string EndDate)
        {
            DateTime SD = Convert.ToDateTime(StartDate);
            DateTime ED = Convert.ToDateTime(EndDate);
            int count = 0;
            while (SD <= ED)
            {
                if (SD.DayOfWeek == DayOfWeek.Sunday)
                {
                    count += 1;
                }
                SD = SD.AddDays(1);
            }
            return count;
        }
        internal string CheckLeaveDate(string USERID, string StartDate, string EndDate, string type)
        {
            string StartDateRange1 = "";
            string StartDateRange2 = "";
            string EndDateRange1 = "";
            string EndDateRange2 = "";
            string Flag0 = "NO";
            string Flag1 = "NO";
            /// Tinh tong ngay nghi
            int TotalDay = 1;
            TotalDay += Convert.ToDateTime(EndDate).Subtract(Convert.ToDateTime(StartDate)).Days;
            int Days = 1;

            if (StartDate != "")
            {
                StartDateRange1 = Convert.ToDateTime(StartDate).AddDays(-1).ToString("yyyy-MM-dd");
                StartDateRange2 = Convert.ToDateTime(StartDate).AddDays(-2).ToString("yyyy-MM-dd");
            }
            if (EndDate != "")
            {
                EndDateRange1 = Convert.ToDateTime(EndDate).AddDays(1).ToString("yyyy-MM-dd");
                EndDateRange2 = Convert.ToDateTime(EndDate).AddDays(2).ToString("yyyy-MM-dd");
            }

            string conn1 = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);

            string cmdTxt = @"SELECT * FROM LYN_Leave WHERE LeaverID = @USERID AND (StartDate BETWEEN @StartDate AND @EndDate OR EndDate BETWEEN @StartDate AND @EndDate) AND flowflag <> 'X' ";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@USERID", USERID);
            this.m_db.AddParameter("@StartDate", StartDate);
            this.m_db.AddParameter("@EndDate", EndDate);
            dt.Load(this.m_db.ExecuteReader(cmdTxt));
            if (dt.Rows.Count > 0)
            {
                Flag0 = "YES";
            }

            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string selectSql = string.Empty;
            selectSql += "SELECT * FROM ST_NHANVIENNGHIPHEP WHERE NV_Ma = @USERID AND NP_TuNgay = @StartDate AND NP_DenNgay = @EndDate;"; //檢查請假日期是否已選擇過
            selectSql += "SELECT * FROM ST_NHANVIENNGHIPHEP WHERE NV_Ma = @USERID AND (NP_TuNgay BETWEEN @StartDate AND @EndDate " +
                         "                                                         OR NP_DenNgay BETWEEN @StartDate AND @EndDate);"; //檢查申請日期內是否包含其它申請單的日期
            selectSql += "SELECT * FROM ST_NHANVIENNGHIPHEP WHERE NV_Ma = @USERID AND NP_DenNgay = @StartDateRange1;"; //檢查申請日期是否連續
            selectSql += "SELECT * FROM ST_NHANVIENNGHIPHEP WHERE NV_Ma = @USERID AND NP_DenNgay = @StartDateRange2;"; //檢查申請日期是否連續
            selectSql += "SELECT * FROM ST_NHANVIENNGHIPHEP WHERE NV_Ma = @USERID AND NP_TuNgay = @EndDateRange1;"; //檢查申請日期是否連續
            selectSql += "SELECT * FROM ST_NHANVIENNGHIPHEP WHERE NV_Ma = @USERID AND NP_TuNgay = @EndDateRange2;"; //檢查申請日期是否連續

            DataSet ds = new DataSet();
            this.m_db.AddParameter("@USERID", USERID);
            this.m_db.AddParameter("@StartDate", StartDate);
            this.m_db.AddParameter("@EndDate", EndDate);
            this.m_db.AddParameter("@StartDateRange1", StartDateRange1);
            this.m_db.AddParameter("@StartDateRange2", StartDateRange2);
            this.m_db.AddParameter("@EndDateRange1", EndDateRange1);
            this.m_db.AddParameter("@EndDateRange2", EndDateRange2);
            ds = this.m_db.ExecuteDataSet(selectSql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Flag0 = "YES";
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                Flag1 = "YES";
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                Days += Convert.ToDateTime(ds.Tables[2].Rows[0][2]).Subtract(Convert.ToDateTime(ds.Tables[2].Rows[0][1])).Days + ds.Tables[2].Rows.Count;
                if (ds.Tables[3].Rows.Count > 0)
                {
                    Days += Convert.ToDateTime(ds.Tables[3].Rows[0][2]).Subtract(Convert.ToDateTime(ds.Tables[3].Rows[0][1])).Days + ds.Tables[3].Rows.Count;
                }
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                Days += Convert.ToDateTime(ds.Tables[4].Rows[0][2]).Subtract(Convert.ToDateTime(ds.Tables[4].Rows[0][1])).Days + ds.Tables[4].Rows.Count;
                if (ds.Tables[5].Rows.Count > 0)
                {
                    Days += Convert.ToDateTime(ds.Tables[5].Rows[0][2]).Subtract(Convert.ToDateTime(ds.Tables[5].Rows[0][1])).Days + ds.Tables[5].Rows.Count;
                }
            }
            Days += Convert.ToDateTime(EndDate).Subtract(Convert.ToDateTime(StartDate)).Days;
            //扣除星期日
            if (type == "Ds" || type == "KHHGD" || type == "ST" || type == "TS")
            {
            }
            else
            {
                Days -= CalSunday(StartDate, EndDate);
                //
                TotalDay -= CalSunday(StartDate, EndDate);
            }

            string result = Flag0 + ";" + Flag1 + ";" + Days + ";" + TotalDay;

            this.m_db.Dispose();

            return result;
        }
        //internal string Check_KhoaSo(string USERID, string StartDate)
        //{
        //    string conn = Training.Properties.Settings.Default.HRM.ToString();
        //    this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

        //    string flag = string.Empty;
        //    string cmdTxt = @"Select Case when MONTH(@StartDate) =SUBSTRING(KS_THANG, 1, 2) and Year(@StartDate)= SUBSTRING(KS_THANG, 4, 4) then 1 else 0 end
        //                    FROM ST_CONGTHOIVIEC_KHOASO  
        //                    WHERE NV_Ma = @USERID";

        //    DataTable dt = new DataTable();
        //    this.m_db.AddParameter("@USERID", USERID);
        //    this.m_db.AddParameter("@StartDate", StartDate);
        //    dt.Load(this.m_db.ExecuteReader(cmdTxt));
        //    if (dt.Rows.Count > 0)
        //    {
        //        flag = dt.Rows[0][0].ToString();
        //    }

        //    this.m_db.Dispose();

        //    if (flag == "1")
        //    {
        //        return "true";
        //    }
        //    else
        //    {
        //        return "false";
        //    }
        //}
        //internal string Check_KhoaSo_All(string LNO)
        //{
        //    string conn = Training.Properties.Settings.Default.UOF.ToString();
        //    this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
        //    string cmdTxt = @"SELECT LeaverID, StartDate FROM LYN_Leave WHERE LNO = @LNO ";

        //    DataTable dt = new DataTable();
        //    this.m_db.AddParameter("@LNO", LNO);
        //    dt.Load(this.m_db.ExecuteReader(cmdTxt));

        //    string USERID = dt.Rows[0][0].ToString(); //請假人部門
        //    string StartDate = dt.Rows[0][1].ToString(); //請假人工號

        //    string flag = string.Empty;
        //    string cmdTxt1 = @"Select Case when MONTH(@StartDate) =SUBSTRING(KS_THANG, 1, 2) and Year(@StartDate)= SUBSTRING(KS_THANG, 4, 4) then 1 else 0 end
        //                    FROM [HRS].[P0104-TYXUAN].[dbo].[ST_CONGTHOIVIEC_KHOASO]  ST_CONGTHOIVIEC_KHOASO
        //                    WHERE NV_Ma = @USERID";

        //    DataTable dt1 = new DataTable();
        //    this.m_db.AddParameter("@USERID", USERID);
        //    this.m_db.AddParameter("@StartDate", StartDate);
        //    dt1.Load(this.m_db.ExecuteReader(cmdTxt1));
        //    if (dt1.Rows.Count > 0)
        //    {
        //        flag = dt1.Rows[0][0].ToString();
        //    }

        //    this.m_db.Dispose();

        //    if (flag == "1")
        //    {
        //        return "true";
        //    }
        //    else
        //    {
        //        return "false";
        //    }
        //}
        internal void InsertLeaveFormData(string LNO, string UserID, string Factory, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string DepartmentID = xE.Attribute("DepartmentID").Value;
            string LeaverID = xE.Attribute("LeaverID").Value;
            string LeaverName = xE.Attribute("LeaverName").Value;
            string DeputyID = xE.Attribute("DeputyID").Value;
            string DeputyName = xE.Attribute("DeputyName").Value;
            string Type = xE.Attribute("Type").Value;
            string LeaveDays = xE.Attribute("LeaveDays").Value;
            string TotalDay = xE.Attribute("TotalDay").Value;
            string TotalHour = xE.Attribute("TotalHour").Value;
            string StartDate = xE.Attribute("StartDate").Value;
            string StartTime = xE.Attribute("StartTime").Value;
            string EndDate = xE.Attribute("EndDate").Value;
            string EndTime = xE.Attribute("EndTime").Value;
            string Reason = xE.Attribute("Reason").Value;
            string Remark = xE.Attribute("Remark").Value;
            string Documents = xE.Attribute("Documents").Value;
            string DHMonth = xE.Attribute("DHMonth").Value;
            string YDLeave = xE.Attribute("YDLeave").Value;
            string Total = xE.Attribute("Total").Value;
            string flowflag = xE.Attribute("flowflag").Value;

            string cmdTxt = @"  INSERT INTO LYN_Leave
                                (	 [LNO] ,  
                                     [Factory] , 
                                     [DepartmentID] ,
                                     [LeaverID] ,
                                     [LeaverName] ,
                                     [DeputyID] ,
                                     [DeputyName] ,
                                     [Type] ,
                                     [LeaveDays] ,
                                     [TotalDay] ,
                                     [TotalHour] ,
                                     [StartDate] ,
                                     [StartTime] ,
                                     [EndDate] ,
                                     [EndTime] ,
                                     [Reason] ,
                                     [Remark] ,
                                     [Documents] ,
                                     [DHMonth] ,
                                     [YDLeave] ,
                                     [Total] ,
                                     [flowflag] ,  
                                     [USERID] ,             
                                     [USERDATE] 
                                ) 
                                 VALUES 
                                 (	
                                     @LNO,
                                     @Factory,
                                     @DepartmentID,
                                     @LeaverID,
                                     @LeaverName,
                                     @DeputyID,
                                     @DeputyName,
                                     @Type,
                                     @LeaveDays,
                                     @TotalDay,
                                     @TotalHour,
                                     @StartDate,
                                     @StartTime,
                                     @EndDate,
                                     @EndTime,
                                     @Reason,
                                     @Remark,
                                     @Documents,
                                     @DHMonth,
                                     @YDLeave,
                                     @Total,
                                     @flowflag,
                                     @USERID,
                                     getdate()
                                )";

            this.m_db.AddParameter("@LNO", LNO);
            this.m_db.AddParameter("@Factory", Factory);
            this.m_db.AddParameter("@DepartmentID", DepartmentID);
            this.m_db.AddParameter("@LeaverID", LeaverID);
            this.m_db.AddParameter("@LeaverName", LeaverName);
            this.m_db.AddParameter("@DeputyID", DeputyID);
            this.m_db.AddParameter("@DeputyName", DeputyName);
            this.m_db.AddParameter("@Type", Type);
            this.m_db.AddParameter("@LeaveDays", LeaveDays);
            this.m_db.AddParameter("@TotalDay", TotalDay);
            this.m_db.AddParameter("@TotalHour", TotalHour);
            this.m_db.AddParameter("@StartDate", StartDate);
            this.m_db.AddParameter("@StartTime", StartTime);
            this.m_db.AddParameter("@EndDate", EndDate);
            this.m_db.AddParameter("@EndTime", EndTime);
            this.m_db.AddParameter("@Reason", Reason);
            this.m_db.AddParameter("@Remark", Remark);
            this.m_db.AddParameter("@Documents", Documents);
            this.m_db.AddParameter("@DHMonth", DHMonth);
            this.m_db.AddParameter("@YDLeave", YDLeave);
            this.m_db.AddParameter("@Total", Total);
            this.m_db.AddParameter("@flowflag", flowflag);
            this.m_db.AddParameter("@USERID", UserID);

            this.m_db.ExecuteNonQuery(cmdTxt);

            this.m_db.Dispose();
        }
        internal void WriteHRM(string ApplyDate, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string DepartmentID = xE.Attribute("DepartmentID").Value;
            string LeaverID = xE.Attribute("LeaverID").Value;
            string Type = xE.Attribute("Type").Value;
            string StartDate = Convert.ToDateTime(xE.Attribute("StartDate").Value).ToString("yyyy-MM-dd HH:mm:ss");
            string EndDate = Convert.ToDateTime(xE.Attribute("EndDate").Value).ToString("yyyy-MM-dd HH:mm:ss");
            string Remark = xE.Attribute("Remark").Value;
            string CloseDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //申請單結案時間

            string SQL = "EXEC SP_KTRA_PHEP_LONGNHAU @NV_MA='" + LeaverID + "', @TUNGAY = '" + StartDate + "', @THAOTAC = 'THEM', @DENNGAY = '" + EndDate + "'";
            DataSet DS = this.m_db.ExecuteDataSet(SQL); //送出SQL語句

            if (DS.Tables[0].Rows.Count == 0)
            {
                SQL = string.Empty;
                SQL += "INSERT INTO ST_NHANVIENNGHIPHEP(NV_Ma, NP_TuNgay, NP_DenNgay, DV_Ma, NP_Ma, NP_GhiChu, NP_NGAYNHAPPHEP, NP_NGAYNHAPSUA) ";
                SQL += "VALUES('" + LeaverID + "', '" + StartDate + "', '" + EndDate + "', '" + DepartmentID + "', '" + Type + "', N'" + Remark.Replace("'", "''") + "', '" + ApplyDate + "', '" + CloseDate + "');";
                //請的假別如為產假則需再寫入IT_NUOICONDUOI12THANG
                if (Type == "TS")
                {
                    SQL += "INSERT INTO IT_NUOICONDUOI12THANG(DV_MA, NV_MA, TUNGAY, DENNGAY, NGAYSANH) ";
                    SQL += "VALUES('" + DepartmentID + "', '" + LeaverID + "', '" + StartDate + "', '" + EndDate + "', '');";
                }

                this.m_db.ExecuteNonQuery(SQL); //送出SQL語句
            }

            this.m_db.Dispose();
        }
        internal void DisableFlowflag(string LNO)
        {
            string conn1 = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);

            string cmdTxt = @"SELECT LeaverID, StartDate, EndDate, Type FROM LYN_Leave WHERE LNO = @LNO AND flowflag IN ('Z')";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LNO", LNO);
            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            string cmdTxt1 = @"UPDATE LYN_Leave SET flowflag = 'X' WHERE LNO = '" + LNO + "'";
            this.m_db.ExecuteNonQuery(cmdTxt1);

            if (dt.Rows.Count > 0)
            {
                string LeaverID = dt.Rows[0][0].ToString();
                string StartDate = Convert.ToDateTime(dt.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
                string EndDate = Convert.ToDateTime(dt.Rows[0][2]).ToString("yyyy-MM-dd HH:mm:ss");
                string Type = dt.Rows[0][3].ToString();

                string conn = Training.Properties.Settings.Default.HRM.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

                string SQL = string.Empty;
                SQL += "Delete ST_NHANVIENNGHIPHEP where NV_Ma = '" + LeaverID + "' and NP_TuNgay = '" + StartDate + "' and NP_DenNgay = '" + EndDate + "' and NP_Ma = '" + Type + "' ";
                //請的假別如為產假則需再寫入IT_NUOICONDUOI12THANG
                if (Type == "TS")
                {
                    SQL += "Delete IT_NUOICONDUOI12THANG where NV_Ma = '" + LeaverID + "' and NP_TuNgay = '" + StartDate + "' and NP_DenNgay = '" + EndDate + "'";
                }

                this.m_db.ExecuteNonQuery(SQL); //送出SQL語句
            }

            this.m_db.Dispose();
        }
        //internal string CheckHRMData(string LNO)
        //{
        //    string conn1 = Training.Properties.Settings.Default.UOF.ToString();
        //    this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);

        //    string cmdTxt = @"SELECT LeaverID, StartDate, EndDate FROM LYN_Leave WHERE LNO = @LNO AND flowflag IN ('N','P')";

        //    DataTable dt = new DataTable();
        //    this.m_db.AddParameter("@LNO", LNO);
        //    dt.Load(this.m_db.ExecuteReader(cmdTxt));

        //    string LeaverID = dt.Rows[0][1].ToString(); //請假人工號
        //    string StartDate = Convert.ToDateTime(dt.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss"); //請假開始時間
        //    string EndDate = Convert.ToDateTime(dt.Rows[0][2]).ToString("yyyy-MM-dd HH:mm:ss"); //請假結束時間

        //    string conn = Training.Properties.Settings.Default.HRM.ToString();
        //    this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

        //    string SQL = "EXEC SP_KTRA_PHEP_LONGNHAU @NV_MA='" + LeaverID + "', @TUNGAY = '" + StartDate + "', @THAOTAC = 'THEM', @DENNGAY = '" + EndDate + "'";
        //    DataSet ds = this.m_db.ExecuteDataSet(SQL); //送出SQL語句

        //    this.m_db.Dispose();

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        /*if (StartDate != Convert.ToDateTime(ds.Tables[0].Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss") || EndDate != Convert.ToDateTime(ds.Tables[0].Rows[0][2]).ToString("yyyy-MM-dd HH:mm:ss"))
        //        {
        //            return "false"; //相關日期有請假資料則不通過檢核，退回上一關
        //        }
        //        else
        //        {
        //            return "true";
        //        }*/
        //        return "false"; //相關日期有請假資料則不通過檢核，退回上一關
        //    }
        //    else
        //    {
        //        return "true";
        //    }
        //}
        internal void UpdateFormStatus(string LNO, string SiteCode, string signStatus, string Factory, XElement xE)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string Type = xE.Attribute("Type").Value;
            string LeaveDays = xE.Attribute("LeaveDays").Value;
            string cmdflowflag = @"SELECT flowflag FROM LYN_Leave WHERE LNO = @LNO";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LNO", LNO);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            string flowflag = dt.Rows[0][0].ToString(); //請假人工號

            string result = "0";

            if (Type == "P" || Type == "No1")
            {
                result = "1";
            }
            else
            {
                if (Convert.ToInt32(LeaveDays) >= 10)
                {
                    result = "2";
                }
                else
                {
                    result = "1";
                }
            }

            if ((flowflag == "NP" || flowflag == "N") && SiteCode != "ReturnToApplicant")
            {
                string DepartmentID = xE.Attribute("DepartmentID").Value;
                string LeaverID = xE.Attribute("LeaverID").Value;
                string LeaverName = xE.Attribute("LeaverName").Value;
                string DeputyID = xE.Attribute("DeputyID").Value;
                string DeputyName = xE.Attribute("DeputyName").Value;
                string TotalDay = xE.Attribute("TotalDay").Value;
                string TotalHour = xE.Attribute("TotalHour").Value;
                string StartDate = xE.Attribute("StartDate").Value;
                string StartTime = xE.Attribute("StartTime").Value;
                string EndDate = xE.Attribute("EndDate").Value;
                string EndTime = xE.Attribute("EndTime").Value;
                string Reason = xE.Attribute("Reason").Value;
                string Remark = xE.Attribute("Remark").Value;
                string Documents = xE.Attribute("Documents").Value;
                string DHMonth = xE.Attribute("DHMonth").Value;
                string YDLeave = xE.Attribute("YDLeave").Value;
                string Total = xE.Attribute("Total").Value;

                string cmdTxt = @"  UPDATE LYN_Leave SET
                                     [Factory] = @Factory,
                                     [DepartmentID] = @DepartmentID,
                                     [LeaverID] = @LeaverID,
                                     [LeaverName] = @LeaverName,
                                     [DeputyID] = @DeputyID,
                                     [DeputyName] = @DeputyName,
                                     [Type] = @Type,
                                     [LeaveDays] = @LeaveDays,
                                     [TotalDay] = @TotalDay,
                                     [TotalHour] = @TotalHour,
                                     [StartDate] = @StartDate,
                                     [StartTime] = @StartTime,
                                     [EndDate] = @EndDate,
                                     [EndTime] = @EndTime,
                                     [Reason] = @Reason,
                                     [Remark] = @Remark,
                                     [Documents] = @Documents,
                                     [DHMonth] = @DHMonth,
                                     [YDLeave] = @YDLeave,
                                     [Total] = @Total,
                                     [flowflag] = @flowflag
                                 WHERE
                                     LNO=@LNO
                                     ";

                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.AddParameter("@Factory", Factory);
                this.m_db.AddParameter("@DepartmentID", DepartmentID);
                this.m_db.AddParameter("@LeaverID", LeaverID);
                this.m_db.AddParameter("@LeaverName", LeaverName);
                this.m_db.AddParameter("@DeputyID", DeputyID);
                this.m_db.AddParameter("@DeputyName", DeputyName);
                this.m_db.AddParameter("@Type", Type);
                this.m_db.AddParameter("@LeaveDays", LeaveDays);
                this.m_db.AddParameter("@TotalDay", TotalDay);
                this.m_db.AddParameter("@TotalHour", TotalHour);
                this.m_db.AddParameter("@StartDate", StartDate);
                this.m_db.AddParameter("@StartTime", StartTime);
                this.m_db.AddParameter("@EndDate", EndDate);
                this.m_db.AddParameter("@EndTime", EndTime);
                this.m_db.AddParameter("@Reason", Reason);
                this.m_db.AddParameter("@Remark", Remark);
                this.m_db.AddParameter("@Documents", Documents);
                this.m_db.AddParameter("@DHMonth", DHMonth);
                this.m_db.AddParameter("@YDLeave", YDLeave);
                this.m_db.AddParameter("@Total", Total);
                this.m_db.AddParameter("@flowflag", "N");

                this.m_db.ExecuteNonQuery(cmdTxt);

                if (!string.IsNullOrEmpty(SiteCode))
                {
                    string cmdTxt1 = "UPDATE LYN_Leave SET flowflag = 'P' WHERE LNO = @LNO ";
                    this.m_db.AddParameter("@LNO", LNO);
                    this.m_db.ExecuteNonQuery(cmdTxt1);
                }
            }
            if (SiteCode == "ReturnToApplicant")
            {
                string cmdTxt = "UPDATE LYN_Leave SET flowflag = 'NP' WHERE LNO = @LNO ";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if ((result == "1" && SiteCode == "SE1" && signStatus == "Approve") || (result == "2" && SiteCode == "SE2" && signStatus == "Approve"))
            {
                string cmdTxt = @"UPDATE LYN_Leave SET flowflag='Z' WHERE LNO = @LNO AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (signStatus == "Disapprove")
            {
                string cmdTxt = @"UPDATE LYN_Leave SET flowflag='X' WHERE LNO = @LNO ";
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
                string cmdTxt = @"UPDATE LYN_Leave SET flowflag='Z' WHERE LNO = @LNO AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }
            else if (formResult == "Reject" || formResult == "Cancel")
            {
                string cmdTxt = @"UPDATE LYN_Leave SET flowflag='X' WHERE LNO = @LNO ";
                this.m_db.AddParameter("@LNO", LNO);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
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
        internal DataTable GetListLeave(string D_STEP_DESC, string flowflag, string DV_MA_, string LNO, string LeaverID, string StartDate, string EndDate, string Documents, string Factory, string Account)
        {
            string conn = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            if (Factory != "")
            {

                string where = " and Factory = '" + Factory + "' ";

                if (DV_MA_ != "") where += " and LOWER(DepartmentID) like LOWER('%" + DV_MA_ + "%') ";
                if (LeaverID != "") where += " and LeaverID like '" + LeaverID + "%' ";
                if (LNO != "") where += " and LOWER(LNO) like LOWER('%" + LNO + "%') ";
                if (StartDate != "") where += " and StartDate >= '" + StartDate + "' ";
                if (EndDate != "") where += " and EndDate <= '" + EndDate + "' ";
                if (D_STEP_DESC == "ALL")
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
                else
                {

                    if (Account == "LYN20067")
                    {
                        where += " and flowflag not in ('X','Z') and STEP_DESC='HRManagerA' ";
                    }
                    else if (Account == "LYN21048")
                    {
                        where += " and flowflag not in ('X','Z') and STEP_DESC='HRManagerHC' ";
                    }
                    else if (Factory == "A")
                    {
                        where += " and flowflag not in ('X','Z') and STEP_DESC='人事A' ";
                    }
                    else
                    {
                        where += " and flowflag not in ('X','Z') and STEP_DESC='人事HC' ";
                    }
                }

                string SQL = @" WITH TB_WKF_TASK_NODE_CTE AS (
                                    SELECT TB_WKF_TASK.TASK_ID, DOC_NBR, 
		                                CASE WHEN (GROUP_NAME = 'HR/CR') AND (TB_EB_JOB_TITLE.TITLE_NAME = '越籍人員' OR ISNULL(TB_EB_JOB_TITLE.TITLE_NAME, '') = '') THEN '人事A' 
		                                WHEN (GROUP_NAME = '行政事務') AND (TB_EB_JOB_TITLE.TITLE_NAME = '越籍人員' OR ISNULL(TB_EB_JOB_TITLE.TITLE_NAME, '') = '') THEN '人事HC' 
                                        WHEN (GROUP_NAME = 'HR/CR') AND (TB_EB_JOB_TITLE.TITLE_NAME <> '越籍人員') THEN 'HRManagerA'
		                                WHEN (GROUP_NAME = '行政事務') AND (TB_EB_JOB_TITLE.TITLE_NAME <> '越籍人員') THEN 'HRManagerHC' ELSE TB_EB_JOB_TITLE.TITLE_NAME END AS STEP_DESC, 
                                        ROW_NUMBER() OVER (PARTITION BY DOC_NBR ORDER BY START_TIME DESC) AS rn
                                    FROM TB_WKF_TASK LEFT JOIN TB_WKF_TASK_NODE ON TB_WKF_TASK.TASK_ID = TB_WKF_TASK_NODE.TASK_ID 
                                    LEFT JOIN TB_WKF_TASK_NODE_SIGNER_INFO ON TB_WKF_TASK_NODE_SIGNER_INFO.SITE_ID = TB_WKF_TASK_NODE.SITE_ID AND TB_WKF_TASK_NODE_SIGNER_INFO.NODE_SEQ = TB_WKF_TASK_NODE.NODE_SEQ 
                                    LEFT JOIN TB_EB_GROUP ON TB_WKF_TASK_NODE_SIGNER_INFO.GROUP_ID = TB_EB_GROUP.GROUP_ID 
                                    LEFT JOIN TB_EB_JOB_TITLE ON TB_EB_JOB_TITLE.TITLE_ID = TB_WKF_TASK_NODE_SIGNER_INFO.TITLE_ID  
                                    WHERE END_TIME IS NULL AND FINISH_TIME IS NULL 
                                ),
                                Name_CTE AS (
                                    SELECT  SUBSTRING(ACCOUNT, PATINDEX('%[0-9]%', ACCOUNT), LEN(ACCOUNT)) UserID, NAME 
                                    FROM TB_EB_USER 
                                )
                                SELECT LYN_Leave.LNO, LYN_Leave.USERID, Name_CTE.NAME as USERNAME, LYN_Leave.DepartmentID DV_MA_, LYN_Leave.DepartmentID, LYN_Leave.LeaverID, LYN_Leave.LeaverName, LYN_Leave.Type,
                                       LYN_Leave.TotalDay, LYN_Leave.StartDate, LYN_Leave.EndDate, LYN_Leave.DHMonth, LYN_Leave.YDLeave, LYN_Leave.Total, LYN_Leave.Reason, LYN_Leave.Remark, 
	                                   CONVERT(VARCHAR, LYN_Leave.USERDATE, 120) USERDATE, ISNULL(TB_WKF_TASK_NODE_CTE.STEP_DESC, '已結案') AS D_STEP_DESC, TB_WKF_TASK.TASK_ID, TB_WKF_TASK.ATTACH_ID, LYN_Leave.Documents 
                                FROM LYN_Leave LEFT JOIN TB_WKF_TASK ON LYN_Leave.LNO = TB_WKF_TASK.DOC_NBR 
                                LEFT JOIN (SELECT * FROM TB_WKF_TASK_NODE_CTE WHERE rn = 1) AS TB_WKF_TASK_NODE_CTE ON LYN_Leave.LNO = TB_WKF_TASK_NODE_CTE.DOC_NBR 
                                LEFT JOIN Name_CTE ON Name_CTE.UserID = LYN_Leave.USERID 
                                WHERE 1=1 " + where + @" 
                                ORDER BY LYN_Leave.LNO desc ";

                DataTable dt = new DataTable();
                this.m_db.Command.CommandTimeout = 120;
                dt.Load(this.m_db.ExecuteReader(SQL));

                this.m_db.Dispose();

                return dt;
            }
            else
            {
                string where = "";

                if (DV_MA_ != "") where += " and LOWER(DepartmentID) like LOWER('%" + DV_MA_ + "%') ";
                if (LeaverID != "") where += " and LeaverID like '" + LeaverID + "%' ";
                if (LNO != "") where += " and LOWER(LNO) like LOWER('%" + LNO + "%') ";
                if (StartDate != "") where += " and StartDate >= '" + StartDate + "' ";
                if (EndDate != "") where += " and EndDate <= '" + EndDate + "' ";
                if (Documents == "0")
                {
                    where += " and Documents = 'N' ";
                }
                else
                {
                    where += " and Documents = 'Y' ";
                }
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

                string SQL = @" SELECT LYN_Leave.LNO, LYN_Leave.USERID, Name.NAME as USERNAME, LYN_Leave.DepartmentID DV_MA_, LYN_Leave.DepartmentID, LYN_Leave.LeaverID, LYN_Leave.LeaverName, LYN_Leave.Type, 
                                       LYN_Leave.TotalDay, LYN_Leave.StartDate, LYN_Leave.EndDate, LYN_Leave.DHMonth, LYN_Leave.YDLeave, LYN_Leave.Total, LYN_Leave.Reason, LYN_Leave.Remark, 
                                       CONVERT(VARCHAR,LYN_Leave.USERDATE,120) USERDATE, '已結案' AS D_STEP_DESC, TB_WKF_TASK.TASK_ID, TB_WKF_TASK.ATTACH_ID, LYN_Leave.Documents 
                                FROM LYN_Leave LEFT JOIN TB_WKF_TASK on LYN_Leave.LNO=TB_WKF_TASK.DOC_NBR 
                                LEFT JOIN ( SELECT SUBSTRING(ACCOUNT, PATINDEX('%[0-9]%', ACCOUNT), LEN(ACCOUNT)) UserID, NAME FROM TB_EB_USER ) AS Name ON Name.UserID = LYN_Leave.USERID 
                                WHERE 1=1 " + where + @" 
                                ORDER BY LYN_Leave.LNO desc ";

                DataTable dt = new DataTable();
                dt.Load(this.m_db.ExecuteReader(SQL));

                this.m_db.Dispose();

                return dt;
            }
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
        internal void UpdateWFLeave(string flag, string Where)
        {
            string conn1 = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
            string cmdTxt = "UPDATE LYN_Leave SET Documents = '" + flag + "' WHERE " + Where;
            this.m_db.ExecuteNonQuery(cmdTxt);

            this.m_db.Dispose();
        }
    }
}
