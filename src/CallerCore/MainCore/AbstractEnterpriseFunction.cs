/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace CallerCore.MainCore
{


    /// <summary>
    /// base class for system functions
    /// 
    /// @author BiscuitDev
    /// </summary>
    public abstract class AbstractEnterpriseFunction : EnterpriseFunction
    {
        /// <summary>
        /// Method to overrived. If not implemented exec the method async
        /// </summary>
        /// <param name="IInfoContext">Main Info Class</param>
        /// <param name="FunctionContext">Params</param>
        /// <returns>return an object</returns>
        public virtual object execute(IInfoContext info, FunctionContext context)
        {
            log(LogLevel.Debug, "Executing method execute by Async implementation");
            var task = Task.Run<object>(() => executeGenericAsync<object>(info, context));
            try
            {
                return task.Result;
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
                    {
                        throw x;
                    });
                return null;
            }
        }
        /// <summary>
        /// Method to overrived. If not implemented exec the method sync
        /// </summary>
        /// <param name="IInfoContext">Main Info Class</param>
        /// <param name="FunctionContext">Params</param>
        /// <returns>return T</returns>
        public virtual async Task<T> executeGenericAsync<T>(IInfoContext info, FunctionContext context)
        {
            await Task.Yield();
            log(LogLevel.Debug, "Executing method executeAsync by Regular implementation");
            var result = execute(info, context);
            if (result != null) return (T)result;
            else return default(T);
            /*return  await Task.Run<T>(() => {
				var result=execute(info, context);
				if (result!=null) return (T)result;
				else return default(T);
			});
			*/
        }
        /// <summary>
        /// Return the function's name
        /// </summary>
        /// <returns> function name </returns>
        public abstract string getName();
        /// <summary>
        /// long name function </summary>
        public string fullName;

        /// <summary>
        /// Main class </summary>
        public CallerCoreMobile main;
        /// <summary>
        /// Logger </summary>
        private ILogger Logger;

        private bool hasAsync;
        /// <summary>
        /// Init the function system
        /// </summary>
        /// <seealso cref= CallarCore.MainCore#init() </seealso>
        public virtual void Load(string fullName, CallerCoreMobile main)
        {
            this.fullName = fullName;
            this.main = main;
            this.Logger = main.getLogger();
        }

        public virtual Task<Dictionary<string, object>> DispatchAsyncToListenersGetResults(FunctionContext context)
        {
            return main.DispatchAsyncToListeners(context);
        }

        public virtual Dictionary<string, object> dispatchToListeners(FunctionContext context)
        {
            return main.DispatchToListeners(context);
        }

        public virtual Task<object> dispatchAsyncToSingleListener(FunctionContext ctx, string functionName)
        {
            return main.DispatchAsyncToSingleListener(ctx, functionName);
        }

        public virtual object dispatchToSingleListener(FunctionContext ctx, string functionName)
        {
            return main.DispatchToSingleListener(ctx, functionName);
        }
        /// <summary>
        /// Return complete name of function
        /// </summary>
        public override string ToString()
        {
            return fullName;
        }

        public virtual void log(LogLevel logLevel, string format, params object[] args)
        {
            Logger.Log(logLevel, $"[{getName()}]:{string.Format(format, args)}");
        }

        public virtual void log(LogLevel logLevel, string message)
        {
            Logger.Log(logLevel, $"[{getName()}]:{message}");
        }

        public virtual void log(string message)
        {
            Logger.Info($"[{getName()}]:{message}");
        }
        public virtual void log(Exception e)
        {
            Logger.Error(e, getName());
        }

        public virtual void setHasAsynMethod(bool hasAsync)
        {
            this.hasAsync = hasAsync;
        }

        public virtual bool isAsyncMethod()
        {
            return hasAsync;
        }

        public virtual bool isYieldMethod()
        {
            return false;
        }
    }

}