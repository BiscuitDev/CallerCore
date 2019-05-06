using System;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class FunctionRegular : AbstractEnterpriseRegular
    {
        public static string NAME = "FunctionRegular";

        #region implemented abstract members of AbstractEnterpriseRegular

        public override object execute(IInfoContext info, FunctionContext context)
        {
            log("Start Regular function ");
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            if (context != null && context.getParam(0) != null)
                throw new SystemException("Fails");
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            log("Ended Regular function ");
            return "Hi";

        }

        #endregion

        public override string getName()
        {
            return NAME;
        }

    }
}

