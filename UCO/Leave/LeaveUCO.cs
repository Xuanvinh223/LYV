using System.Data;
using System.Xml.Linq;
using Training.LYVLeave.PO;

namespace Training.Leave.UCO
{
    public class LeaveUCO
    {
        LeavePO m_LeavePO = new LeavePO();

        public DataTable GetUser()
        {
            return m_LeavePO.GetUser();
        }

        public DataTable GetDep()
        {
            return m_LeavePO.GetDep();
        }

        public string GetEmployee(string UserID)
        {
            return m_LeavePO.GetEmployee(UserID);
        }

        public string GetLeaveData(string ID, string Year)
        {
            return m_LeavePO.GetLeaveData(ID, Year);
        }

        public DataTable ST_NHANVIENNGHIPHEP(string UserID)
        {
            return m_LeavePO.ST_NHANVIENNGHIPHEP(UserID);
        }

        public string CheckLeaveDate(string USERID, string StartDate, string EndDate, string type)
        {
            return m_LeavePO.CheckLeaveDate(USERID, StartDate, EndDate, type);
        }

        //public string Check_KhoaSo(string USERID, string StartDate)
        //{
        //    return m_LeavePO.Check_KhoaSo(USERID, StartDate);
        //}
        //public string Check_KhoaSo_All(string LNO)
        //{
        //    return m_LeavePO.Check_KhoaSo_All(LNO);
        //}

        public void InsertLeaveFormData(string LNO, string UserID, string Factory, XElement xE)
        {
            m_LeavePO.InsertLeaveFormData(LNO, UserID, Factory, xE);
        }
        public void UpdateFormStatus(string LNO, string SiteCode, string signStatus, string Factory, XElement xE)
        {
            m_LeavePO.UpdateFormStatus(LNO, SiteCode, signStatus, Factory, xE);
        }

        //public string CheckHRMData(string LNO)
        //{
        //    return m_LeavePO.CheckHRMData(LNO);
        //}
        public void WriteHRM(string ApplyDate, XElement xE)
        {
            m_LeavePO.WriteHRM(ApplyDate, xE);
        }

        public void UpdateFormResult(string LNO, string formResult)
        {
            m_LeavePO.UpdateFormResult(LNO, formResult);
        }

        public string retrieveActualSigner(string siteID, string taskID, string docNbr, string signStatus = "S")
        {
            return m_LeavePO.retrieveActualSigner(siteID, taskID, docNbr, signStatus);
        }

        public DataTable GetListLeave(string D_STEP_DESC, string flowflag, string DV_MA_, string LNO, string LeaverID, string StartDate, string EndDate, string Documents, string Factory, string Account)
        {
            return m_LeavePO.GetListLeave(D_STEP_DESC, flowflag, DV_MA_, LNO, LeaverID, StartDate, EndDate, Documents, Factory, Account);
        }

        public DataTable GetWSSignNextInfo(string DOC_NBR, string siteName, string UserGUID)
        {
            return m_LeavePO.getWSSignNextInfo(DOC_NBR, UserGUID);
        }
        public void UpdateWFLeave(string flag, string Where)
        {
            m_LeavePO.UpdateWFLeave(flag, Where);
        }
        public void DisableFlowflag(string LNO)
        {
            m_LeavePO.DisableFlowflag(LNO);
        }
        public string getFlowflag(string LNO)
        {
            return m_LeavePO.getFlowflag(LNO);
        }
    }
}
