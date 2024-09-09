using Ede.Uof.WKF.ExternalUtility;
using System;
using LYV.Supplier.UCO;

namespace LYV.Trigger.Supplier
{
    public class Supplier_End : ICallbackTriggerPlugin
    {
        public void Finally()
        {
            //  throw new NotImplementedException();
        }

        public string GetFormResult(ApplyTask applyTask)
        {
     
            SupplierUCO uco = new SupplierUCO();
            string RNO = applyTask.FormNumber;
            string signStatus = applyTask.FormResult.ToString();
            uco.UpdateFormResult(RNO, signStatus);
            return "";
        }

        public void OnError(Exception errorException)
        {
            //  throw new NotImplementedException();
        }
    }
}
