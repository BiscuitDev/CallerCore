using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallerCore.MainCore;

namespace TestCallerCore.Droid
{
	public class FunctionTypedTaskList : AbstractEnterpriseAsync<List<string>>
	{
		public static string NAME = "FunctionTypedTaskList";

		public override Task<List<string>> executeAsync(IInfoContext info, FunctionContext context)
		{
			new System.Threading.ManualResetEvent(false).WaitOne(1000);
			var z = context.getObject<List<string>>(0);
			return Task.FromResult(z);
		}

		public override string getName()
		{
			return NAME;
		}
	}
}
