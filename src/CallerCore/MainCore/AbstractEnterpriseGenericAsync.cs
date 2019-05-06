using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
	public abstract class AbstractEnterpriseGenericAsync : AbstractEnterpriseFunction
	{
		/// <summary>
		/// add async at abstract function
		/// </summary>
		public abstract override Task<T> executeGenericAsync<T>(IInfoContext info, FunctionContext context);
	}
}

