using System.Data;

namespace LYN.MaterialExports.PO
{
    internal class MaterialExportSPO : Ede.Uof.Utility.Data.BasePersistentObject
    {
        internal DataTable GetKCLLS_X(string LLNO)
        {
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT KCLLS.LLNO, KCLLS.CLBH, CLZL.DWBH, CONVERT(VARCHAR, KCLLS.TempQty) AS TempQty, KCLLS.YYBH, SCBLYY.YWSM, SCBLYY.ZWSM, CLZL.YWPM, CLZL.ZWPM, KCLLS.MEMO, CONVERT(VARCHAR, ISNULL(KCZLS.KCQty,0)) AS KCQty, 
                                     CONVERT(VARCHAR, (ISNULL(UnKCLL.UnLLQty,0)-CASE KCLL.CFMID WHEN 'NO' THEN KCLLS.Tempqty ELSE 0 END)) AS UnLLQty, 
                                     CONVERT(VARCHAR, (ISNULL(KCZLS.KCQty,0)-ISNULL(UnKCLL.UnLLQty,0)+CASE KCLL.CFMID WHEN 'NO' THEN KCLLS.Tempqty ELSE 0 END)) AS 
                                     AvailQty FROM KCLLS 
                              LEFT JOIN KCLL ON KCLL.LLNO = KCLLS.LLNO 
                              LEFT JOIN CLZL ON CLZL.CLDH = KCLLS.CLBH 
                              LEFT JOIN KCZLS ON KCZLS.CLBH = KCLLS.CLBH AND KCZLS.CKBH = KCLL.CKBH 
                              LEFT JOIN ( 
                                    SELECT KCLLS.CLBH, KCLL.CKBH, SUM(KCLLS.TempQty) AS UnLLQty FROM KCLLS, KCLL 
                                    WHERE KCLLS.LLNO = KCLL.LLNO AND KCLL.CFMID = 'NO' and flowflag <>'X' GROUP BY KCLLS.CLBH,KCLL.CKBH 
                                  ) AS UnKCLL ON UnKCLL.CLBH = KCLLS.CLBH AND UnKCLL.CKBH = KCLL.CKBH 
                              LEFT JOIN SCBLYY ON SCBLYY.YYBH = KCLLS.YYBH 
                              WHERE KCLLS.LLNO = '" + LLNO + "' ";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
        internal DataTable GetKCLLS(string LLNO)
        {
            string conn = Training.Properties.Settings.Default.LYV_ERP.ToString();
            this.m_db = new Ede.Uof.Utility.Data.DatabaseHelper(conn);
            string cmdTxt = @"SELECT KCLLS.LLNO,KCLLS.CLBH,KCLLS.DFL,KCLLS.SCBH,KCLLS.TempQty,KCLLS.Qty,KCLLS.HGLB,KCLLS.CNO,KCLLS.USERID,KCLLS.USERDATE,KCLLS.YN,clzl.YWPM,clzl.ZWPM,
                                     clzl.DWBH,SCBLYY.YWSM,SCBLYY.ZWSM
                              FROM KCLLS 
                              LEFT JOIN clzl on clzl.cldh=kclls.clbh
                              LEFT JOIN SCBLYY on SCBLYY.YYBH=KCLLS.YYBH
                              WHERE KCLLS.LLNO = '" + LLNO + "' ";

            DataTable dt = new DataTable();

            dt.Load(this.m_db.ExecuteReader(cmdTxt));

            this.m_db.Dispose();

            return dt;
        }
    }
}
