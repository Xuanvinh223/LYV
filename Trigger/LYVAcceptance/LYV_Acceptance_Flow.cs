using System;
using System.Xml;
using System.Xml.Linq;
using Ede.Uof.WKF.ExternalUtility;
using Training.LYVAcceptance.UCO;

namespace Training.Trigger.LYVAcceptance
{
    internal class LYV_Acceptance_Flow : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {

            AccpetanceUCO uco = new AccpetanceUCO();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            string SiteCode = applyTask.SiteCode;
            string signStatus = applyTask.SignResult.ToString();

            string LNO = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string ListType = applyTask.Task.CurrentDocument.Fields["ListType"].FieldValue.ToString();
            string AcceptanceDate = applyTask.Task.CurrentDocument.Fields["AcceptanceDate"].FieldValue.ToString();
            string Department = applyTask.Task.CurrentDocument.Fields["Department"].FieldValue.ToString();
            string Applicant = applyTask.Task.CurrentDocument.Fields["Applicant"].FieldValue.ToString();
            string Description = applyTask.Task.CurrentDocument.Fields["Description"].FieldValue.ToString();
            string PropertyNumbers = applyTask.Task.CurrentDocument.Fields["PropertyNumbers"].FieldValue.ToString();


            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Acceptance"].FieldValue.ToString());

            uco.UpdateFormStatus(LNO, SiteCode, signStatus, ListType, AcceptanceDate, Department, Applicant, Description, PropertyNumbers, xE);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
