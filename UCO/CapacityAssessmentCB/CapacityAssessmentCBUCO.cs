using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Training.CapacityAssessmentCB.PO;
using Training.Data;
using System.Xml.Linq;


namespace Training.CapacityAssessmentCB.UCO
{
    public  class CapacityAssessmentCBUCO
    {
        CapacityAssessmentCBPO m_CapacityAssessmentCBPO = new CapacityAssessmentCBPO();

        public string GetEmployee(string UserID)
        {
            return m_CapacityAssessmentCBPO.GetEmployee(UserID);
        }
        public string GetLEV(string UserID, string groupID)
        {
            return m_CapacityAssessmentCBPO.GetLEV(UserID, groupID);
        }
        public void InsertCapacityAssessmentCBData(string CNO, string UserID, string DepID, XElement xE)
        {
            m_CapacityAssessmentCBPO.InsertCapacityAssessmentCBData(CNO, UserID, DepID, xE);
        }

        public void UpdateFormStatus(string CNO, string UserID, string DepID, string SiteCode, string signStatus, XElement xE)
        {
            m_CapacityAssessmentCBPO.UpdateFormStatus(CNO, UserID, DepID, SiteCode, signStatus, xE);
        }
        public void UpdateFormResult(string CNO, string formResult)
        {
            m_CapacityAssessmentCBPO.UpdateFormResult(CNO, formResult);
        }
        public string retrieveActualSigner(string siteID, string taskID, string docNbr, string signStatus = "S")
        {
            return m_CapacityAssessmentCBPO.retrieveActualSigner(siteID, taskID, docNbr, signStatus);
        }
        public DataTable GetListPR(string Factory, string Department, string Year, string Month1, string Month2, string flowflag, string CFMID)
        {
            return m_CapacityAssessmentCBPO.GetListPR(Factory, Department, Year, Month1, Month2, flowflag, CFMID);
        }
    }
}
