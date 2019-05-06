using System;
using System.Threading.Tasks;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class ListenerGenericTaskAsync : AbstractListenerGenericAsync
    {
        #region implemented abstract members of AbstractListenerGenericAsync

        public async override Task<object> addContextToGenericAsyncQueue(FunctionContext fctx)
        {

            log("1.Start Listener " + NAME + " " + fctx.type);
            await Task.Delay(1000);
            log("2.Waiting Listener " + NAME + " " + fctx.type);
            //throw new Exception ("Vediamo");
            await Task.Delay(1000);
            log("3.Ended Listener " + NAME + " " + fctx.type);

            if (fctx.type.CompareTo("TEST3") == 0)
            {
                throw new Exception(fctx.type + " Fails");
            }
            return (object)"Hi";

        }

        #endregion

        #region implemented abstract members of AbstractListener
        public static string NAME = "ListenerGenericTaskAsync";
        public static string[] LISTENER_TYPES = { "TEST1", "TEST2", "TEST3" };

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



        #endregion


    }
}

