using System.Data;
using LYV.BusinessTripReport.PO;
using System.Xml.Linq;

namespace LYV.BusinessTripReport.UCO
{
    public  class BusinessTripReportUCO
    {
        BusinessTripReportPO m_BusinessTripReportPO = new BusinessTripReportPO();

        public DataTable GetListBT(string LYV, string Name, string Name_ID, string BTime1, string BTime2)
        {
            return m_BusinessTripReportPO.GetListBT(LYV, Name, Name_ID, BTime1, BTime2);
        }
        public DataTable GetBusinessTripReport_BLYV(string BLYV)
        {
            return m_BusinessTripReportPO.GetBusinessTripReport_BLYV(BLYV);
        }
        public void InsertBusinessTripReportData(string LYV, string UserID, string Department, string Date, string Date1, string Name, string Destination, string Description, XElement xE)
        {
            m_BusinessTripReportPO.InsertBusinessTripReportData(LYV, UserID, Department, Date, Date1, Name, Destination, Description, xE);
        }
        public string GetBusinessTripReport(string LYV)
        {
            return m_BusinessTripReportPO.GetBusinessTripReport(LYV);
        }
        public void UpdateCancelReason(string LYV, string CancelReason)
        {
            m_BusinessTripReportPO.UpdateCancelReason(LYV, CancelReason);
        }
        public void Confirm(string LYV, string CFMID)
        {
            m_BusinessTripReportPO.Confirm(LYV, CFMID);
        }
    }
}
