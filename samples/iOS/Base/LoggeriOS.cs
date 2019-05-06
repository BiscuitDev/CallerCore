using System;
using CallerCore.MainCore;

namespace CallerCoreSample.iOS.Log
{
	public class LoggeriOS: BaseLogger
	{
		public override void WriteInfo(string level,string message)
		{
			Console.WriteLine("[{0}:{1}] {2}",level, DateTime.UtcNow, message);
		}

		public override void WriteError(string level,Exception e)
		{
			Console.WriteLine("[{0}:{1}] {2}", level,DateTime.UtcNow, e.StackTrace);
		}
	}
}

