using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ede.Uof.WKF.ExternalUtility;
using Training.BusinessTripExpert.UCO;
using System.Xml.Linq;
using Ede.Uof.EIP.Organization.Util;

namespace Training.Trigger.BusinessTripExpert
{
    public class LYN_BusinessTripExpert_Flow : ICallbackTriggerPlugin
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

            string LNO = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            //string MaPhieu = applyTask.Task.CurrentDocument.Fields["MaPhieu"].FieldValue.ToString();

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            BusinessTripExpertUCO uco = new BusinessTripExpertUCO();

            uco.UpdateFormStatus(LNO, SiteCode, signStatus, "MaPhieu", xE);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
