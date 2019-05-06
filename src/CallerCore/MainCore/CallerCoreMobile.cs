using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;


/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>
namespace CallerCore.MainCore
{

    public class CallerCoreMobile
    {
        public IInfoContext infoctx;
        public FunctionCaller functionCaller;
        public static readonly string DEFAULT = "default";
        public static CallerCoreMobile callerCoreMobile;

        public CallerCoreMobile(IInfoContext infoctx)
        {
            this.infoctx = infoctx;
            callerCoreMobile = this;
            functionCaller = new FunctionCaller(this);
        }
        /// <summary>
        /// Get Logger
        /// </summary>
        public ILogger getLogger()
        {
            return functionCaller.getLogger();
        }


        /// <summary>
        /// Starts the Task, scheduling it for execution to the current TaskScheduler.
        /// </summary>
        /// <param name="functionName">	Function Name. </param>
        /// <param name="context">	Params that can be passed to the function using FunctionContext. </param> 
        /// <param name="info">    Class that implements IInfoContext. </param>
        public virtual void CallSynchronizedTaskFunction(string functionName, IInfoContext info, FunctionContext context)
        {
            WorkerTask asyncWorker = new WorkerTask(this, info, functionName, context);
            asyncWorker.init();
            Task runner = new Task(asyncWorker.run);
            runner.Start();
            //return runner;
            //return Task.Run(() => asyncWorker.run());
        }
        /// <summary>
        /// Starts the Task, scheduling it for execution to the current TaskScheduler.
        /// <seealso cref="callThreadFunction( functionName,  info,  context)"/>
        /// </summary>
        public virtual void CallSynchronizedTaskFunction(string functionName)
        {
            CallSynchronizedTaskFunction(functionName, this.infoctx, null);
        }
        /// <summary>
        /// Starts the Task, scheduling it for execution to the current TaskScheduler.
        /// <seealso cref="callThreadFunction( functionName,  info,  context)"/>
        /// </summary>
        public virtual void CallSynchronizedTaskFunction(string functionName, FunctionContext context)
        {
            CallSynchronizedTaskFunction(functionName, this.infoctx, context);
        }

        /// <summary>
        /// Call an Async Message Handler by FunctionContext.Type
        /// </summary>
        public virtual Task<T> CallMessageHandlerAsync<T>(FunctionContext context)
        {
            return functionCaller.callMessageHandlerAsync<T>(this.infoctx, context);

        }
        /// <summary>
        /// Call a Message Handler by FunctionContext.Type
        /// </summary>
        public virtual object CallMessageHandler(FunctionContext context)
        {
            return functionCaller.callMessageHandler(this.infoctx, context);
        }

        /// <summary>
        /// Call a Message Handler by FunctionContext.Type
        /// </summary>
        public virtual T CallMessageHandler<T>(FunctionContext context)
        {
            return (T)functionCaller.callMessageHandler(this.infoctx, context);
        }

        /// <summary>
        /// Async Dispatch a message(FunctionContext) to listener by FunctionContext.Type
        /// </summary>
        public virtual Task<Dictionary<string, object>> DispatchAsyncToListeners(FunctionContext context)
        {
            return functionCaller.dispatchMessageToAsyncListeners(context);
        }
        /// <summary>
        /// Async Dispatch a message(FunctionContext) to listener by FunctionContext.Type
        /// </summary>
        public virtual Dictionary<string, Task<object>> DispatchAsyncToListenersGetTasks(FunctionContext context)
        {
            return functionCaller.dispatchMessageToAsyncListenersGetTasks(context);
        }

        public virtual void DispatchToScheduledExecution(FunctionContext context)
        {
            functionCaller.dispatchMessageToScheduledExecution(context);
        }
        /// <summary>
        ///  Dispatch a message(FunctionContext) to listener by FunctionContext.Type
        /// </summary>
        public virtual Dictionary<string, object> DispatchToListeners(FunctionContext context)
        {
            return functionCaller.dispatchMessageToListeners(context);
        }
        /// <summary>
        /// Async Dispatch a message(FunctionContext)  to a single listener by FunctionContext.Type
        /// </summary>
        public virtual Task<T> DispatchAsyncToSingleListener<T>(FunctionContext ctx, string functionName)
        {
            return functionCaller.dispatchMessageToAsyncSingleListener<T>(ctx, functionName);
        }
        public virtual Task<object> DispatchAsyncToSingleListener(FunctionContext ctx, string functionName)
        {
            return functionCaller.dispatchMessageToAsyncSingleListener(ctx, functionName);
        }

        public virtual void DispatchToScheduledExecution(FunctionContext ctx, string functionName)
        {
            functionCaller.dispatchMessageToSingleScheduledExecution(ctx, functionName);
        }
        /// <summary>
        /// Dispatch a message(FunctionContext)  to a single listener by FunctionContext.Type
        /// </summary>
        public virtual T DispatchToSingleListener<T>(FunctionContext ctx, string functionName)
        {
            return functionCaller.dispatchMessageToSingleListener<T>(ctx, functionName);
        }
        public virtual object DispatchToSingleListener(FunctionContext ctx, string functionName)
        {
            return functionCaller.dispatchMessageToSingleListener(ctx, functionName);
        }

        /// <summary>
        /// Call A Function in Async
        /// </summary>
        /// <param name="functionName">	Function Name. </param>
        /// <param name="context">	Params that can be passed to the function using FunctionContext. </param> 
        /// <param name="info">    Class that implements IInfoContext. </param>
        public virtual Task<T> CallFunctionAsync<T>(string functionName, IInfoContext info, FunctionContext context)
        {
            return functionCaller.callFunctionAsync<T>(functionName, info, context);
        }
        /// <summary>
        /// Call A Function in Async
        /// <seealso cref="callFunctionAsync<T>(functionName,info,context)"/>
        /// </summary>	
        public virtual Task<T> CallFunctionAsync<T>(string functionName)
        {
            return CallFunctionAsync<T>(functionName, this.infoctx, null);
        }
        /// <summary>
        /// Call A Function in Async
        /// <seealso cref="callFunctionAsync<T>(functionName,info,context)"/>
        /// </summary>	
        public virtual Task<T> CallFunctionAsync<T>(string functionName, FunctionContext context)
        {
            return CallFunctionAsync<T>(functionName, this.infoctx, context);
        }
        /// <summary>
        /// Call a Regular Function
        /// </summary>
        /// <typeparam name="T">Generic Type to return</typeparam>
        /// <param name="functionName">	Function Name. </param>
        /// <param name="context">	Params that can be passed to the function using FunctionContext. </param> 
        /// <param name="info">    Class that implements IInfoContext. </param>
        /// <returns>Return a generic type result</returns>
        public virtual T CallFunction<T>(string functionName, IInfoContext info, FunctionContext context)
        {
            var result = CallFunction(functionName, info, context);
            if (result != null)
                return (T)result;
            else
                return default(T);

        }
        /// <summary>
        /// Call a Regular Function
        /// <seealso cref="callFunction(functionName,info,context)"/>
        /// </summary>	
        public virtual T CallFunction<T>(string functionName, FunctionContext context)
        {
            return CallFunction<T>(functionName, this.infoctx, context);
        }
        /// <summary>
        /// Call a Regular Function
        /// <seealso cref="callFunction<T>(functionName,info,context)"/>
        /// </summary>	
        public virtual T CallFunction<T>(string functionName)
        {
            return CallFunction<T>(functionName, this.infoctx, null);
        }
        /// <summary>
        /// Call a Regular Function
        /// </summary>
        /// <param name="functionName">	Function Name. </param>
        /// <param name="context">	Params that can be passed to the function using FunctionContext. </param> 
        /// <param name="info">    Class that implements IInfoContext. </param>
        /// <returns>Return an object result</returns>
        public virtual object CallFunction(string functionName, IInfoContext info, FunctionContext context)
        {
            return functionCaller.callFunction(functionName, info, context);
        }
        /// <summary>
        /// Call a Regular Function
        /// <seealso cref="callFunction(functionName,info,context)"/>
        /// </summary>	
        public virtual object CallFunction(string functionName, FunctionContext context)
        {
            return CallFunction(functionName, this.infoctx, context);
        }
        /// <summary>
        /// Call a Regular Function
        /// <seealso cref="callFunction(functionName,info,context)"/>
        /// </summary>	
        public virtual object CallFunction(string functionName)
        {
            return CallFunction(functionName, this.infoctx, null);
        }
        /// <summary>
        /// Register a Singleton Class with its fields
        /// </summary>
        /// <typeparam name="T">Generic Type Interface </typeparam>
        /// <param name="className"> NameSpace and Class Name that Implement T. </param>
        /// <param name="args"> fields of this Class. </param>
        /// <returns>Interface's implementation</returns>
        public static T RegisterAsSingleton<T>(string className, params object[] args)
        {
            return FunctionCaller.registerAsSingleton<T>(className, args);
        }
        /// <summary>
        /// Register a Singleton Class 
        /// </summary>
        /// <typeparam name="T">Generic Type Interface </typeparam>
        /// <param name="className"> NameSpace and Class Name that Implement T. </param>
        /// <returns>Interface's implementation</returns>
        public static T RegisterAsSingleton<T>(string className)
        {
            return FunctionCaller.registerAsSingleton<T>(className);
        }

        public static T RegisterAsSingleton<T, K>()
        {
            Type clazz = typeof(K);
            return FunctionCaller.registerAsSingleton<T>(clazz);
        }
        public static T RegisterAsSingleton<T, K>(params object[] args)
        {
            Type clazz = typeof(K);
            return FunctionCaller.registerAsSingleton<T>(clazz, args);
        }
        /// <summary>
        ///  Register a Singleton Class. Interface's implementation at T parameter
        /// </summary>
        /// <param name="applicationName"> Assign variable to an Application name [default or your name]</param>
        /// <param name="result"> Parameter that Implement the Class T. </param>
        /// <param name="className"> NameSpace and Class Name that Implement the function. </param>
        public virtual void RegisterAsSingleton<T>(string applicationName, ref T result, string className, params object[] args)
        {
            if (functionCaller.isLoadable(this.infoctx, applicationName))
                result = functionCaller.registerAsSingleton<T>(this.infoctx, applicationName, className, args);

        }
        public virtual void RegisterAsSingleton<T, K>(string applicationName, ref T result, params object[] args)
        {
            if (functionCaller.isLoadable(this.infoctx, applicationName))
                result = functionCaller.registerAsSingleton<T, K>(this.infoctx, this.infoctx.TypeApplication, args);
        }
        public virtual void RegisterAsSingleton<T, K>(ref T result, params object[] args)
        {
            result = functionCaller.registerAsSingleton<T, K>(this.infoctx, this.infoctx.TypeApplication, args);
        }
        /// <summary>
        /// Register a Function
        /// </summary>
        /// <param name="applicationName"> Assign a Function to an Application name [default or your name]</param>
        /// <param name="className"> NameSpace and Class Name that Implement the function. </param>
        public virtual void RegisterAsFunction(string applicationName, string className)
        {
            functionCaller.addFunction(this.infoctx, applicationName, className);
        }
        /// <summary>
        /// Register a Function
        /// </summary>
        /// <param name="applicationName"> Assign a Function to an Application name [default or your name]</param>
        /// <typeparam name="T">ClassType to instance as Function </typeparam>
        public virtual void RegisterAsFunction<T>(string applicationName) where T : EnterpriseFunction, new()
        {
            functionCaller.addFunction<T>(this.infoctx, applicationName);
        }
        /// <summary>
        /// Register a Function
        /// </summary>
        /// <param name="applicationName"> Assign a Function to an Application name [default or your name]</param>
        /// <typeparam name="T">ClassType to instance as Function </typeparam>
        public virtual void RegisterAsFunction<T>() where T : EnterpriseFunction, new()
        {
            functionCaller.addFunction<T>(this.infoctx, this.infoctx.TypeApplication);
        }
        /// <summary>
        /// Register a listener
        /// </summary>
        /// <param name="applicationName"> Assign a Listener to an Application name [default or your name]</param>
        /// <param name="className"> NameSpace and Class Name that Implement the listener. </param>
        public virtual void RegisterAsListener(string applicationName, string className)
        {
            functionCaller.addListener(this.infoctx, applicationName, className);
        }
        /// <summary>
        /// Register a listener
        /// </summary>
        /// <param name="applicationName"> Assign a Listener to an Application name [default or your name]</param>
        /// <typeparam name="T">ClassType to instance as Listener </typeparam>		
        public virtual void RegisterAsListener<T>(string applicationName) where T : AbstractListener, new()
        {
            functionCaller.addListener<T>(this.infoctx, applicationName);
        }

        public virtual void RegisterAsListener<T>() where T : AbstractListener, new()
        {
            functionCaller.addListener<T>(this.infoctx, this.infoctx.TypeApplication);
        }


        public virtual void RegisterAsLogger(string applicationName, string className)
        {
            functionCaller.registerLogger(this.infoctx, applicationName, className);
        }
        public virtual void RegisterAsLogger(string applicationName, string className, LogLevel loglevel)
        {
            functionCaller.registerLogger(this.infoctx, applicationName, className, loglevel);
        }
        public virtual void RegisterAsLogger<T>()
        {
            functionCaller.registerLogger<T>(this.infoctx, this.infoctx.TypeApplication);
        }
        public virtual void RegisterAsLogger<T>(LogLevel loglevel)
        {
            functionCaller.registerLogger<T>(this.infoctx, this.infoctx.TypeApplication, loglevel);
        }
        public virtual void RegisterAsLogger<T>(string applicationName)
        {
            functionCaller.registerLogger<T>(this.infoctx, applicationName);
        }
        public virtual void RegisterAsLogger<T>(string applicationName, LogLevel loglevel)
        {
            functionCaller.registerLogger<T>(this.infoctx, applicationName, loglevel);
        }
        public virtual void RemoveFunction(string functionName)
        {
            functionCaller.removeFunction(this.infoctx, functionName);
        }
        public virtual void RemoveFunction<T>() where T : EnterpriseFunction, new()
        {
            functionCaller.removeFunction<T>();
        }
        public virtual void CleanAssemblyCache()
        {
            FunctionCaller.CleanAssemblyCache();
        }
    }
}
