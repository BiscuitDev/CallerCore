﻿using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace TestCallerCore.Droid
{
    public class ListenerTypedTask : AbstractListenerAsync<string>
    {
        #region implemented abstract members of AbstractListenerGenericAsync



        #endregion

        #region implemented abstract members of AbstractListener

        public static string NAME = "ListenerTypedTask";
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

        public override System.Threading.Tasks.Task<string> addContextToAsyncQueue(FunctionContext fctx)
        {
            log("1.Start Listener " + NAME + " " + fctx.type);
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            log("2.Waiting Listener " + NAME + " " + fctx.type);
            new System.Threading.ManualResetEvent(false).WaitOne(1000);
            log("3.Ended Listener " + NAME + " " + fctx.type);
            if (fctx.type.CompareTo("TEST3") == 0)
            {
                throw new Exception(fctx.type + " Fails");
            }
            return Task.FromResult("Hi");
        }

        #endregion


    }
}


