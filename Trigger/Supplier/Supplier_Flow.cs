using Ede.Uof.WKF.ExternalUtility;
using System;
using System.Xml;
using LYV.Supplier.UCO;

namespace LYV.Trigger.Supplier
{
    public class Supplier_Flow : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            SupplierUCO uco = new SupplierUCO();
            uco.UpdateFormStatus(applyTask);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
