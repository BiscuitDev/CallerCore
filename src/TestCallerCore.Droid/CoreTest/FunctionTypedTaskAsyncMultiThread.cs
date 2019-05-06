using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace TestCallerCore.Droid
{
    public class FunctionTypedTaskAsyncMultiThread : AbstractEnterpriseAsync<int>
    {
        #region implemented abstract members of AbstractEnterpriseFunction
        public static string NAME = "FunctionTypedTaskAsyncMultiThread";
        public override string getName()
        {
            return NAME;
        }

        #endregion

        #region implemented abstract members of AbstractEnterpriseTAsync

        public override async System.Threading.Tasks.Task<int> executeAsync(IInfoContext info, FunctionContext context)
        {
            var myparam = context.getIntParam("test").Value;
            log("Sono partito " + getName() + " Param " + myparam);
            await Task.Delay(500);
            if (context.getIntParam("test").Value % 30 == 0)
                throw new SystemException($"Error {myparam}");
            log("Sono finito " + getName() + " Param " + myparam);
            return await Task.FromResult(myparam);
        }

        #endregion


    }
}