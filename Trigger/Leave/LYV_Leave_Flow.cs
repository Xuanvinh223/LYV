using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ede.Uof.WKF.ExternalUtility;
using Training.Leave.UCO;
using System.Xml.Linq;
using Ede.Uof.EIP.Organization.Util;

namespace Training.Trigger.Leave
{
    public class LYV_Leave_Flow : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {
            //<Form formVersionId="30d33f52-802f-49b3-933e-f93a9c5d61cb">
            //  <FormFieldValue>
            //    <FieldItem fieldId="NO" fieldValue="" realValue="" />
            //    <FieldItem fieldId="A01" fieldValue="xxx" realValue="" fillerName="黃建龍" fillerUserGuid="07a00c72-270e-403e-b9df-20b530ba45e8" fillerAccount="Howard_Huang" fillSiteId="" />
            //    <FieldItem fieldId="A02" fieldValue="3" realValue="" fillerName="黃建龍" fillerUserGuid="07a00c72-270e-403e-b9df-20b530ba45e8" fillerAccount="Howard_Huang" fillSiteId="" />
            //    <FieldItem fieldId="A03" fieldValue="4" realValue="" fillerName="黃建龍" fillerUserGuid="07a00c72-270e-403e-b9df-20b530ba45e8" fillerAccount="Howard_Huang" fillSiteId="" />
            //    <FieldItem fieldId="A04" fieldValue="222" realValue="" fillerName="黃建龍" fillerUserGuid="07a00c72-270e-403e-b9df-20b530ba45e8" fillerAccount="Howard_Huang" fillSiteId="" />
            //  </FormFieldValue>
            //</Form>

            //applyTask.Task.Applicant
            //applyTask.Task.ApplicantGroupId
            //applyTask.Task.Agent_User
            //applyTask.SiteCode
            //applyTask.NextSiteCode
            // applyTask.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            string SiteCode = applyTask.SiteCode;
            string signStatus = applyTask.SignResult.ToString();

            string LNO = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string Factory = applyTask.Task.CurrentDocument.Fields["Department"].FieldValue.ToString();
            string ApplyDate = DateTime.Parse(applyTask.Task.BeginTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());
            string Type = xE.Attribute("Type").Value;
            string LeaveDays = xE.Attribute("LeaveDays").Value;
            string result = "0";

            if (Type == "P" || Type == "No1")
            {
                result = "1";
            }
            else
            {
                if (Convert.ToInt32(LeaveDays) >= 10)
                {
                    result = "2";
                }
                else
                {
                    result = "1";
                }
            }

            LeaveUCO uco = new LeaveUCO();

            uco.UpdateFormStatus(LNO, SiteCode, signStatus, Factory, xE);

            if ((result == "1" && SiteCode == "SE1" && signStatus == "Approve") || (result == "2" && SiteCode == "SE2" && signStatus == "Approve"))
            {
                uco.WriteHRM(ApplyDate, xE);
            }
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
