using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Training.Data;
using Training.LYVAcceptance.UCO;

namespace Training.Trigger.LYVAcceptance
{
    internal class LYV_Acceptance_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);
            // applyTask.Task.CurrentSite.SiteId
            // applyTask.Task.TaskDs.TB_WKF_TASK_NODE.ACTUAL_SIGNERColumn
            DemoDataSet ds = new DemoDataSet();
            DemoDataSet.TB_DEMO_DLL_FORMRow dr = ds.TB_DEMO_DLL_FORM.NewTB_DEMO_DLL_FORMRow();
            //dr.ID = Guid.NewGuid().ToString();
            string LNO = applyTask.FormNumber;
            string ListType = applyTask.Task.CurrentDocument.Fields["ListType"].FieldValue.ToString();
            string AcceptanceDate = applyTask.Task.CurrentDocument.Fields["AcceptanceDate"].FieldValue.ToString();
            string Department = applyTask.Task.CurrentDocument.Fields["Department"].FieldValue.ToString();
            string Applicant = applyTask.Task.CurrentDocument.Fields["Applicant"].FieldValue.ToString();
            string Description = applyTask.Task.CurrentDocument.Fields["Description"].FieldValue.ToString();
            string PropertyNumbers = applyTask.Task.CurrentDocument.Fields["PropertyNumbers"].FieldValue.ToString();

            string SIGN_STATIS = applyTask.SiteCode;
            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Acceptance"].FieldValue.ToString());
            AccpetanceUCO uco = new AccpetanceUCO();

            uco.InsertData_BeginForm(LNO, ListType, AcceptanceDate, Department, Applicant, Description, PropertyNumbers, xE);


            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
