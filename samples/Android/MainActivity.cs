using Android.App;
using Android.Widget;
using Android.OS;
using CallerCore.MainCore;
using CallerCoreSample.PCL;

namespace CallerCoreSample.Droid
{
    [Activity(Label = "CallerCore.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public static MainCoreDroid main;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            main = new MainCoreDroid();

            TextView MyTextView = FindViewById<TextView>(Resource.Id.textView);
            MyTextView.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
            MyTextView.Text = "Working..";

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
            ISampleSingleton issios = CallerCoreMobile.RegisterAsSingleton<ISampleSingleton>("CallerCoreSample.Droid.SampleSingleton", "1", "2");
            MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Singleton SampleResult invoked from Droid:{issios.SampleResult()}{System.Environment.NewLine}{System.Environment.NewLine}";


            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += async delegate
            {
                this.RunOnUiThread(() => { MyTextView.Text = $"{MyTextView.Text}Calling EnterpriseFunction..."; });
                bool con = await main.mmc.CallFunctionAsync<bool>(Connectivity.NAME);
                main.Primarylogger.Info("Connectitivity is:" + con);
                this.RunOnUiThread(() => { MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Connectitivity invoked from Droid is {con}"; });
                bool conpcl = await main.TestConnectivityPCL();
                main.Primarylogger.Info("Connectitivity is:" + conpcl);
                this.RunOnUiThread(() => { MyTextView.Text = $"{MyTextView.Text}{System.Environment.NewLine}Connectitivity invoked from PCL is {conpcl}{System.Environment.NewLine}"; });
            };
        }
    }
}


