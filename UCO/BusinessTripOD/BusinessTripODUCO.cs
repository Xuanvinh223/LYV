using System.Data;
using LYV.BusinessTripOD.PO;
using System.Xml.Linq;
using LYV.BusinessTrip.PO;

namespace LYV.BusinessTripOD.UCO
{
    public  class BusinessTripODUCO
    {
        BusinessTripODPO m_BusinessTripODPO = new BusinessTripODPO();

        public string GetLEV(string UserID, string groupID)
        {
            return m_BusinessTripODPO.GetLEV(UserID, groupID);
        }
        public string GetMaPhieu()
        {
            return m_BusinessTripODPO.GetMaPhieu();
        }
        public DataTable GetDep()
        {
            return m_BusinessTripODPO.GetDep();
        }
        public string GetEmployee(string UserID)
        {
            return m_BusinessTripODPO.GetEmployee(UserID);
        }
        public void InsertBusinessTripODFormData(string LYV, string EmployeeType, string RequestDate, string Type, string DepID, string UserID, XElement xE)
        {
            m_BusinessTripODPO.InsertBusinessTripODFormData(LYV,EmployeeType,RequestDate,Type,DepID,UserID,xE);
        }
        public void UpdateFormStatus(string LYV, string EmployeeType, string RequestDate, string Type, string DepID, string UserID, string SiteCode, string signStatus, XElement xE)
        {
            m_BusinessTripODPO.UpdateFormStatus( LYV,  EmployeeType,  RequestDate,  Type,  DepID,  UserID,  SiteCode,  signStatus,  xE);
        }

        public void UpdateFormResult(string LNO, string formResult)
        {
            m_BusinessTripODPO.UpdateFormResult(LNO, formResult);
        }
        public DataTable GetWSSignNextInfo(string DOC_NBR, string siteName, string UserGUID)
        {
            return m_BusinessTripODPO.getWSSignNextInfo(DOC_NBR, UserGUID);
        }
        public DataTable GetListBT(string LNO, string Name, string Name_ID, string BTime1, string BTime2)
        {
            return m_BusinessTripODPO.GetListBT(LNO, Name, Name_ID, BTime1, BTime2);
        }

        public string getFlowflag(string LYV)
        {
            return m_BusinessTripODPO.getFlowflag(LYV);
        }
    }
}
