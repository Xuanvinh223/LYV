using System;
using System.Xml;
using LYV.Supplier.UCO;

namespace LYV.Trigger.Supplier
{
    public class Supplier_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            string USERID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
            USERID = USERID.Substring(USERID.IndexOf('(') + 4, USERID.IndexOf(')') - USERID.IndexOf('(') - 4);
            SupplierUCO uco = new SupplierUCO();
            uco.InsertTaskData(applyTask, USERID);
            return "";
        }

     
        public void OnError(Exception errorException)
        {

        }
    }
}
