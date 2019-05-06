using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
    public abstract class AbstractEnterpriseRegular : AbstractEnterpriseFunction
    {
        public abstract override object execute(IInfoContext info, FunctionContext context);
    }
}
