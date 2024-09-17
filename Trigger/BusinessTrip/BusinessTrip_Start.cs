using LYV.BusinessTrip.UCO;
using System;
using System.Xml;
using System.Xml.Linq;
namespace LYV.Trigger.BusinessTrip
{
    public class BusinessTrip_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            {
                BusinessTripUCO uco = new BusinessTripUCO();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(applyTask.CurrentDocXML);

                string LYV = applyTask.Task.CurrentDocument.Fields["LYV_TDC"].FieldValue.ToString();
                string Area = applyTask.Task.CurrentDocument.Fields["Area"].FieldValue.ToString();
                string MaPhieu = applyTask.Task.CurrentDocument.Fields["MaPhieu"].FieldValue.ToString();
                string EmployeeType = applyTask.Task.CurrentDocument.Fields["EmployeeType"].FieldValue.ToString();
                string DepID = applyTask.Task.CurrentDocument.Fields["DepID"].FieldValue.ToString();
                string UserID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
                UserID = UserID.Substring(UserID.IndexOf('(') + 4, UserID.IndexOf(')') - UserID.IndexOf('(') - 4);
                XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

                uco.InsertBusinessTripFormData(LYV, Area, MaPhieu, EmployeeType, DepID, UserID, xE);
                return "";
            }
        }
        public void OnError(Exception errorException)
        {

        }
    }
}
