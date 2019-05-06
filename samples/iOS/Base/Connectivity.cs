using System;
using CallerCore.MainCore;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CallerCoreSample.iOS
{
	public class Connectivity: AbstractEnterpriseAsync<bool>
	{
		public static string NAME = "Connectivity";

		#region implemented abstract members of AbstractEnterpriseAsync

		public override async Task<bool> executeAsync (IInfoContext info, FunctionContext context)
		{
			string host = "google.com";
			int msTimeout = 5000;
			int port = 80;
			return await Task.Run(() =>
				{
					try
					{
						log(LogLevel.Info,"Connect to internet....");
						var clientDone = new ManualResetEvent(false);
						var reachable = false;
						var hostEntry = new DnsEndPoint(host, port);
						using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
						{
							var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = hostEntry };
							socketEventArg.Completed += (s, e) =>
							{
								reachable = e.SocketError == SocketError.Success;

								clientDone.Set();
							};

							clientDone.Reset();

							socket.ConnectAsync(socketEventArg);

							clientDone.WaitOne(msTimeout);

							return reachable;
						}
					}
					catch (Exception ex)
					{
						log("Unable to reach: " + host + " Error: " + ex);
						return false;
					}
				});
		}

		#endregion


		public override string getName()
		{
			return NAME;
		}
	}
}



