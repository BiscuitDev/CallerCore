using System;
using System.Collections.Generic;
using System.Linq;
using CallerCore.MainCore;


namespace TestCallerCore.Droid
{
    public class FunctionRegularList : AbstractEnterpriseRegular
    {

        public static string NAME = "FunctionRegularList";

        public override object execute(IInfoContext info, FunctionContext context)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            var z = context.getObject<List<string>>(0);
            return z;
        }

        public override string getName()
        {
            return NAME;
        }
    }
}
