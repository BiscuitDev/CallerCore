using System;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
    public class MainBase : IInfoContext
    {
        public CallerCoreMobile mmc { get; set; }
        public MainBase()
        {
            mmc = new CallerCoreMobile(this);
        }

        public string TypeApplication
        {
            get { return "Droid1"; }
            set { throw new NotImplementedException(); }
        }
        public bool DebugMode { get { return true; } set { throw new NotImplementedException(); } }
    }
}
