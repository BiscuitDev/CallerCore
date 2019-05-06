using System;
using System.Threading.Tasks;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class FunctionGenericTaskAsyncList : AbstractEnterpriseGenericAsync
    {
        public static string NAME = "FunctionGenericTaskAsyncList";
        public override async Task<T> executeGenericAsync<T>(IInfoContext info, FunctionContext context)
        {
            await Task.Delay(1000);
            var z = context.getObject<T>(0);

            return await Task.FromResult(z);
        }

        public override string getName()
        {
            return NAME;
        }
    }
}
