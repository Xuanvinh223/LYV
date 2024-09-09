using System.Data;
using Ede.Uof.WKF.ExternalUtility;
using LYV.Supplier.PO;


namespace LYV.Supplier.UCO
{
    public class SupplierUCO
    {
        SupplierPO m_SupplierPO = new SupplierPO();

        public void InsertTaskData(ApplyTask applyTask,string USERID)
        {
            m_SupplierPO.InsertTaskData(applyTask, USERID);
        }
        public void UpdateFormStatus(ApplyTask applyTask)
        {
            m_SupplierPO.UpdateFormStatus( applyTask);
        }
        public void UpdateFormResult(string RNO, string signStatus)
        {
            m_SupplierPO.UpdateFormResult(RNO, signStatus);
        }
        public DataTable GetWSSignNextInfo(string DOC_NBR, string siteName, string UserGUID)
        {
            return m_SupplierPO.getWSSignNextInfo(DOC_NBR, UserGUID);
        }
        public DataTable GetListSUP(string Type, string SupplierID, string SupplierName)
        {
            return m_SupplierPO.GetListSUP(Type, SupplierID, SupplierName);
        }
    }
}
