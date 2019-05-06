using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
	public abstract class AbstractListenerAsync<T2> : AbstractListenerGenericAsync
	{

		#region implemented abstract members of AbstractListenerGenericAsync

		public override async Task<object> addContextToGenericAsyncQueue (FunctionContext fctx)
		{
			if (!isAsyncMethod()) await Task.Yield();
			var result = await addContextToAsyncQueue (fctx);
			return (object)result;
		}

		/*
		public override Task<object> addContextToGenericAsyncQueue (FunctionContext fctx)
		{
			//return addContextToAsyncQueue(fctx).ContinueWith( t => (object)t.Result);
			var tcs = new TaskCompletionSource<object>();
			addContextToAsyncQueue(fctx).ContinueWith( t => {
				if (t.IsFaulted)
				{
					// faulted with exception
					Exception ex = t.Exception;
					while (ex is AggregateException && ex.InnerException != null)
						ex = ex.InnerException;
					tcs.TrySetException(ex);
				}
				else if (t.IsCanceled) tcs.TrySetCanceled();
				else tcs.TrySetResult((object)t.Result);
			}
			);
			return tcs.Task;
		}
		*/

		#endregion

		public override bool isYieldMethod(){
			return true;
		}
		public  abstract Task<T2> addContextToAsyncQueue (FunctionContext fctx);

	}
}

