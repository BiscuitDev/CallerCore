using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{
    /// <summary>
    /// Interface of main class referenced by all functions
    /// </summary>
    public interface IInfoContext
    {
        string TypeApplication { get; set; }
        bool DebugMode { get; set; }
    }
}
