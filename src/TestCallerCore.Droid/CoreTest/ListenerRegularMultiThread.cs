using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace TestCallerCore.Droid
{
    public class ListenerRegularMultiThread : AbstractListenerRegular
    {

        #region implemented abstract members of AbstractListener

        public static string NAME = "ListenerRegularMultiThread";
        public static string[] LISTENER_TYPES = { "TESTMULTI1", "TESTMULTI2", "TESTMULTI3" };

        public override string getName()
        {
            return NAME;
        }

        public override string[] getListenerTypes()
        {
            return LISTENER_TYPES;
        }

        #endregion

        #region implemented abstract members of AbstractListenerAsync

        public override object addContextToQueue(FunctionContext fctx)
        {
            var myparam = fctx.getIntParam("test").Value;
            log("Start " + getName() + " Param " + myparam + " Type " + fctx.type);
            new System.Threading.ManualResetEvent(false).WaitOne(500);

            if (fctx.type.CompareTo("TESTMULTI3") == 0)
            {
                throw new Exception($"Error {fctx.type} {myparam}");
            }
            log("Ended " + getName() + " Param " + myparam);

            return myparam;
        }

        #endregion


    }
}