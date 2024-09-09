using System;
using System.Xml;
using Training.Leave.UCO;
using System.Xml.Linq;


namespace Training.Trigger.Leave
{
    public class LYV_Leave_Start : Ede.Uof.WKF.ExternalUtility.ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(Ede.Uof.WKF.ExternalUtility.ApplyTask applyTask)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(applyTask.CurrentDocXML);

            string LNO = applyTask.Task.CurrentDocument.Fields["LYV"].FieldValue.ToString();
            string UserID = applyTask.Task.CurrentDocument.Fields["Applicant"].FieldValue.ToString();
            UserID = UserID.Substring(UserID.IndexOf('(') + 4, UserID.IndexOf(')') - UserID.IndexOf('(') - 4);
            string Factory = applyTask.Task.CurrentDocument.Fields["Department"].FieldValue.ToString();

            XElement xE = XElement.Parse(applyTask.Task.CurrentDocument.Fields["Form"].FieldValue.ToString());

            LeaveUCO uco = new LeaveUCO();

            /*            string docNbr = applyTask.FormNumber;

                        string singerGUID = uco.retrieveActualSigner(applyTask.SiteID, applyTask.TaskId, docNbr);
                        UserUCO userUco = new UserUCO();
                        EBUser ebuser = userUco.GetEBUser(singerGUID);
                        string signerAccount = ebuser.Account;*/

            uco.InsertLeaveFormData(LNO, UserID, Factory, xE);

            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
