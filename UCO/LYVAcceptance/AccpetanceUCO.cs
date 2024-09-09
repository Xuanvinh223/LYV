
using System.Data;
using Training.LYVAcceptance.PO;
using System.Xml.Linq;

namespace Training.LYVAcceptance.UCO
{
    public class AccpetanceUCO
    {
        LYV_AcceptancePO AcceptancePO = new LYV_AcceptancePO();


        public DataTable GetData(string RKNO)
        {
            return AcceptancePO.GetData(RKNO);
        }

        public void InsertData_BeginForm(string LNO, string ListType, string AcceptanceDate, string Department, string Applicant, string Description, string PropertyNumbers, XElement xE)
        {
            AcceptancePO.InsertData_BeginForm(LNO, ListType, AcceptanceDate, Department, Applicant, Description, PropertyNumbers, xE);
        }

        public void UpdateFormStatus(string LNO, string SiteCode, string signStatus, string ListType, string AcceptanceDate, string Department, string Applicant, string Description, string PropertyNumbers, XElement xE)
        {
            AcceptancePO.UpdateFormStatus(LNO, SiteCode, signStatus, ListType, AcceptanceDate, Department, Applicant, Description, PropertyNumbers, xE);
        }

        public string getFlowflag(string LNO)
        {
            return AcceptancePO.getFlowflag(LNO);
        }

        public void UpdateFormResult(string LNO, string formResult)
        {
            AcceptancePO.UpdateFormResult(LNO, formResult);
        }

        public DataTable GetGridView_Close(string LNO, string RKNO, string ListType, string Sdate, string Edate)
        {
            return AcceptancePO.GetGridView_Close(LNO, RKNO, ListType, Sdate, Edate);
        }
    }
}
