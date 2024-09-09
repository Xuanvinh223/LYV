using System;
using System.Xml;
using LYV.BoSungNhanSu.UCO;
using System.Xml.Linq;
using Ede.Uof.EIP.SystemInfo;

namespace LYV.Trigger.BoSungNhanSu
{
    public class BSNS_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);

            string LYV = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string NoiNhan = applyTask.Task.CurrentDocument.Fields["NoiNhan"].FieldValue.ToString();
            string UserID = Current.Account;
            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            BoSungNhanSuUCO uco = new BoSungNhanSuUCO();

            uco.InsertData_BeginForm(LYV,NoiNhan, UserID, xE);

            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
