using System;
using System.Xml;
using Training.Out.UCO;
using System.Xml.Linq;

namespace Training.Trigger.Out
{
    public class LYV_Out_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
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
            string DepID = applyTask.Task.CurrentDocument.Fields["DepID"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(') + 4, UserID.IndexOf(')') - UserID.IndexOf('(') - 4);

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            OutUCO uco = new OutUCO();

            uco.InsertOutData(LNO, UserID, DepID, xE);

            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
