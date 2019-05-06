﻿using System;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class ListenerRegular : AbstractListenerRegular
    {
        public static string NAME = "ListenerRegular";
        public static string[] LISTENER_TYPES = { "TEST1", "TEST2", "TEST3" };
        #region implemented abstract members of AbstractListener
        public override string getName()
        {
            return NAME;
        }
        public override string[] getListenerTypes()
        {
            return LISTENER_TYPES;
        }
        public override object addContextToQueue(FunctionContext fctx)
        {
            log("1.Start Listener " + NAME + " " + fctx.type);
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            log("2.Start Listener " + NAME + " " + fctx.type);
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            log("3.Ended Listener " + NAME + " " + fctx.type);
            if (fctx.type.CompareTo("TEST3") == 0)
            {
                throw new Exception(fctx.type + " Fails");
            }
            return "Hi";
        }
        #endregion
    }
}

