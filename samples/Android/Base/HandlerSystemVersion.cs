using System;
using CallerCore.MainCore;
using CallerCoreSample.PCL;

namespace CallerCoreSample.Droid
{
    public class HandlerSystemVersion : AbstractMessageHandler
    {
        public static string NAME = "SystemVersion";
        public static string[] TARGET_TYPES = { MyHandlerTypes.SV.ToString() };

        public override object execute(IInfoContext info, FunctionContext context)
        {
            return Android.OS.Build.VERSION.Release;
        }

        public override string getName()
        {
            return NAME;
        }

        public override string[] getTargetTypes()
        {
            return TARGET_TYPES;
        }
    }

}


