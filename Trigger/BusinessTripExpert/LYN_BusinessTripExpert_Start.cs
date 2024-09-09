using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Training.Data;
using System.Xml.Linq;
using Ede.Uof.WKF.ExternalUtility;
using Training.BusinessTripExpert.UCO;

namespace Training.Trigger.BusinessTripExpert
{
    public class LYN_BusinessTripExpert_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            BusinessTripExpertUCO uco = new BusinessTripExpertUCO();
            TaskUtility taskUtility = new TaskUtility();

            xmlDoc.LoadXml(applyTask.CurrentDocXML);

            string LNO = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string UserID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(') + 4, UserID.IndexOf(')') - UserID.IndexOf('(') - 4);
            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            //string taskID = applyTask.TaskId;
            //string expert = xE.Attribute("expert").Value;
            //taskUtility.UpdateTaskContent(false, taskID, "expert", expert, "");

            uco.InsertBusinessTripExpertFormData(LNO, UserID, xE);

            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
