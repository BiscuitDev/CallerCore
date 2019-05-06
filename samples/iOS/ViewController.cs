using System;

using UIKit;
using System.Threading.Tasks;
using CallerCore.MainCore;
using CallerCoreSample.PCL;

namespace CallerCoreSample.iOS
{
    public partial class ViewController : UIViewController
    {

        public static MainCoreiOS main;

        public ViewController(IntPtr handle) : base(handle)
        {
            main = new MainCoreiOS();


        }

        public async Task Test()
        {
            this.InvokeOnMainThread(() => { MyTextView.Text = $"{MyTextView.Text} Calling EnterpriseFunction..."; });
            bool con = await main.mmc.CallFunctionAsync<bool>(Connectivity.NAME);
            main.Primarylogger.Info($"Connectitivity is:{con}");
            this.InvokeOnMainThread(() => { MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Connectivity invoked from IOS is {con}"; });
            bool conpcl = await main.TestConnectivityPCL();
            main.Primarylogger.Info("Connectitivity is:" + conpcl);
            this.InvokeOnMainThread(() => { MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Connectivity invoked from PCL is {conpcl}{System.Environment.NewLine}"; });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //MessageHandler
            MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Call Handler to obtain SystemVersion";
            string sv = main.GetSystemVersion();
            main.Primarylogger.Info($"SystemVersion is:{sv}");
            MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}SystemVersion is:{sv}{System.Environment.NewLine}";
            //Listener
            FunctionContext fctx = new FunctionContext().AddType(ListenerPrintString.TYPE_RLOG).AddParam(ListenerPrintString.PARAM_PRINT, "Hello");
            /**
             * Sample FunctionContext
             * fctx.setStringParam (0,"Hello");
             * fctx.setObject<string>(0, "Hello");
             * fctx.setObject<string>(0, "Hello");
             * new FunctionContext().AddType(MyListenerTypes.RLOG).AddParam(0,"Hello");
             * fctx.setParam (0,"Hello");
             */

            main.mmc.DispatchToListeners(fctx);

            fctx = new FunctionContext().AddType(ListenerPrintString.TYPE_ADD);
            main.mmc.DispatchToListeners(fctx);
            MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Dispatch messagge to increase the counter...";
            //Calling EnterpriseFunction
            int p = main.mmc.CallFunction<int>(PrintCounter.NAME);
            MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Total Counter is {p}{System.Environment.NewLine}";

            //Sample Singleton by PCL
            MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Singleton SampleResult invoked from PCL:{main.iss.SampleResult()}";
            ISampleSingleton issios = CallerCoreMobile.RegisterAsSingleton<ISampleSingleton>("CallerCoreSample.iOS.SampleSingleton", "1", "2");
            MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Singleton SampleResult invoked from Droid:{issios.SampleResult()}{System.Environment.NewLine}{System.Environment.NewLine}";
            //Calling EnterpriseFunction Async
            CheckConnButton.TouchUpInside += async (sender, e) =>
             {
                 await Test();
             };

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

    }
}

