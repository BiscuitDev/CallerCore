using System;
using CallerCoreSample.PCL;
using System.Threading.Tasks;

namespace CallerCoreSample.Droid
{
	public class MainCoreDroid: MainCorePcl
	{
		public MainCoreDroid ()
		{
			TypeApplication = "Droid1";
			DebugMode = true;
			Init ();
			mmc.RegisterAsFunction<Connectivity>();
			mmc.RegisterAsFunction<HandlerSystemVersion>();
			mmc.RegisterAsSingleton<ISampleSingleton,SampleSingleton>(ref iss,"one","two" );
		}

	}
}

