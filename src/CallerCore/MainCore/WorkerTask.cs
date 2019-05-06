using System;
using System.Text;
using System.Diagnostics;


/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>

namespace CallerCore.MainCore
{
	public class WorkerTask : Worker
	{
		internal string functionName;

		internal FunctionContext context;

		internal IInfoContext info;

		internal static int asyncWorkerID;

		internal string signature;

		internal CallerCoreMobile main;

        private ILogger Logger;

		public WorkerTask(CallerCoreMobile main, IInfoContext info, string functionName, FunctionContext context)
		{
			this.main = main;
			this.info = info;
			this.functionName = functionName;
            this.Logger = main.getLogger();
            if (context != null)
                this.context = new FunctionContext(context);
            else
                this.context = null;
			asyncWorkerID++;
		   // asyncWorkerID = worker.asyncWorkers++;
		}

		public virtual void init()
		{

			// Firma del Worker
			StringBuilder buff = new StringBuilder(32);
			buff.Append("[WorkerTask ").Append(functionName);
			buff.Append('-').Append(asyncWorkerID).Append("] ");
			signature = buff.ToString();

		}

		public virtual void run()
		{
            #if DEBUG
            Stopwatch timeOperations = Stopwatch.StartNew();
			if (info.DebugMode) Logger.Debug(signature+"Calling WorkerTask function " + functionName);
            #endif
			try
			{
				main.CallFunction(functionName, context);
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
			finally
			{
            #if DEBUG
            timeOperations.Stop();
			if (info.DebugMode) Logger.Debug(signature+"Called WorkerTask " + functionName + " in " + timeOperations.ElapsedMilliseconds + "ms");
            #endif
            }
		}
    }

}