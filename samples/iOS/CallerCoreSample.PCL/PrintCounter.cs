using System;
using CallerCore.MainCore;

namespace CallerCoreSample.PCL
{
	public class PrintCounter  : AbstractEnterpriseFunction
	{
		#region implemented abstract members of AbstractEnterpriseFunction
		public static string NAME = "Counter";

		public override string getName ()
		{
			return NAME;
		}

		#endregion
		public override object execute(IInfoContext info, FunctionContext context)
		{
			MainCorePcl mainAdapter = (MainCorePcl)info;

			log("This counter is "+mainAdapter.Counter());
				
			return mainAdapter.Counter();
		}

	}
}

