using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
    public abstract class AbstractMessageHandler : AbstractEnterpriseFunction, MessageHandler
    {

        public abstract string[] getTargetTypes();

        //public virtual bool checkRepeatedNumber()
        //{
        //    return false;
        //}

    }
}
