using System;
using CallerCore.MainCore;

namespace CallerCore.MainCore
{
	public abstract class AbstractListenerRegular  : AbstractListener
	{
		public abstract override object addContextToQueue(FunctionContext fctx);
	}
}

