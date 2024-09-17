using System.Data;
using LYN.MaterialExports.PO;

namespace LYN.MaterialExports.UCO
{
    public class MaterialExportsUCO
    {
        MaterialExportSPO m_KCLLSPO = new MaterialExportSPO();

        public DataTable GetKCLLS_X(string LLNO)
        {
            return m_KCLLSPO.GetKCLLS_X(LLNO);
        }
        public DataTable GetKCLLS(string LLNO)
        {
            return m_KCLLSPO.GetKCLLS(LLNO);
        }
    }
}
