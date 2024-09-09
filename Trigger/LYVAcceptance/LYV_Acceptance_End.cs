using Ede.Uof.WKF.ExternalUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.LYVAcceptance.UCO;
namespace Training.Trigger.LYVAcceptance
{
    internal class LYV_Acceptance_End : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {

            AccpetanceUCO uco = new AccpetanceUCO();
            string LNO = applyTask.FormNumber;
            string signStatus = applyTask.FormResult.ToString();

            uco.UpdateFormResult(LNO, signStatus);
            return "";
        }

        public void OnError(Exception errorException)
        {

        }
    }
}
