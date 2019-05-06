/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CallerCore.MainCore
{

    /// <summary>
    /// Base class for listener
    /// 
    /// @author BiscuitDev
    /// </summary>
    public abstract class AbstractListener : EnterpriseListener
    {

        public virtual object addContextToQueue(FunctionContext fctx)
        {
            log(LogLevel.Debug, "Executing method addContextToQueue by Async implementation");
            var task = Task.Run<object>(() => addContextToGenericAsyncQueue(fctx));
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

        public virtual async Task<object> addContextToGenericAsyncQueue(FunctionContext fctx)
        {
            await Task.Yield();
            log(LogLevel.Debug, "Executing method addContextToAsyncQueue by Regular implementation");
            var result = addContextToQueue(fctx);
            if (result != null) return result;
            else return default(object);
            /*
			return  await Task.Run<object>(() => {
				var result=addContextToQueue(fctx);
				if (result!=null) return result;
				else return default(object);
			});*/

        }
        /// <summary>
        /// Main Class. </summary>
        protected internal CallerCoreMobile main;
        protected IInfoContext info;

        private ILogger Logger;

        private bool hasAsync;

        public abstract string getName();

        public abstract string[] getListenerTypes();

        /// <summary>
        /// Init the function
        /// </summary>
        /// <seealso cref= CallerCore.MainCore.EnterpriseListener#init() </seealso>
        /// 

        public virtual void init(CallerCoreMobile main)
        {
            this.main = main;
            this.info = main.infoctx;
            this.Logger = main.getLogger();
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