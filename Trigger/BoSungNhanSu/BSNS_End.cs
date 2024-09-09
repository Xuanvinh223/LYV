using System;
using Ede.Uof.WKF.ExternalUtility;
using LYV.BoSungNhanSu.UCO;

namespace LYV.Trigger.BoSungNhanSu
{
    public class BSNS_End : ICallbackTriggerPlugin
    {
        public void Finally()
        {

        }

        public string GetFormResult(ApplyTask applyTask)
        {

            BoSungNhanSuUCO uco = new BoSungNhanSuUCO();
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
