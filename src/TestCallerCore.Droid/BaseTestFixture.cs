using System;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class BaseTestFixture
    {
        MainBase _vlr;
        public FunctionContext fctx;
        private object m_Locker = new object();
        protected MainBase vlr
        {
            get
            {
                lock (m_Locker)
                {
                    if (_vlr == null)
                    {
                        _vlr = new MainBase();
                        _vlr.mmc.RegisterAsFunction<FunctionTypedTask>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionTypedTaskAsync>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionRegular>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionGenericTaskAsync>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionGenericTask>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionGenericTaskAsyncList>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionRegularList>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionTypedTaskList>(CallerCoreMobile.DEFAULT);

                        _vlr.mmc.RegisterAsListener<ListenerRegular>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsListener<ListenerGenericTaskAsync>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsListener<ListenerTypedTaskAsync>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsListener<ListenerGenericTask>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsListener<ListenerTypedTask>(CallerCoreMobile.DEFAULT);

                        _vlr.mmc.RegisterAsFunction<FunctionTypedTaskAsyncMultiThread>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsFunction<FunctionRegularMultiThread>(CallerCoreMobile.DEFAULT);

                        _vlr.mmc.RegisterAsListener<ListenerRegularMultiThread>(CallerCoreMobile.DEFAULT);
                        _vlr.mmc.RegisterAsListener<ListenerTypedTaskAsyncMultiThread>(CallerCoreMobile.DEFAULT);

                        return _vlr;
                    }
                    else
                    {

                        return _vlr;
                    }
                }
            }
        }
    }
}

