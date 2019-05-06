using System;
using System.Threading.Tasks;
using CallerCore.MainCore;
using Java.Net;

namespace CallerCoreSample.Droid
{
	public class Connectivity: AbstractEnterpriseAsync<bool>
	{
		public static string NAME = "Connectivity";

	   public override async Task<bool> executeAsync (IInfoContext info, FunctionContext context)
		{
			string host = "google.com";
			int msTimeout = 5000;

			return await Task.Run<bool> (() => {
				bool reachable;
				try {
					log(LogLevel.Info,"Connect to internet....");
					reachable = InetAddress.GetByName (host).IsReachable (msTimeout);
				} catch (UnknownHostException ex) {
					log ("Unable to reach: " + host + " Error: " + ex);
					reachable = false;
				} catch (Exception ex2) {
					log ("Unable to reach: " + host + " Error: " + ex2);
					reachable = false;
				}
				return reachable;
			});

		}
		public override string getName()
		{
			return NAME;
		}
		
	}
}

