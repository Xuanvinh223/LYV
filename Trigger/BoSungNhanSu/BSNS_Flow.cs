using System;
using System.Xml;
using System.Xml.Linq;
using Ede.Uof.WKF.ExternalUtility;
using LYV.BoSungNhanSu.UCO;

namespace LYV.Trigger.BoSungNhanSu
{
    public class BSNS_Flow : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {
           
            BoSungNhanSuUCO uco = new BoSungNhanSuUCO();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            string SiteCode = applyTask.SiteCode;
            string signStatus = applyTask.SignResult.ToString();

            string LYV = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string NoiNhan = applyTask.Task.CurrentDocument.Fields["NoiNhan"].FieldValue.ToString();

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            uco.UpdateFormStatus( LYV, SiteCode, signStatus, NoiNhan,  xE);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
