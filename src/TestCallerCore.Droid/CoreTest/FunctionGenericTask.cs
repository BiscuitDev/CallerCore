using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace TestCallerCore.Droid
{
    public class FunctionGenericTask : AbstractEnterpriseGenericAsync
    {
        public static string NAME = "FunctionGenericTask";
        #region implemented abstract members of AbstractEnterpriseFunction

        public override string getName()
        {
            return NAME;
        }

        #endregion

        #region implemented abstract members of AbstractEnterpriseAsync

        public override System.Threading.Tasks.Task<T> executeGenericAsync<T>(IInfoContext info, FunctionContext context)
        {
            log("****** Num Param=" + ((context != null) ? $"{context.NumParam}" : "-1"));

            log("Start Task function");
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            if (context != null && context.getParam(0) != null)
                throw new SystemException("Fails");
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            log("Ended Task function ");


            return Task.FromResult((T)(object)"Hi");
        }

        #endregion




    }
}

