using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ede.Uof.WKF.ExternalUtility;
using LYV.BusinessTripOD.UCO;

namespace LYV.Trigger.BusinessTripOD
{
    public class BusinessTripOD_End : ICallbackTriggerPlugin
    {
        public void Finally()
        {
            //  throw new NotImplementedException();
        }

        public string GetFormResult(ApplyTask applyTask)
        {
            // throw new NotImplementedException();

            BusinessTripODUCO uco = new BusinessTripODUCO();
            string LNO = applyTask.FormNumber;
            string signStatus = applyTask.FormResult.ToString();

            uco.UpdateFormResult(LNO, signStatus);
            return "";
        }

        public void OnError(Exception errorException)
        {
            //  throw new NotImplementedException();
        }
    }
}
