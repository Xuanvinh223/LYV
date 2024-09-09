using System;
using System.Data;
using System.Xml.Linq;

namespace LYV.BoSungNhanSu.PO
{
    internal class BoSungNhanSuPO :Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal string GetEmployee(string UserID)
        {
            string conn = Training.Properties.Settings.Default.HRM.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            DataTable dt = new DataTable();
            string TenNV = "";
            string DonVi = "";
            string GioiTinh = "";
            string NgayvaoCT = "";
            string NgayThoiViec = "";
            string LyDoNghi = "";

            string cmdTxt = @"SELECT ST_NHANVIEN.NV_Ma, ST_NHANVIEN.NV_Ten, ST_NHANVIEN.DV_MA_,CASE WHEN  ST_NHANVIEN.NV_GioiTinh = '1' THEN N'Nam' else N'Nữ' end NV_GioiTinh,  ST_DONVI.KHU, ST_NHANVIEN.NV_Ngayvao, ST_NHANVIENTHOIVIEC.TV_NgayThoiViec, ST_NHANVIENTHOIVIEC.TV_LyDo
                              FROM ST_NHANVIEN 
                              LEFT JOIN ST_NHANVIENTHOIVIEC ON ST_NHANVIENTHOIVIEC.NV_Ma = ST_NHANVIEN.NV_Ma 
                              LEFT JOIN ST_DONVI ON ST_DONVI.DV_MA = ST_NHANVIEN.DV_MA 
                              WHERE ST_NHANVIEN.NV_Ma = @UserID and NV_THOIVIEC = '1'";

            this.m_db.AddParameter("@UserID", UserID);
            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            if (dt.Rows.Count > 0)
            {        
                TenNV = dt.Rows[0]["NV_Ten"].ToString(); ;
                DonVi = dt.Rows[0]["DV_MA_"].ToString(); ;
                GioiTinh = dt.Rows[0]["NV_GioiTinh"].ToString(); ;
                NgayvaoCT = dt.Rows[0]["NV_Ngayvao"].ToString(); ;
                NgayThoiViec = dt.Rows[0]["TV_NgayThoiViec"].ToString(); ;
                LyDoNghi = dt.Rows[0]["TV_LyDo"].ToString(); ;
            }

            string result = TenNV + ";" + DonVi + ";" + GioiTinh + ";" + NgayvaoCT + ";" + NgayThoiViec + ";" + LyDoNghi;

            this.m_db.Dispose();

            return result;

        }


        internal void InsertData_BeginForm(string LYV,string NoiNhan, string UserID, XElement xE)
        {
            string conn =Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

            string MaPhieu = xE.Attribute("MaPhieu").Value;
            string NoiGui = xE.Attribute("NoiGui").Value;
            string SLBienChe_Nam = xE.Attribute("SLBienChe_Nam").Value;
            string SLBienChe_Nu = xE.Attribute("SLBienChe_Nu").Value;
            string SLThucTe_Nam = xE.Attribute("SLThucTe_Nam").Value;
            string SLThucTe_Nu = xE.Attribute("SLThucTe_Nu").Value;
            string SLDeNghi = xE.Attribute("SLDeNghi").Value;
            string TrinhDoVH = xE.Attribute("TrinhDoVH").Value;
            string ThoiGianBS = xE.Attribute("ThoiGianBS").Value;
            string LyDoBS = xE.Attribute("LyDoBS").Value;
            string GhiChu = xE.Attribute("GhiChu").Value;
            string DNChude = xE.Attribute("DNChude").Value;
            string DNNoidung = xE.Attribute("DNNoidung").Value;

            int Person = Convert.ToInt32(xE.Attribute("Person").Value);

            string cmdTxt = @" INSERT INTO [dbo].[LYV_BoSungNhanSu]
                                       ([LYV]
                                       ,[MaPhieu]
                                       ,[NoiNhan]
                                       ,[NoiGui]
                                       ,[SLBienChe_Nam]
                                       ,[SLBienChe_Nu]
                                       ,[SLThucTe_Nam]
                                       ,[SLThucTe_Nu]
                                       ,[SLDeNghi]
                                       ,[TrinhDoVH]
                                       ,[ThoiGianBS]
                                       ,[LyDoBS]
                                       ,[GhiChu]
                                       ,[UserID]
                                       ,[UserDate]
                                       ,[flowflag]
                                       ,[DNChude]
                                       ,[DNNoidung])
                                 VALUES
                                 (	
                                       @LYV,
                                       @MaPhieu,
                                       @NoiNhan,
                                       @NoiGui,
                                       @SLBienChe_Nam,
                                       @SLBienChe_Nu,
                                       @SLThucTe_Nam,
                                       @SLThucTe_Nu,
                                       @SLDeNghi,
                                       @TrinhDoVH,
                                       @ThoiGianBS,
                                       @LyDoBS,
                                       @GhiChu,
                                       @UserID,
                                       getdate(),
                                       @flowflag,
                                       @DNChude,
                                       @DNNoidung
                                )  ";


            this.m_db.AddParameter("@LYV", LYV);
            this.m_db.AddParameter("@MaPhieu", MaPhieu);
            this.m_db.AddParameter("@NoiNhan", NoiNhan);
            this.m_db.AddParameter("@NoiGui", NoiGui);
            this.m_db.AddParameter("@SLBienChe_Nam", SLBienChe_Nam);
            this.m_db.AddParameter("@SLBienChe_Nu", SLBienChe_Nu);
            this.m_db.AddParameter("@SLThucTe_Nam", SLThucTe_Nam);
            this.m_db.AddParameter("@SLThucTe_Nu", SLThucTe_Nu);
            this.m_db.AddParameter("@SLDeNghi", SLDeNghi);
            this.m_db.AddParameter("@TrinhDoVH", TrinhDoVH);
            this.m_db.AddParameter("@ThoiGianBS", ThoiGianBS);
            this.m_db.AddParameter("@LyDoBS", LyDoBS);
            this.m_db.AddParameter("@GhiChu", GhiChu);
            this.m_db.AddParameter("@DNChude", DNChude);
            this.m_db.AddParameter("@DNNoidung", DNNoidung);
            this.m_db.AddParameter("@UserID", UserID);
            this.m_db.AddParameter("@flowflag", "N");

            for (int i = 0; i < Person; i++)
            {
                cmdTxt += @"  
                          INSERT INTO [dbo].[LYV_BoSungNhanSus]
                                   ([LYV]
                                   ,[DonVi]
                                   ,[MaNV]
                                   ,[TenNV]
                                   ,[GioiTinh]
                                   ,[NgayVaoCT]
                                   ,[NgayNghiViec]
                                   ,[LyDoNghi])
                             VALUES
                           (	
                                @LYV,
                                @DonVi" + i.ToString() + @",
                                @MaNV" + i.ToString() + @",
                                @TenNV" + i.ToString() + @",
                                @GioiTinh" + i.ToString() + @",
                                @NgayVaoCT" + i.ToString() + @",
                                @NgayNghiViec" + i.ToString() + @",
                                @LyDoNghi" + i.ToString() + @" 
                           )  ";

                XElement xE_1 = xE.Element("LYV_BSBN_" + i.ToString());
                this.m_db.AddParameter("@DonVi" + i.ToString(), xE_1.Attribute("DonVi").Value);
                this.m_db.AddParameter("@MaNV" + i.ToString(), xE_1.Attribute("MaNV").Value);
                this.m_db.AddParameter("@TenNV" + i.ToString(), xE_1.Attribute("TenNV").Value);
                this.m_db.AddParameter("@GioiTinh" + i.ToString(), xE_1.Attribute("GioiTinh").Value);
                this.m_db.AddParameter("@NgayVaoCT" + i.ToString(), xE_1.Attribute("NgayVaoCT").Value);
                this.m_db.AddParameter("@NgayNghiViec" + i.ToString(), xE_1.Attribute("NgayNghiViec").Value);
                this.m_db.AddParameter("@LyDoNghi" + i.ToString(), xE_1.Attribute("LyDoNghi").Value);

            }

            this.m_db.ExecuteNonQuery(cmdTxt);

            this.m_db.Dispose();
        }

        //
        internal void UpdateFormStatus(string LYV, string SiteCode , string signStatus,string NoiNhan, XElement xE)
        {
            string cmdflowflag = @"SELECT flowflag FROM LYV_BoSungNhanSu WHERE LYV = @LYV";

            DataTable dt = new DataTable();
            this.m_db.AddParameter("@LYV", LYV);
            dt.Load(this.m_db.ExecuteReader(cmdflowflag));

            string flowflag = dt.Rows[0][0].ToString(); //請假人工號
            if (flowflag == "NP" || flowflag == "N")
            {
                string conn = Training.Properties.Settings.Default.UOF.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);

                string MaPhieu = xE.Attribute("MaPhieu").Value;
                string NoiGui = xE.Attribute("NoiGui").Value;
                string SLBienChe_Nam = xE.Attribute("SLBienChe_Nam").Value;
                string SLBienChe_Nu = xE.Attribute("SLBienChe_Nu").Value;
                string SLThucTe_Nam = xE.Attribute("SLThucTe_Nam").Value;
                string SLThucTe_Nu = xE.Attribute("SLThucTe_Nu").Value;
                string SLDeNghi = xE.Attribute("SLDeNghi").Value;
                string TrinhDoVH = xE.Attribute("TrinhDoVH").Value;
                string ThoiGianBS = xE.Attribute("ThoiGianBS").Value;
                string LyDoBS = xE.Attribute("LyDoBS").Value;
                string GhiChu = xE.Attribute("GhiChu").Value;
                string DNChude = xE.Attribute("DNChude").Value;
                string DNNoidung = xE.Attribute("DNNoidung").Value;
                int Person = Convert.ToInt32(xE.Attribute("Person").Value);

                string cmdTxt = @" UPDATE [dbo].[LYV_BoSungNhanSu]
                                   SET [MaPhieu] = @MaPhieu 
                                      ,[NoiNhan] = @NoiNhan
                                      ,[NoiGui] = @NoiGui
                                      ,[SLBienChe_Nam] = @SLBienChe_Nam
                                      ,[SLBienChe_Nu] = @SLBienChe_Nu
                                      ,[SLThucTe_Nam] = @SLThucTe_Nam
                                      ,[SLThucTe_Nu] = @SLThucTe_Nu
                                      ,[SLDeNghi] = @SLDeNghi
                                      ,[TrinhDoVH] = @TrinhDoVH
                                      ,[ThoiGianBS] = @ThoiGianBS
                                      ,[LyDoBS] = @LyDoBS
                                      ,[GhiChu] =   @GhiChu
                                      ,[flowflag] = @flowflag
                                      ,[DNChude] = @DNChude
                                      ,[DNNoidung] = @DNNoidung
                                  WHERE LYV = @LYV 

                                  DELETE FROM LYV_BoSungNhanSus WHERE LYV=@LYV;
                                ";

                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.AddParameter("@MaPhieu", MaPhieu);
                this.m_db.AddParameter("@NoiNhan", NoiNhan);
                this.m_db.AddParameter("@NoiGui", NoiGui);
                this.m_db.AddParameter("@SLBienChe_Nam", SLBienChe_Nam);
                this.m_db.AddParameter("@SLBienChe_Nu", SLBienChe_Nu);
                this.m_db.AddParameter("@SLThucTe_Nam", SLThucTe_Nam);
                this.m_db.AddParameter("@SLThucTe_Nu", SLThucTe_Nu);
                this.m_db.AddParameter("@SLDeNghi", SLDeNghi);
                this.m_db.AddParameter("@TrinhDoVH", TrinhDoVH);
                this.m_db.AddParameter("@ThoiGianBS", ThoiGianBS);
                this.m_db.AddParameter("@LyDoBS", LyDoBS);
                this.m_db.AddParameter("@GhiChu", GhiChu);
                this.m_db.AddParameter("@DNChude", DNChude);
                this.m_db.AddParameter("@DNNoidung", DNNoidung);
                this.m_db.AddParameter("@flowflag", "P");
                this.m_db.ExecuteNonQuery(cmdTxt);

                for (int i = 0; i < Person; i++)
                {
                    cmdTxt += @"  
                          INSERT INTO [dbo].[LYV_BoSungNhanSus]
                                   ([LYV]
                                   ,[DonVi]
                                   ,[MaNV]
                                   ,[TenNV]
                                   ,[GioiTinh]
                                   ,[NgayVaoCT]
                                   ,[NgayNghiViec]
                                   ,[LyDoNghi])
                             VALUES
                           (	
                                @LYV,
                                @DonVi" + i.ToString() + @",
                                @MaNV" + i.ToString() + @",
                                @TenNV" + i.ToString() + @",
                                @GioiTinh" + i.ToString() + @",
                                @NgayVaoCT" + i.ToString() + @",
                                @NgayNghiViec" + i.ToString() + @",
                                @LyDoNghi" + i.ToString() + @" 
                           )  ";
                    XElement xE_1 = xE.Element("Training_BSBN_" + i.ToString());
                    this.m_db.AddParameter("@DonVi" + i.ToString(), xE_1.Attribute("DonVi").Value);
                    this.m_db.AddParameter("@MaNV" + i.ToString(), xE_1.Attribute("MaNV").Value);
                    this.m_db.AddParameter("@TenNV" + i.ToString(), xE_1.Attribute("TenNV").Value);
                    this.m_db.AddParameter("@GioiTinh" + i.ToString(), xE_1.Attribute("GioiTinh").Value);
                    this.m_db.AddParameter("@NgayVaoCT" + i.ToString(), xE_1.Attribute("NgayVaoCT").Value);
                    this.m_db.AddParameter("@NgayNghiViec" + i.ToString(), xE_1.Attribute("NgayNghiViec").Value);
                    this.m_db.AddParameter("@LyDoNghi" + i.ToString(), xE_1.Attribute("LyDoNghi").Value);

                }


            }
             if (SiteCode == "ReturnToApplicant")
            {
                string conn1 = Training.Properties.Settings.Default.UOF.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = "UPDATE LYV_BoSungNhanSu SET flowflag = 'NP' WHERE LYV = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
                this.m_db.Dispose();
            }
            else if (SiteCode == "EndFrom" && signStatus == "Approve" )
            {
                
                string cmdTxt = @"UPDATE LYV_BoSungNhanSu SET flowflag='Z' WHERE LYV = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);

            }
            else if (signStatus == "Disapprove")
            {
                string cmdTxt = @"UPDATE LYV_BoSungNhanSu SET flowflag='X' WHERE LYV = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
            }

            this.m_db.Dispose();
        }

        //
        internal void UpdateFormResult(string LYV, string formResult)
        {
            if (formResult == "Adopt")
            {    
                string conn1 = Training.Properties.Settings.Default.UOF.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = @"UPDATE LYV_BoSungNhanSu SET flowflag='Z' WHERE LYV = @LYV AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
                this.m_db.Dispose();
            }
            else if (formResult == "Reject" || formResult == "Cancel")
            {
                string conn1 = Training.Properties.Settings.Default.UOF.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = @"UPDATE LYV_BoSungNhanSu SET flowflag='X' WHERE LYV = @LYV ";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
                this.m_db.Dispose();
            }
            else
            {
                string conn1 = Training.Properties.Settings.Default.UOF.ToString();
                this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn1);
                string cmdTxt = @"UPDATE LYV_BoSungNhanSu SET flowflag='NP' WHERE LYV = @LYV AND flowflag IN ('N','P')";
                this.m_db.AddParameter("@LYV", LYV);
                this.m_db.ExecuteNonQuery(cmdTxt);
                this.m_db.Dispose();
            }
        }

        internal DataTable GetGridView_Close(string LYV, string MaPhieu, string NoiNhan,string LyDoBS, string Sdate, string Edate,string fLowflag)
        {

            string connStr = Training.Properties.Settings.Default.UOF.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(connStr);
            string cmdTxt = @"select LYV, MaPhieu,NoiGui, SLBienChe_Nam, SLBienChe_Nu, SLThucTe_Nam, SLThucTe_Nu, SLDeNghi,TrinhDoVH, ThoiGianBS, LyDoBS, GhiChu, UserID, TB_WKF_TASK.TASK_ID   from LYV_BoSungNhanSu 
                            LEFT JOIN TB_WKF_TASK ON TB_WKF_TASK.DOC_NBR = LYV_BoSungNhanSu.LYV
                            where LYV like '%" + LYV + @"%' and  MaPhieu like '%" + MaPhieu + @"%' and LyDoBS like N'%" + LyDoBS + @"%'
                            and flowflag like '%" + fLowflag + @"%' and NoiNhan  like N'%" + NoiNhan + @"%'";
            if (Sdate != "")
            {
                cmdTxt += @" and convert(smalldatetime,convert(varchar,UserDate,111))  >= '" + Sdate + @"'";
            }
            if (Edate != "")
            {
                cmdTxt += @" and  convert(smalldatetime,convert(varchar,UserDate,111))  <= '" + Edate + @"'  ";
            }


            DataTable dt = new DataTable();
                
            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            return dt;

        }

    }
}
