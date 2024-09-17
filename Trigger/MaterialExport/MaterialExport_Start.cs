using System;
using System.Xml;
using LYV.MaterialExport.UCO;
using System.Xml.Linq;

namespace LYV.Trigger.MaterialExport
{
    public class MaterialExport_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            MaterialExportUCO uco = new MaterialExportUCO();
            
            string status = "N";
            string FNO = applyTask.FormNumber;
            string UserID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
            string DepID = applyTask.Task.CurrentDocument.Fields["DepID"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(') + 4, UserID.IndexOf(')') - UserID.IndexOf('(') - 4);

            uco.UpdateFormStatus(xE, status, "", FNO, UserID, DepID, "XXXXXXXXXX");

            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
