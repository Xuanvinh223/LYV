using System.Data;
using System.Xml.Linq;
using Training.LYVOut.PO;

namespace Training.Out.UCO
{
    public class OutUCO
    {
        OutPO m_OutPO = new OutPO();

        public string GetEmployee(string UserID)
        {
            return m_OutPO.GetEmployee(UserID);
        }
        public void InsertOutData(string LNO, string UserID, string DepID, XElement xE)
        {
            m_OutPO.InsertOutData(LNO, UserID, DepID, xE);
        }
        public string GetOut(string LNO)
        {
            return m_OutPO.GetOut(LNO);
        }
        public void UpdateCancelReason(string LNO, string CancelReason)
        {
            m_OutPO.UpdateCancelReason(LNO, CancelReason);
        }
        public DataTable GetListOut(string D_STEP_DESC, string Type, string LNO, string OutID, string LeaveTime, string ReturnTime, string UserDate1, string UserDate2, string OutCheck, string UserID)
        {
            return m_OutPO.GetListOut(D_STEP_DESC, Type, LNO, OutID, LeaveTime, ReturnTime, UserDate1, UserDate2, OutCheck, UserID);
        }
    }
}
