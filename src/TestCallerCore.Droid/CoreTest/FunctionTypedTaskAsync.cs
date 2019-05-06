using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace TestCallerCore.Droid
{
    public class FunctionTypedTaskAsync : AbstractEnterpriseAsync<string>
    {
        #region implemented abstract members of AbstractEnterpriseFunction
        public static string NAME = "FunctionTypedTaskAsync";
        public override string getName()
        {
            return NAME;
        }

        #endregion

        #region implemented abstract members of AbstractEnterpriseTAsync

        public override async System.Threading.Tasks.Task<string> executeAsync(IInfoContext info, FunctionContext context)
        {

            log("Sono partito " + getName());
            await Task.Delay(1000);
            if (context != null && context.getParam(0) != null)
                throw new SystemException("Fails");
            await Task.Delay(1000);
            log("Sono finito " + getName());
            return await Task.FromResult("Hi");
        }

        #endregion


    }
}

