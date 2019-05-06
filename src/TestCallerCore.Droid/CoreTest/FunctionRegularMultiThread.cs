using System;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class FunctionRegularMultiThread : AbstractEnterpriseRegular
    {
        public static string NAME = "FunctionRegularMultiThread";

        #region implemented abstract members of AbstractEnterpriseRegular

        public override object execute(IInfoContext info, FunctionContext context)
        {
            var myparam = context.getIntParam("test").Value;
            log("Start Regular function " + getName() + " Param " + myparam);
            new System.Threading.ManualResetEvent(false).WaitOne(500);
            if (context.getIntParam("test").Value % 30 == 0)
                throw new SystemException($"Error {myparam}");
            log("Ended Regular function " + getName() + " Param " + myparam);
            return myparam;
        }

        #endregion

        public override string getName()
        {
            return NAME;
        }

    }
}