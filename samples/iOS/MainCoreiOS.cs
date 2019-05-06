using System;
using CallerCoreSample.PCL;
using System.Threading.Tasks;

namespace CallerCoreSample.iOS
{
	public class MainCoreiOS : MainCorePcl
	{
		public MainCoreiOS ()
		{
			TypeApplication = "iOS1";
			DebugMode = true;
			Init ();
			mmc.RegisterAsFunction<Connectivity>(TypeApplication);
			mmc.RegisterAsFunction<HandlerSystemVersion>(TypeApplication);
			mmc.RegisterAsSingleton<ISampleSingleton, SampleSingleton>(ref iss, "one", "two");
		}

	}
}

