using System;
using CallerCore.MainCore;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
	public abstract class AbstractListenerGenericAsync : AbstractListener
	{
		public abstract override Task<object> addContextToGenericAsyncQueue(FunctionContext fctx);
	}
}

