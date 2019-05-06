using System;
using CallerCore.MainCore;

namespace CallerCoreSample.Droid.Log
{
	public class LoggerDroid : BaseLogger
	{
		const string TAG = "CallerCore";

		public override void WriteInfo(string level,string message)
		{
			Android.Util.Log.Info(TAG, string.Format("[{0}:{1}] {2}", level,DateTime.UtcNow, message));
		}
		public override void WriteError(string level,Exception e)
		{
			Android.Util.Log.Error(TAG, string.Format("[{0}:{1}] {2}", level,DateTime.UtcNow, e.StackTrace));
		}
	}
}

