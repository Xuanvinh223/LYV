using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ede.Uof.WKF.ExternalUtility;
using Training.CapacityAssessmentCB.UCO;
using System.Xml.Linq;
using Ede.Uof.EIP.Organization.Util;

namespace Training.Trigger.CapacityAssessmentCB
{
    public class LYN_CapacityAssessmentCB_Flow : ICallbackTriggerPlugin
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

            string CNO = applyTask.Task.CurrentDocument.Fields["CNO"].FieldValue.ToString();
            string DepID = applyTask.Task.CurrentDocument.Fields["DepID"].FieldValue.ToString();
            string UserID = applyTask.Task.CurrentDocument.Fields["UserID"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(') + 4, UserID.IndexOf(')') - UserID.IndexOf('(') - 4);

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            CapacityAssessmentCBUCO uco = new CapacityAssessmentCBUCO();

            uco.UpdateFormStatus(CNO, UserID, DepID, SiteCode, signStatus, xE);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
