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
            string Area = applyTask.Task.CurrentDocument.Fields["Area"].FieldValue.ToString();
            string MaPhieu = applyTask.Task.CurrentDocument.Fields["MaPhieu"].FieldValue.ToString();
            string EmployeeType = applyTask.Task.CurrentDocument.Fields["EmployeeType"].FieldValue.ToString();
            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            uco.UpdateFormStatus(LYV, Area, MaPhieu, EmployeeType, SiteCode, signStatus, xE);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
