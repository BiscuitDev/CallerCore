using System;
using System.Threading.Tasks;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class FunctionGenericTaskAsync : AbstractEnterpriseGenericAsync
    {
        public static string NAME = "FunctionGenericTaskAsync";
        #region implemented abstract members of AbstractEnterpriseFunction
        public override string getName()
        {
            return NAME;
        }
        #endregion
        #region implemented abstract members of AbstractEnterpriseAsync
        public override async System.Threading.Tasks.Task<T> executeGenericAsync<T>(IInfoContext info, FunctionContext context)
        {

            log("Start Async function ");
            await Task.Delay(1000);
            if (context != null && context.getParam(0) != null)
                throw new SystemException("Fails");
            await Task.Delay(1000);
            log("Ended Async function ");
            return (T)(object)"Hi";
        }
        #endregion
    }
}

