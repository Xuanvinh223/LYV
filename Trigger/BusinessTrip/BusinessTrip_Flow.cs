using System;
using System.Xml;
using Ede.Uof.WKF.ExternalUtility;
using LYV.BusinessTrip.UCO;
using System.Xml.Linq;

namespace LYV.Trigger.BusinessTrip
{
    public class BusinessTrip_Flow : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            BusinessTripUCO uco = new BusinessTripUCO();
            string SiteCode = applyTask.SiteCode;
            string signStatus = applyTask.SignResult.ToString();

            string LYV = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string EmployeeType = applyTask.Task.CurrentDocument.Fields["EmployeeType"].FieldValue.ToString();
            string RequestDate = applyTask.Task.CurrentDocument.Fields["RequestDate"].FieldValue.ToString();
            string Type = applyTask.Task.CurrentDocument.Fields["Type"].FieldValue.ToString();
            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            uco.UpdateFormStatus(LYV, EmployeeType, RequestDate, Type, SiteCode, signStatus, xE);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
