using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
	public abstract class AbstractEnterpriseAsync<T2> : AbstractEnterpriseGenericAsync
	{
		
		#region implemented abstract members of AbstractEnterpriseAsync
		public override async Task<T> executeGenericAsync<T> (IInfoContext info, FunctionContext context)
		{	
		if (!isAsyncMethod()) await Task.Yield();
		var result = await executeAsync (info, context);
		return (T)(object)result;
		}

		/*
		public override Task<T> executeGenericAsync<T> (IInfoContext info, FunctionContext context)
		{

			var tcs = new TaskCompletionSource<T>();
			//.ContinueWith<T>( t => (T)(object)t.Result);
		    executeAsync(info, context).ContinueWith( t => {
				if (t.IsFaulted)
				{
					// faulted with exception
					Exception ex = t.Exception;
					while (ex is AggregateException && ex.InnerException != null)
						ex = ex.InnerException;
					tcs.TrySetException(ex);
				}
				else if (t.IsCanceled) tcs.TrySetCanceled();
				else tcs.TrySetResult((T)(object)t.Result);

			}
			);
			return tcs.Task;

		}
		*/

		/// <summary>
		/// Method to overrived. If not implemented exec the method async
		/// </summary>
		/// <param name="IInfoContext">Main Info Class</param>
		/// <param name="FunctionContext">Params</param>
		/// <returns>return an object</returns>
		public override object execute(IInfoContext info, FunctionContext context)
		{
			log(LogLevel.Debug,"Executing method execute by Async implementation");
			var task= Task.Run<T2>(() => executeGenericAsync<T2>(info, context));
			try {
				return (object)task.Result;
			}
			catch (AggregateException ae) {
				ae.Handle((x) =>
					{
						throw x;
					});
				return null;
			}
		}

		//public abstract string getName();
		public override bool isYieldMethod(){
			return true;
		}
		#endregion
		public  abstract Task<T2> executeAsync (IInfoContext info, FunctionContext context);

	}
}

