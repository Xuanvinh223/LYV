using System;
using System.Xml;
using Ede.Uof.WKF.ExternalUtility;
using System.Xml.Linq;
using LYV.MaterialExport.UCO;

namespace LYV.Trigger.MaterialExport
{
    public class MaterialExport_Flow : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {

            //applyTask.Task.Applicant
            //applyTask.Task.ApplicantGroupId
            //applyTask.Task.Agent_User
            //applyTask.SiteCode
            //applyTask.NextSiteCode
            // applyTask.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            string SiteCode = applyTask.SiteCode;
            string signStatus = applyTask.SignResult.ToString();

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            MaterialExportUCO uco = new MaterialExportUCO();

            uco.UpdateFormStatus(xE, SiteCode, signStatus, "", "","", "XXXXXXXXXX");
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
