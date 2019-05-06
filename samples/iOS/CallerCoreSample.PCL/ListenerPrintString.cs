using System;
using CallerCore.MainCore;
using System;
using CallerCore.MainCore;

namespace CallerCoreSample.PCL
{
    public class ListenerPrintString : AbstractListenerRegular
    {

        public static string NAME = "ListenerPrintString";
        public static string TYPE_RLOG = "RLOG";
        public static string TYPE_ADD = "ADD";
        public static string PARAM_PRINT = "PRINT";
        string[] LISTENER_TYPES = { TYPE_RLOG, TYPE_ADD };
        MainCorePcl mainAdapter;

        public override void init(CallerCoreMobile main)
        {
            base.init(main);
            mainAdapter = (MainCorePcl)info;
        }

        #region implemented abstract members of AbstractListener

        public override string getName()
        {
            return NAME;
        }

        public override string[] getListenerTypes()
        {
            return LISTENER_TYPES;
        }

        #endregion

        #region implemented abstract members of AbstractListenerRegular

        public override object addContextToQueue(FunctionContext fctx)
        {
            log(LogLevel.Debug, "[Command] " + fctx.type);
            if (fctx.type == TYPE_RLOG)
            {

                log("Received: " + fctx.getStringParam(PARAM_PRINT, null));
            }
            if (fctx.type == TYPE_ADD)
            {
                mainAdapter.Add();
            }
            return null;
        }

        #endregion


    }
}

