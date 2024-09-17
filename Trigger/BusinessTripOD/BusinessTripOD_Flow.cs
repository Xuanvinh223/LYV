using System;
using System.Xml;
using Ede.Uof.WKF.ExternalUtility;
using LYV.BusinessTripOD.UCO;
using System.Xml.Linq;

namespace LYV.Trigger.BusinessTripOD
{
    public class BusinessTripOD_Flow : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            string SiteCode = applyTask.SiteCode;
            string signStatus = applyTask.SignResult.ToString();

            string LYV = applyTask.Task.CurrentDocument.Fields["LYV_TSS"].FieldValue.ToString();
            string Area = applyTask.Task.CurrentDocument.Fields["Area"].FieldValue.ToString();
            string MaPhieu = applyTask.Task.CurrentDocument.Fields["MaPhieu"].FieldValue.ToString();
            string EmployeeType = applyTask.Task.CurrentDocument.Fields["EmployeeType"].FieldValue.ToString();
            string DepID = applyTask.Task.CurrentDocument.Fields["DepID"].FieldValue.ToString();
            string UserID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(') + 4, UserID.IndexOf(')') - UserID.IndexOf('(') - 4);
            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            BusinessTripODUCO uco = new BusinessTripODUCO();

            uco.UpdateFormStatus( LYV, Area, MaPhieu, EmployeeType,  DepID,  UserID,  SiteCode,  signStatus,  xE);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
