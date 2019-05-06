using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;


using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;

namespace CallerCore.MainCore
{


    /// <summary>
    /// Un enterprise caller � un oggetto responsabile del caricamento delle funzioni del
    /// sistema e della loro esecuzione per conto dei Worker thread.
    /// 
    /// @author BiscuitDev
    /// </summary>

    public class FunctionProps
    {

        public string appType;
        public string functionName;
        public string className;
        //        protected List<string> _Proprietis;
        //        public List<string> proprietis
        //        {
        //            get
        //            {
        //                if (null == _Proprietis)
        //                {
        //                    _Proprietis = new List<string>();
        //                }
        //                return _Proprietis;
        //            }
        //            set
        //            {
        //                _Proprietis = value;
        //            }
        //        }
    }

    public class ListenerProps
    {

        public string appType;
        public string functionName;
        public string className;
        //        protected List<string> _dataType;
        //        public List<string> dataTypes
        //        {
        //            get
        //            {
        //                if (null == _dataType)
        //                {
        //                    _dataType = new List<string>();
        //                }
        //                return _dataType;
        //            }
        //            set
        //            {
        //                _dataType = value;
        //            }
        //        }
    }
    public class FunctionCaller
    {

        internal Dictionary<string, EnterpriseFunction> defaultApplicationMap;
        //internal Dictionary<string, Dictionary<string, EnterpriseFunction>> functionsMap;
        internal Dictionary<string, MessageHandler> defaultHandlersMap;
        //internal Dictionary<string, Dictionary<string, MessageHandler>> handlersMap;
        internal Dictionary<string, List<AbstractListener>> listenerMapDataType;
        internal Dictionary<string, AbstractListener> listenerDefaultWorkerMap;
        internal CallerCoreMobile main;
        private ILogger loggerWorker;
        private static readonly object padlock = new object();
        private static List<Assembly> AssembliesList;
        //private Logger Logger = Logger.getLogger(); 
        /// <summary>
        /// Costruisce un nuovo enterprise caller con il file di inizializzazione specificato.
        /// </summary>
        /// <param name="iniFileName">	il file di inizializzazione, contenente il mapping tra nome funzione
        /// 						e classe di implementazione. </param>
        public FunctionCaller(CallerCoreMobile mainCaller)
        {

            //this.dba = dba;
            this.main = mainCaller;
            /// <summary>
            /// contine le funzioni personalizzate per tipo applicazione: numero_applicazione ed hashtable delle funzioni assegnate 2={IgnitionManager=2.IgnitionManager} </summary>
            //this.functionsMap = new Dictionary<string, Dictionary<string, EnterpriseFunction>>();
            /// <summary>
            /// Contiene le funzioni con NomeFunzione e La classe funzione es; GpsWorker=default.GpsWorker </summary>
            this.defaultApplicationMap = new Dictionary<string, EnterpriseFunction>();
            /// <summary>
            /// Contiene le funzioni con MsgType ed Handler  </summary>
            this.defaultHandlersMap = new Dictionary<string, MessageHandler>();
            /// <summary>
            /// Contiene le funzioni con MsgType ed Handler non usato </summary>
            //this.handlersMap = new Dictionary<string, Dictionary<string, MessageHandler>>();

            this.listenerMapDataType = new Dictionary<string, List<AbstractListener>>();
            this.listenerDefaultWorkerMap = new Dictionary<string, AbstractListener>();

            this.registerLogger(main.infoctx);
        }

        /// <summary>
        /// Ricarica le funzioni predefinite del sistema.
        /// </summary>
        /// <exception cref="Exception"> </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public void reloadFunctions(InfoContext info) throws Exception
        public virtual void reloadFunctions(IInfoContext info)
        {
            reloadFunctions(info.TypeApplication);
        }

        /// <summary>
        /// Ricarica le funzioni del sistema specificate in un file INI.
        /// </summary>
        /// <exception cref="Exception"> </exception>
        public virtual void reloadFunctions(string typeApplication)
        {
            lock (this)
            {

            }
        }

        public MethodInfo GetMethod(Type type, string name)
        {
            return GetMethods(type, BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .FirstOrDefault(m => m.Name == name);
        }

        public IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags flags)
        {
            var properties = type.GetTypeInfo().DeclaredMethods;
            if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
            {
                properties = type.GetRuntimeMethods();
            }

            return properties
                .Where(m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)
                .Where(m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)
                .Where(m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic);
        }

        private bool IsAsyncMethod(Type classType, string methodName)
        {
            // Obtain the method with the specified name.
            MethodInfo method = GetMethod(classType, methodName);//classType.GetMethod(methodName);
            Type attType = typeof(AsyncStateMachineAttribute);
            // Obtain the custom attribute for the method. 
            // The value returned contains the StateMachineType property. 
            // Null is returned if the attribute isn't present for the method. 
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);
            return (attrib != null);
        }

        private static bool IsSubclassOfRawGeneric(Type derivedType, string search)
        {
            while (derivedType != null && derivedType != typeof(object))
            {
                var currentType = derivedType.IsConstructedGenericType ? derivedType.GetGenericTypeDefinition() : derivedType;
                if (currentType.FullName.Contains(search))
                {
                    return true;
                }

                derivedType = derivedType.GetTypeInfo().BaseType;
            }
            return false;
        }

        public virtual void loadListeners(IInfoContext infoctx, List<ListenerProps> baselistener)
        {
            lock (this)
            {
                getLogger().Debug("EnterpriseCaller: loading listener...");
                int listenerCounter = 0;
                List<string> listenerlist = new List<string>();
                for (int i = 0; i < baselistener.Count; i++)
                {
                    try
                    {
                        ListenerProps lps = baselistener[i];
                        string companyName = lps.appType;
                        string functionName = lps.functionName;
                        string className = lps.className;
                        //string[] datatype = lps.dataTypes.ToArray();

                        if ((companyName.CompareTo("default") == 0 || companyName.CompareTo(infoctx.TypeApplication) == 0) && !listenerlist.Contains(functionName))
                        {
                            listenerlist.Add(functionName);
                            addListener(infoctx, companyName, className, functionName);
                            listenerCounter++;
                        }
                    }
                    catch (Exception e)
                    {
                        getLogger().Error(e);
                    }
                    finally
                    {

                    }
                }
                CleanAssemblyCache();
                getLogger().Debug("Regular Listener " + listenerCounter + " handlers loaded");

            }
        }

        /// <summary>
        /// Carica le funzioni predefinite del sistema.
        /// </summary>
        /// <exception cref="Exception">	se si verifica un errore durante il caricamento. </exception>
        public virtual void loadFunctions(IInfoContext infoctx, List<FunctionProps> basefunc)
        {
            lock (this)
            {
                getLogger().Debug("EnterpriseCaller: loading enterprise functions...");
                int functionsCounter = 0;
                int handlersCounter = 0;
                List<string> functionlist = new List<string>();
                EnterpriseFunction function;

                for (int i = 0; i < basefunc.Count; i++)
                {
                    try
                    {
                        FunctionProps fps = basefunc[i];
                        string companyName = fps.appType;
                        string functionName = fps.functionName;
                        string classFuncName = fps.className;
                        if ((companyName.CompareTo("default") == 0 || companyName.CompareTo(infoctx.TypeApplication) == 0) && !functionlist.Contains(functionName))
                        {
                            functionlist.Add(functionName);
                            function = addFunction(infoctx, companyName, classFuncName, functionName);
                            functionsCounter++;
                            if (function is MessageHandler)
                            {
                                handlersCounter++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        getLogger().Error(e);
                    }
                    finally
                    {
                    }
                }
                CleanAssemblyCache();
                getLogger().Debug("Regular " + functionsCounter + " functions and " + handlersCounter + " handlers loaded");
            }
        }

        public bool isLoadable(IInfoContext infoctx, string applicationType)
        {
            if (applicationType.CompareTo("default") == 0 || applicationType.CompareTo(infoctx.TypeApplication) == 0 || (applicationType.EndsWith("*") && infoctx.TypeApplication.StartsWith(applicationType.Substring(0, applicationType.Length - 1))))
            {
                return true;
            }
            return false;
        }

        public virtual EnterpriseFunction addFunction(IInfoContext infoctx, string applicationType, string className)
        {
            return addFunction(infoctx, applicationType, className, null);
        }

        public virtual EnterpriseFunction addFunction<T>(IInfoContext infoctx, string applicationType) where T : EnterpriseFunction, new()
        {
            return addFunction<T>(infoctx, applicationType, null);
        }



        /// <summary>
        /// Carica una funzione nel sistema.
        /// </summary>
        /// <param name="companyName">	il nome dell'azienda. </param>
        /// <param name="functionName">	il nome della funzione. </param>
        /// <param name="className">		la classe che imlementa la funzione. </param>
        /// <param name="properties">	eventuali propriet� della funzione.
        /// 
        /// @return				la funzione creata.
        /// </param>
        /// <exception cref="Exception">	se si verifica un errore. </exception>
        protected virtual EnterpriseFunction addFunction(IInfoContext infoctx, string applicationType, string className, string functionName)
        {
            if (isLoadable(infoctx, applicationType))
            {
                Type clazz = GetType(className);
                return addFunction(clazz, applicationType, functionName);
            }
            return null;
        }

        protected virtual EnterpriseFunction addFunction<T>(IInfoContext infoctx, string applicationType, string functionName) where T : EnterpriseFunction, new()
        {
            if (isLoadable(infoctx, applicationType))
            {
                Type clazz = typeof(T);
                return addFunction(clazz, applicationType, functionName);
            }
            return null;
        }



        protected virtual EnterpriseFunction addFunction(Type clazz, string applicationType, string functionName)
        {
            EnterpriseFunction function = Activator.CreateInstance(clazz) as EnterpriseFunction;
            if (functionName == null) functionName = function.getName();

            if (IsSubclassOfRawGeneric(clazz, "AbstractEnterpriseAsync"))
                function.setHasAsynMethod(IsAsyncMethod(clazz, "executeAsync"));
            else
                function.setHasAsynMethod(IsAsyncMethod(clazz, "executeGenericAsync"));

            EnterpriseFunction functionloaded = LoadFunction(function, applicationType, functionName, null);
            return functionloaded;
        }

        protected virtual EnterpriseFunction LoadFunction(EnterpriseFunction function, string applicationType, string functionName, string[] properties)
        {
            function.Load(applicationType + "." + functionName, main);
            if (function is MessageHandler)
            {
                MessageHandler handler = (MessageHandler)function;
                string[] msgTypes = handler.getTargetTypes();
                for (int i = 0; i < msgTypes.Length; i++)
                {
                    string msgType = msgTypes[i];
                    MessageHandler otherHandler;
                    defaultHandlersMap.TryGetValue(msgType, out otherHandler);
                    if (otherHandler != null)
                    {
                        getLogger().Warning("MessageHandler " + msgType + " just defined. It will be assigned from " + otherHandler.getName() + " to " + handler.getName());
                    }
                    defaultHandlersMap[msgType] = handler;
                }
            }
            defaultApplicationMap[functionName] = function;
            return function;

        }

        //        public virtual EnterpriseFunction LoadFunctionServer( EnterpriseFunction function,string applicationType, string functionName, string[] properties)
        //        {
        //			function.init(applicationType + "." + functionName, main);
        //
        //            if (function is MessageHandler)
        //            {
        //        	    MessageHandler handler = (MessageHandler)function;
        //                string[] msgTypes = handler.getTargetTypes();
        //                for (int i = 0; i < msgTypes.Length; i++)
        //                {
        //                    string msgType = msgTypes[i];
        //                    if (applicationType.Equals("default", StringComparison.CurrentCultureIgnoreCase))
        //                    {
        //                        MessageHandler otherHandler;
        //                        defaultHandlersMap.TryGetValue(msgType, out otherHandler);
        //                        if (otherHandler != null)
        //                        {
        //							getLogger().Error("******************** ERROR HANDLER JUST DEFINED *****************************");
        //                        }
        //                        else {
        //                        defaultHandlersMap[msgType] = handler;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //Dictionary<string, MessageHandler> companyMap = handlersMap.get(companyName);
        //						Dictionary<string, MessageHandler> applicationMap;
        //                        handlersMap.TryGetValue(applicationType, out applicationMap);
        //                        if (applicationMap == null)
        //                        {
        //                            applicationMap = new Dictionary<string, MessageHandler>();
        //                            handlersMap[applicationType] = applicationMap;
        //                        }
        //                        MessageHandler otherHandler;
        //                        applicationMap.TryGetValue(msgType, out otherHandler);
        //                        if (otherHandler != null)
        //                        {
        //							getLogger().Error("******************** ERROR HANDLER JUST DEFINED *****************************");
        //                        }
        //                        else
        //                        {
        //                            applicationMap[msgType] = handler;
        //                        }
        //                        
        //                    }
        //                }
        //            }
        //
        //			// Aggiunge la funzione alla mappa
        //			if (applicationType.Equals("default", StringComparison.CurrentCultureIgnoreCase))
        //			{
        //				defaultApplicationMap[functionName] = function;
        //			}
        //			else
        //			{
        //                //Dictionary<string, EnterpriseFunction> companyMap = (Dictionary<string, EnterpriseFunction>)functionsMap[companyName];
        //				Dictionary<string, EnterpriseFunction> applicationMap;
        //                functionsMap.TryGetValue(applicationType, out applicationMap);
        //				if (applicationMap == null)
        //				{
        //                    applicationMap = new Dictionary<string, EnterpriseFunction>();
        //					functionsMap[applicationType] = applicationMap;
        //				}
        //				applicationMap[functionName] = function;
        //			}
        //
        //			return function;
        //		}


        public virtual void removeFunction<T>() where T : EnterpriseFunction, new()
        {
            Type clazz = typeof(T);
            EnterpriseFunction function = Activator.CreateInstance(clazz) as EnterpriseFunction;
            removeFunction(function);
        }

        public virtual void removeFunction(IInfoContext info, string functionName)
        {
            EnterpriseFunction function = getFunction(info.TypeApplication, functionName);
            removeFunction(function);
        }

        public virtual void removeFunction(EnterpriseFunction function)
        {
            string functionName = function.getName();
            if (function is MessageHandler)
            {
                MessageHandler handler = (MessageHandler)function;
                string[] msgTypes = handler.getTargetTypes();
                for (int i = 0; i < msgTypes.Length; i++)
                {
                    string msgType = msgTypes[i];
                    defaultHandlersMap.Remove(msgType);
                }
            }
            defaultApplicationMap.Remove(functionName);
        }

        /// <summary>
        /// Call a function and return a result
        /// </summary>
        /// <param name="functionName">	il nome della funzione. </param>
        /// <param name="worker">		il worker che sta eseguendo la sessione. </param>
        /// <param name="context">		il contesto della chiamata.
        /// 
        /// @return				il risultato della chiamata.
        /// </param>
        /// <exception cref="FunctionNotFoundException">	se la funzione non � definita nel sistema. </exception>
        /// <exception cref="Exception">					se si verifica un errore. </exception>
        public Task<T> callFunctionAsync<T>(string functionName, IInfoContext info, FunctionContext context)
        {
            return callFunctionAsync<T>(getFunction(info.TypeApplication, functionName), info, context);
        }

        public object callFunction(string functionName, IInfoContext info, FunctionContext context)
        {
            return callFunction(getFunction(info.TypeApplication, functionName), info, context);
        }

        /// <summary>
        /// Chiama una funzione.
        /// </summary>
        /// <param name="function">		la funzione da chiamare. </param>
        /// <param name="worker">		il worker che sta eseguendo la sessione. </param>
        /// <param name="context">		il contesto della chiamata.
        /// 
        /// @return				il risultato della chiamata.
        /// </param>
        /// <exception cref="FunctionNotFoundException">	se la classe non � definita nel sistema. </exception>
        /// <exception cref="Exception">					se si verifica un errore. </exception>
        public Task<T> callFunctionAsync<T>(EnterpriseFunction function, IInfoContext info, FunctionContext context)
        {

#if DEBUG
            Stopwatch timeOperations = Stopwatch.StartNew();
#endif
            try
            {
                if (function.isAsyncMethod() || function.isYieldMethod())
                {
                    if (main.infoctx.DebugMode) getLogger().Debug("Calling Async function " + function.getName());
                    return function.executeGenericAsync<T>(info, context);
                }
                else
                {
                    if (main.infoctx.DebugMode) getLogger().Debug("Calling Awaitable function " + function.getName());
                    var t = Task.Run<T>(async () =>
                    {
                        return await function.executeGenericAsync<T>(info, context);
                    });
                    return t;


                }
            }
            finally
            {
#if DEBUG
                timeOperations.Stop();
                getLogger().Debug("Called functionAsync " + function.getName() + " in " + timeOperations.ElapsedMilliseconds + "ms");
#endif
            }
        }


        public object callFunction(EnterpriseFunction function, IInfoContext info, FunctionContext context)
        {

#if DEBUG
            Stopwatch timeOperations = Stopwatch.StartNew();
#endif
            try
            {
                if (main.infoctx.DebugMode) getLogger().Debug("Calling Regular function " + function.getName());
                return function.execute(info, context);
            }
            finally
            {
#if DEBUG
                timeOperations.Stop();
                getLogger().Debug("Called function " + function.getName() + " in " + timeOperations.ElapsedMilliseconds + "ms");
#endif
            }
        }
        public Task<T> callMessageHandlerAsync<T>(IInfoContext info, FunctionContext context)
        {
            MessageHandler messageHandler = getMessageHandler(info.TypeApplication, context.type);
            return callFunctionAsync<T>(messageHandler, info, context);
        }
        public object callMessageHandler(IInfoContext info, FunctionContext context)
        {
            MessageHandler messageHandler = getMessageHandler(info.TypeApplication, context.type);
            return callFunction(messageHandler, info, context);
        }
        /// <summary>
        /// Restituisce una funzione del sistema.
        /// </summary>
        /// <param name="company">		l'azienda proprietaria del terminale. </param>
        /// <param name="functionName">	il nome della funzione.
        /// 
        /// @return				la funzione. </param>
        protected virtual EnterpriseFunction getFunction(string applicationType, string functionName)
        {
            EnterpriseFunction functionCompany;
            if (!defaultApplicationMap.TryGetValue(functionName, out functionCompany))
            {
                getLogger().Error("Function " + functionName + " not found");
                throw new FunctionNotFoundException("Function " + functionName + " not found");
            }
            return functionCompany;
        }

        //		public virtual EnterpriseFunction getFunction(string applicationType, string functionName)
        //		{
        //			if (applicationType[0] == '*')
        //			{
        //                EnterpriseFunction functionCompany;
        //                defaultApplicationMap.TryGetValue(functionName, out functionCompany);
        //                return functionCompany;
        //				//return (EnterpriseFunction)defaultCompanyMap[functionName];
        //			}
        //			Dictionary<string, EnterpriseFunction> applicationMap;
        //            functionsMap.TryGetValue(applicationType, out applicationMap);
        //            //Dictionary<string, EnterpriseFunction> companyMap = (Dictionary<string, EnterpriseFunction>)functionsMap[company];
        //			if (applicationMap == null)
        //			{
        //                EnterpriseFunction functionCompany;
        //                defaultApplicationMap.TryGetValue(functionName, out functionCompany);
        //                return functionCompany;
        //			}
        //
        //			EnterpriseFunction function;
        //            applicationMap.TryGetValue(functionName, out function);
        //          
        //			if (function == null)
        //			{
        //                EnterpriseFunction functionCompany;
        //                defaultApplicationMap.TryGetValue(functionName, out functionCompany);
        //                return functionCompany;
        //				//return (EnterpriseFunction)defaultCompanyMap[functionName];
        //			}
        //
        //			return function;
        //		}

        //		public virtual bool typeApplicationHasFunction(string applicationType, string functionName)
        //		{
        //			Dictionary<string, EnterpriseFunction> applicationMap;
        //			functionsMap.TryGetValue(applicationType, out applicationMap);
        //			if (applicationMap == null) {
        //				return false;
        //			}
        //			EnterpriseFunction function;
        //			applicationMap.TryGetValue(functionName, out function);
        //			if (function == null) {
        //				return false;
        //			}
        //			return true;
        //		}

        /// <summary>
        /// Restituisce la funzione adeguata a gestire il tipo di messaggio ricevuto.
        /// </summary>
        /// <param name="company">		tipo dell'applicazione. </param>
        /// <param name="msgType">	il tipo di messaggio ricevuto.
        /// 
        /// @return				la funzione. </param>

        protected MessageHandler getMessageHandler(string applicationType, string msgType)
        {
            MessageHandler messageHandler;
            defaultHandlersMap.TryGetValue(msgType, out messageHandler);
            return messageHandler;
        }
        //        protected MessageHandler getMessageHandler(string applicationType, string msgType)
        //        {
        //            if (applicationType[0] == '*')
        //            {
        //
        //                MessageHandler messageHandler;
        //                defaultHandlersMap.TryGetValue(msgType, out messageHandler);
        //                return messageHandler;
        //            }
        //			Dictionary<string, MessageHandler> applicationMap;
        //            handlersMap.TryGetValue(applicationType, out applicationMap);
        //            if (applicationMap == null) {
        //                MessageHandler messageHandler;
        //                defaultHandlersMap.TryGetValue(msgType, out messageHandler);
        //                return messageHandler;
        //            }
        //            MessageHandler handler;
        //            applicationMap.TryGetValue(msgType, out handler);
        //            if (handler == null) {
        //                MessageHandler messageHandler;
        //                defaultHandlersMap.TryGetValue(msgType, out messageHandler);
        //                return messageHandler;
        //            }
        //            return handler;
        //        }


        public virtual AbstractListener addListener(IInfoContext infoctx, string applicationType, string className)
        {
            return addListener(infoctx, applicationType, className, null);
        }

        public virtual AbstractListener addListener<T>(IInfoContext infoctx, string applicationType) where T : AbstractListener, new()
        {
            return addListener<T>(infoctx, applicationType, null);
        }

        public virtual AbstractListener addListener(IInfoContext infoctx, string applicationType, string className, string listenerName)
        {
            if (isLoadable(infoctx, applicationType))
            {
                Type clazz = GetType(className);
                return addListener(clazz, applicationType, listenerName);
            }
            return null;
        }
        public virtual AbstractListener addListener<T>(IInfoContext infoctx, string applicationType, string listenerName) where T : AbstractListener, new()
        {
            if (isLoadable(infoctx, applicationType))
            {
                Type clazz = typeof(T);
                return addListener(clazz, applicationType, listenerName);
            }
            return null;

        }
        protected virtual AbstractListener addListener(Type clazz, string applicationType, string listenerName)
        {
            AbstractListener _listenerWorker = Activator.CreateInstance(clazz) as AbstractListener;
            if (listenerName == null) listenerName = _listenerWorker.getName();
            if (IsSubclassOfRawGeneric(clazz, "AbstractListenerAsync"))
                _listenerWorker.setHasAsynMethod(IsAsyncMethod(clazz, "addContextToAsyncQueue"));
            else
                _listenerWorker.setHasAsynMethod(IsAsyncMethod(clazz, "addContextToGenericAsyncQueue"));
            AbstractListener listenerloaded = LoadListener(_listenerWorker, applicationType, listenerName);
            return listenerloaded;
        }



        protected virtual AbstractListener LoadListener(AbstractListener listenerWorker, string applicationType, string listenerName)
        {
            try
            {
                if (typeApplicationHasListener(listenerName))
                {
                    getLogger().Error("Listener " + listenerName + " repeated. Request discarded.");
                    throw new RepeatedListenerNameException("Listener " + listenerName + " repeated. Request discarded.");
                }
                listenerWorker.init(main);
                listenerDefaultWorkerMap[listenerName] = listenerWorker;
                string[] datatype = listenerWorker.getListenerTypes();
                for (int count = 0; count < datatype.Length; count++)
                {
                    List<AbstractListener> existing;
                    if (!listenerMapDataType.TryGetValue(datatype[count], out existing))
                    {
                        existing = new List<AbstractListener>();
                        listenerMapDataType[datatype[count]] = existing;
                    }
                    // At this point we know that "existing" refers to the relevant list in the 
                    // dictionary, one way or another.
                    existing.Add(listenerWorker);
                }
            }
            catch (Exception e)
            {
                getLogger().Error(e);
            }

            return listenerWorker;
        }

        public virtual bool typeApplicationHasListener(string listenerName)
        {
            AbstractListener applicationMap;
            if (!listenerDefaultWorkerMap.TryGetValue(listenerName, out applicationMap))
            {
                return false;
            }
            return true;
        }

        public virtual AbstractListener getListener(string FunctionName)
        {
            AbstractListener ablistworker = (AbstractListener)listenerDefaultWorkerMap[FunctionName];
            return ablistworker;
        }


        private static Type GetType(string className)
        {
            var type = Type.GetType(className);
            if (type != null) return type;
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Searching " + className);
#endif
            if (AssembliesList == null) AssembliesList = GetAssemblies();
            foreach (var a in AssembliesList)
            {
                type = a.GetType(className);
                if (type != null)
                    return type;
            }
            return null;
        }
        public static void CleanAssemblyCache()
        {
            AssembliesList = null;
        }
        public static List<Assembly> GetAssemblies()
        {
            var currentDomain = typeof(string).GetTypeInfo().Assembly.GetType("System.AppDomain").GetRuntimeProperty("CurrentDomain").GetMethod.Invoke(null, new object[] { });
            var getAssemblies = currentDomain.GetType().GetRuntimeMethod("GetAssemblies", new Type[] { });
            var assemblies = getAssemblies.Invoke(currentDomain, new object[] { }) as Assembly[];
            //			var ignoreChecks = new List<string> () {
            //				"Microsoft.",
            //				"System.",
            //				"System,",
            //				"CR_ExtUnitTest",
            //				"mscorlib,",
            //				"CR_VSTest.",
            //				"DevExpress.CodeRush.",
            //				"Android.",
            //				"Java.",
            //				"Javax.",
            //				"Mono.",
            //				"Xamarin.",
            //			};
            //			foreach (Assembly assem in assemblies) {
            //				foreach (TypeInfo bb in assem.DefinedTypes) {
            //					if (bb.IsClass && !ignoreChecks.Any(exception =>	bb.FullName.StartsWith(exception)))
            //						Debug.WriteLine (bb.FullName);
            //				}
            //			}
            return assemblies.ToList();
        }

        //		Slow Method
        //		public static Type getTypeByClassName(string className){
        //			Type objectType = (from asm in GetAssemblies()
        //				from type in asm.DefinedTypes
        //				where type.IsClass && type.FullName == className
        //				select type.AsType()).Single ();
        //			return objectType;
        //		}
        //		public static Type getTypeByClassName(string typeName)
        //		{
        //	
        //			var type = Type.GetType(typeName);
        //			if (type != null) return type;
        //			foreach (var a in  GetAssemblies())
        //			{
        //				type = a.GetType(typeName);
        //				if (type != null)
        //					return type;
        //			}
        //			return null ;
        //		
        //		}


        public static T registerAsSingleton<T>(Type clazz, params object[] args)
        {
            Object t;
            if (args.Length == 0)
                t = Activator.CreateInstance(clazz) as Object;
            else
                t = Activator.CreateInstance(clazz, args) as Object;
            return (T)t;

        }

        public static T registerAsSingleton<T>(string className, params object[] args)
        {
            Type clazz = GetType(className);
            return FunctionCaller.registerAsSingleton<T>(clazz, args);

        }

        public virtual T registerAsSingleton<T>(IInfoContext infoctx, string applicationType, string className, params object[] args)
        {
            if (isLoadable(infoctx, applicationType))
            {
                return (T)FunctionCaller.registerAsSingleton<T>(className, args);

            }
            return default(T);
        }

        public virtual T registerAsSingleton<T, K>(IInfoContext infoctx, string applicationType, params object[] args)
        {
            if (isLoadable(infoctx, applicationType))
            {
                Type clazz = typeof(K);
                return (T)FunctionCaller.registerAsSingleton<T>(clazz, args);

            }
            return default(T);
        }

        protected ILogger registerLogger(IInfoContext infoctx)
        {
            return registerLogger("default", "CallerCore.MainCore.BaseLogger", infoctx);
        }

        protected virtual ILogger registerLogger(string applicationType, string className, IInfoContext infoctx)
        {
            return registerLogger(className, LogLevel.Info, infoctx);
        }

        public virtual ILogger registerLogger(IInfoContext infoctx, string applicationType, string className)
        {
            return registerLogger(infoctx, applicationType, className, LogLevel.Info);
        }
        public virtual ILogger registerLogger(IInfoContext infoctx, string applicationType, string className, LogLevel logLevel)
        {
            if (isLoadable(infoctx, applicationType))
            {
                return registerLogger(className, logLevel, infoctx);
            }
            return null;
        }
        public virtual ILogger registerLogger<T>(IInfoContext infoctx, string applicationType)
        {
            return registerLogger<T>(infoctx, applicationType, LogLevel.Info);
        }
        public virtual ILogger registerLogger<T>(IInfoContext infoctx, string applicationType, LogLevel logLevel)
        {
            if (isLoadable(infoctx, applicationType))
            {
                Type clazz = typeof(T);
                return registerLogger(clazz, logLevel, infoctx);
            }
            return null;
        }

        protected virtual ILogger registerLogger(string className, LogLevel logLevel, IInfoContext infoctx)
        {
            Type clazz = GetType(className);
            return registerLogger(clazz, logLevel, infoctx);
        }

        protected virtual ILogger registerLogger(Type clazz, LogLevel logLevel, IInfoContext infoctx)
        {
            lock (padlock)
            {
                this.loggerWorker = Activator.CreateInstance(clazz) as ILogger;
                this.loggerWorker.InitLog(infoctx);
                this.loggerWorker.SetLogLevel(logLevel);
                return this.loggerWorker;
            }
        }

        public virtual ILogger getLogger()
        {
            lock (padlock)
            {
                return this.loggerWorker;
            }
        }
        public virtual Dictionary<string, Task<object>> dispatchMessageToAsyncListenersGetTasks(FunctionContext context)
        {

            Dictionary<string, Task<object>> tasksDict = new Dictionary<string, Task<object>>();
            if (listenerMapDataType.ContainsKey(context.type))
            {
                for (int i = 0; i < listenerMapDataType[context.type].Count; i++)
                {

#if DEBUG
                    Stopwatch timeOperations = Stopwatch.StartNew();
#endif
                    AbstractListener ablistworker = (AbstractListener)listenerMapDataType[context.type][i];
                    try
                    {
                        Task<object> task;
                        if (ablistworker.isAsyncMethod() || ablistworker.isYieldMethod())
                        {
                            if (main.infoctx.DebugMode) getLogger().Debug("Calling Async dispatch " + ablistworker.getName());
                            task = ablistworker.addContextToGenericAsyncQueue(context);
                        }
                        else
                        {
                            if (main.infoctx.DebugMode) getLogger().Debug("Calling Awaitable dispatch " + ablistworker.getName());
                            task = Task.Run<object>(async () =>
                            {
                                return await ablistworker.addContextToGenericAsyncQueue(context);
                            });
                        }
                        tasksDict.Add(ablistworker.getName(), task);
                    }
                    finally
                    {
#if DEBUG
                        timeOperations.Stop();
                        getLogger().Debug("Dispatched MessageAsync to " + ablistworker.getName() + " in " + timeOperations.ElapsedMilliseconds + "ms");
#endif
                    }
                }
            }
            else
                getLogger().Warning("Not Found listener for " + context.type);

            return tasksDict;
        }

        public virtual async Task<Dictionary<string, object>> dispatchMessageToAsyncListeners(FunctionContext context)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            var tasksDict = dispatchMessageToAsyncListenersGetTasks(context);
            List<Task<object>> tasks = tasksDict.Values.ToList();
            string name;
            while (tasks.Count > 0)
            {
                var t = await Task.WhenAny(tasks);
                tasks.Remove(t);
                await t;
                name = tasksDict.FirstOrDefault(x => x.Value.Id == t.Id).Key;
                result.Add(name, t.Result);
            }
            return result;

        }




        public virtual Dictionary<string, object> dispatchMessageToListeners(FunctionContext context)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (listenerMapDataType.ContainsKey(context.type))
            {
                for (int i = 0; i < listenerMapDataType[context.type].Count; i++)
                {
#if DEBUG
                    Stopwatch timeOperations = Stopwatch.StartNew();
#endif
                    AbstractListener ablistworker = (AbstractListener)listenerMapDataType[context.type][i];
                    try
                    {
                        object ret = ablistworker.addContextToQueue(context);
                        result.Add(ablistworker.getName(), ret);
                    }
                    finally
                    {
#if DEBUG
                        timeOperations.Stop();
                        getLogger().Debug("Dispatched message to " + ablistworker.getName() + " in " + timeOperations.ElapsedMilliseconds + "ms");
#endif
                    }
                }
            }
            else
                getLogger().Warning("Not Found listener for " + context.type);
            return result;

        }

        public virtual void dispatchMessageToScheduledExecution(FunctionContext context)
        {

            var tasksDict = new Task(() => dispatchMessageToAsyncListenersGetTasks(context));
            tasksDict.Start();
        }

        public virtual void dispatchMessageToSingleScheduledExecution(FunctionContext context, string functionName)
        {

            var tasksSingle = new Task(() => dispatchMessageToSingleListener(context, functionName));
            tasksSingle.Start();
        }

        public virtual Task<T> dispatchMessageToAsyncSingleListener<T>(FunctionContext context, string functionName)
        {
            var tcs = new TaskCompletionSource<T>();
            dispatchMessageToAsyncSingleListener(context, functionName).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    // faulted with exception
                    Exception ex = t.Exception;
                    while (ex is AggregateException && ex.InnerException != null)
                        ex = ex.InnerException;
                    tcs.TrySetException(ex);
                }
                else if (t.IsCanceled) tcs.TrySetCanceled();
                else tcs.TrySetResult((T)t.Result);
            }
            );
            return tcs.Task;
            //.ContinueWith<T> (t => (T)(object)t.Result);
        }

        public virtual Task<object> dispatchMessageToAsyncSingleListener(FunctionContext context, string functionName)
        {
            /*return  Task.Run<T>(() => {
				var result=dispatchMessageToSingleListener(context,functionName);
				if (result!=null) return (T)result;
				else return default(T);
			});
			*/
            if (listenerMapDataType.ContainsKey(context.type))
            {
                AbstractListener ablistworker = getListener(functionName);
                if (ablistworker != null)
                {
#if DEBUG
                    Stopwatch timeOperations = Stopwatch.StartNew();
#endif
                    try
                    {
                        Task<object> task;
                        if (ablistworker.isAsyncMethod() || ablistworker.isYieldMethod())
                        {
                            if (main.infoctx.DebugMode) getLogger().Debug("Calling Async dispatch " + ablistworker.getName());
                            task = ablistworker.addContextToGenericAsyncQueue(context);
                        }
                        else
                        {
                            if (main.infoctx.DebugMode) getLogger().Debug("Calling Awaitable dispatch " + ablistworker.getName());
                            task = Task.Run<object>(async () =>
                            {
                                return await ablistworker.addContextToGenericAsyncQueue(context);
                            });
                        }
                        return task;
                    }
                    finally
                    {
#if DEBUG
                        timeOperations.Stop();
                        getLogger().Debug("Dispatched messageAsyc to " + ablistworker.getName() + " in " + timeOperations.ElapsedMilliseconds + "ms");
#endif
                    }
                }
                else
                    getLogger().Warning("Not Found listener for function " + functionName);

            }
            else
                getLogger().Warning("Not Found listener for " + context.type);

            return null;

        }

        public virtual T dispatchMessageToSingleListener<T>(FunctionContext context, string functionName)
        {
            return (T)dispatchMessageToSingleListener(context, functionName);
        }

        public virtual object dispatchMessageToSingleListener(FunctionContext context, string functionName)
        {

            if (listenerMapDataType.ContainsKey(context.type))
            {
                AbstractListener ablistworker = getListener(functionName);
                if (ablistworker != null)
                {
#if DEBUG
                    Stopwatch timeOperations = Stopwatch.StartNew();
#endif
                    try
                    {
                        return ablistworker.addContextToQueue(context);
                    }
                    finally
                    {
#if DEBUG
                        timeOperations.Stop();
                        getLogger().Debug("Dispatched message to " + ablistworker.getName() + " in " + timeOperations.ElapsedMilliseconds + "ms");
#endif
                    }
                }
                else
                    getLogger().Warning("Not Found listener for function " + functionName);
            }
            else
                getLogger().Warning("Not Found listener for " + context.type);

            return null;

        }

    }

    [Flags]
    public enum BindingFlags
    {
        None = 0,
        Instance = 1,
        Public = 2,
        Static = 4,
        FlattenHierarchy = 8,
        SetProperty = 8192
    }
}