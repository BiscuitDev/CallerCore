using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace TestCallerCore.Droid
{
    public class FunctionTypedTask : AbstractEnterpriseAsync<string>
    {
        #region implemented abstract members of AbstractEnterpriseFunction
        public static string NAME = "FunctionTypedTask";
        public override string getName()
        {
            return NAME;
        }

        #endregion

        #region implemented abstract members of AbstractEnterpriseTAsync

        public override System.Threading.Tasks.Task<string> executeAsync(IInfoContext info, FunctionContext context)
        {
            log("Sono partito " + getName());
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            if (context != null && context.getParam(0) != null)
                throw new SystemException("Fails");
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            log("Sono finito " + getName());
            return Task.FromResult("Hi");
        }

        #endregion


    }
}

