using LYV.BoSungNhanSu.PO;
using System.Data;
using System.Xml.Linq;

namespace LYV.BoSungNhanSu.UCO
{
    public  class BoSungNhanSuUCO
    {
        BoSungNhanSuPO BoSungNhanSuPO = new BoSungNhanSuPO();

        public void InsertData_BeginForm(string LNO, string NoiNhan, string UserID, XElement xE)
        {
            BoSungNhanSuPO.InsertData_BeginForm( LNO, NoiNhan, UserID, xE);
        }
        public void UpdateFormStatus(string LNO, string SiteCode, string signStatus, string NoiNhan, XElement xE)
        {
            BoSungNhanSuPO.UpdateFormStatus( LNO, SiteCode, signStatus, NoiNhan, xE);
        }
        public void UpdateFormResult(string LNO, string formResult)
        {
            BoSungNhanSuPO.UpdateFormResult(LNO, formResult);
        }
        public string GetEmployee(string UserID)
        {
            return BoSungNhanSuPO.GetEmployee(UserID);
        }
        public DataTable GetGridView_Close(string LNO, string MaPhieu, string  NoiNhan, string LyDoBS, string Sdate, string Edate,string flowflag)
        {
            return BoSungNhanSuPO.GetGridView_Close(LNO, MaPhieu,NoiNhan, LyDoBS, Sdate, Edate, flowflag);
        }

    }
}
