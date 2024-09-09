using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Training.BusinessTripExpert.PO;
using Training.Data;
using System.Xml.Linq;

namespace Training.BusinessTripExpert.UCO
{
    public  class BusinessTripExpertUCO
    {
        BusinessTripExpertPO m_BusinessTripExpertPO = new BusinessTripExpertPO();

        public string GetLEV(string UserID, string groupID)
        {
            return m_BusinessTripExpertPO.GetLEV(UserID, groupID);
        }
        public DataTable GetDep()
        {
            return m_BusinessTripExpertPO.GetDep();
        }
        public string GetEmployee(string UserID)
        {
            return m_BusinessTripExpertPO.GetEmployee(UserID);
        }
        public void InsertBusinessTripExpertFormData(string LNO, string UserID, XElement xE)
        {
            m_BusinessTripExpertPO.InsertBusinessTripExpertFormData(LNO, UserID, xE);
        }
        public void UpdateFormStatus(string LNO, string SiteCode, string signStatus, string MaPhieu, XElement xE)
        {
            m_BusinessTripExpertPO.UpdateFormStatus(LNO, SiteCode, signStatus, MaPhieu, xE);
        }

        public void UpdateFormResult(string LNO, string formResult)
        {
            m_BusinessTripExpertPO.UpdateFormResult(LNO, formResult);
        }
        public DataTable GetWSSignNextInfo(string DOC_NBR, string siteName, string UserGUID)
        {
            return m_BusinessTripExpertPO.getWSSignNextInfo(DOC_NBR, UserGUID);
        }
        public DataTable GetListBT(string LNO, string Name, string Name_ID, string BTime1, string BTime2, string expert)
        {
            return m_BusinessTripExpertPO.GetListBT(LNO, Name, Name_ID, BTime1, BTime2, expert);
        }
    }
}
