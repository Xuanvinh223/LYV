using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Training.Data;
using Training.CapacityAssessmentCB.UCO;
using System.Xml.Linq;
using Ede.Uof.EIP.Organization.Util;

namespace Training.Trigger.CapacityAssessmentCB
{
    public class LYN_CapacityAssessmentCB_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);

            string CNO = applyTask.Task.CurrentDocument.Fields["CNO"].FieldValue.ToString();
            string DepID = applyTask.Task.CurrentDocument.Fields["DepID"].FieldValue.ToString();
            string UserID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(')+4, UserID.IndexOf(')')- UserID.IndexOf('(')-4);

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            CapacityAssessmentCBUCO uco = new CapacityAssessmentCBUCO();

            uco.InsertCapacityAssessmentCBData(CNO, UserID, DepID, xE);

            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
