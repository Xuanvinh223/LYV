using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LYV.MaterialExport.PO;
using System.Xml.Linq;
using LYV.BusinessTrip.PO;

namespace LYV.MaterialExport.UCO
{
    public  class MaterialExportUCO
    {
        MaterialExportPO m_KCLLPO = new MaterialExportPO();

        public DataTable GetKCLL_X(string UserID, string LLNO, string DepID)
        {
            return m_KCLLPO.GetKCLL_X(UserID, LLNO, DepID);
        }
        public DataTable GetKCLL_X_LLNO(string LLNO)
        {
            return m_KCLLPO.GetKCLL_X_LLNO(LLNO);
        }
        public void UpdateFormStatus(XElement xE, string status, string signStatus, string FNO, string UserID,string DepID, string type)
        {
            m_KCLLPO.UpdateFormStatus(xE, status, signStatus, FNO, UserID, DepID, type);
        }
        public DataTable GetKCLL(string UserID, string LLNO, string DepID)
        {
            return m_KCLLPO.GetKCLL(UserID, LLNO, DepID);
        }
        public DataTable GetKCLL_LLNO(string LLNO)
        {
            return m_KCLLPO.GetKCLL_LLNO(LLNO);
        }
        public DataTable GetWSSignNextInfo(string DOC_NBR, string siteName, string UserGUID)
        {
            return m_KCLLPO.getWSSignNextInfo(DOC_NBR, UserGUID);
        }
        public DataTable GetListME(string LNO, string LLNO, string Table_Name, string Type)
        {
            return m_KCLLPO.GetListME(LNO, LLNO, Table_Name, Type);
        }
    }
}
