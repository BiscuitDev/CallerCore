using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace CallerCoreSample.PCL
{
	public abstract class MainCorePcl : IInfoContext
	{

		public  CallerCoreMobile mmc { get; set; }
		public ILogger Primarylogger = null;
		public  string TypeApplication { get; set;}
		private int counter;
		public  bool DebugMode { get; set;}
		public ISampleSingleton iss=null;

		public void Init ()
		{
			mmc = new CallerCoreMobile (this);
			BuckleUp ();
			Primarylogger = mmc.getLogger ();
			Primarylogger.SetLogLevel(LogLevel.Debug);

		}

		private void BuckleUp()
		{
			mmc.RegisterAsLogger ("Droid*", "CallerCoreSample.Droid.Log.LoggerDroid");
			mmc.RegisterAsLogger ("iOS*","CallerCoreSample.iOS.Log.LoggeriOS");
			/**
			 * other examples as you can register functions from PCL
			 * mmc.RegisterAsFunction ("Droid1", "CallerCoreSample.Droid.Connectivity");
			 * mmc.RegisterAsFunction ("iOS1", "CallerCoreSample.iOS.Connectivity");
			 * mmc.RegisterAsFunction ("iOS1", "CallerCoreSample.iOS.Connectivity,CallerCoreSample.iOS");
			 * mmc.RegisterAsFunction ("Droid1", "CallerCoreSample.Droid.HandlerSystemVersion");
			 * mmc.RegisterAsFunction ("iOS1", "CallerCoreSample.iOS.HandlerSystemVersion");
			 * mmc.RegisterAsSingleton<ISampleSingleton> ("iOS1", ref iss,"CallerCoreSample.iOS.SampleSingleton", "one", "two");
			 * mmc.RegisterAsSingleton<ISampleSingleton> ("Droid1", ref iss,"CallerCoreSample.Droid.SampleSingleton", "one", "two");
			**/
			mmc.RegisterAsFunction<PrintCounter> (CallerCoreMobile.DEFAULT);
			mmc.RegisterAsListener<ListenerPrintString> (CallerCoreMobile.DEFAULT);

		}

		public Task<bool> TestConnectivityPCL(){
			return mmc.CallFunctionAsync<bool> ("Connectivity");
		}

		public string GetSystemVersion(){
			return mmc.CallMessageHandler<string>(FunctionContext.NewFunctionContext(MyHandlerTypes.SV));
		}

		internal void Add(){
			counter++;
		}

		internal int Counter(){
			return counter; 
		}
	}
}

