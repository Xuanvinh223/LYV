using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Training.Data;
using LYV.BusinessTripReport.UCO;
using System.Xml.Linq;
using Ede.Uof.EIP.Organization.Util;

namespace LYV.Trigger.BusinessTripReport
{
    public class BusinessTripReport_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);

            string LNO = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string UserID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
            string Department = applyTask.Task.CurrentDocument.Fields["DepID"].FieldValue.ToString();
            string Date = applyTask.Task.CurrentDocument.Fields["Date"].FieldValue.ToString();
            string Date1 = applyTask.Task.CurrentDocument.Fields["Date1"].FieldValue.ToString();
            string Name = applyTask.Task.CurrentDocument.Fields["Name"].FieldValue.ToString();
            string Destination = applyTask.Task.CurrentDocument.Fields["Destination"].FieldValue.ToString();
            string Description = applyTask.Task.CurrentDocument.Fields["Description"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(')+4, UserID.IndexOf(')')- UserID.IndexOf('(')-4);

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            BusinessTripReportUCO uco = new BusinessTripReportUCO();

            uco.InsertBusinessTripReportData(LNO, UserID, Department, Date, Date1, Name, Destination, Description, xE);

            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
