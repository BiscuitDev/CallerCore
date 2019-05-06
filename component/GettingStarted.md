CallerCore allows the developers to create pluggable components
CallerCore is based on provider design pattern and Inversion of Control Container (IoC)

To start we must register the functions. Every register is simply a contract between a Function and the Business Logic/Data Abstraction Layer. 

CallerCore is the responsibility of the function to contain the implementation for that method, calling whatever Business Logic Layer  is necessary

A Function implementation must derive from an abstract base class, which is used to define a contract for a particular feature
The class that inherits from the abstract base class will do some of the work and the abstract base class will do some of the work.
Every type of function listed above has his abastract that share a common base type. We have three type of functions:

A) EnterpriseFunction:
* The abstract are AbstractEnterpriseAsync (to implement your method async), AbstractEnterpriseRegular (to implement your regular method), AbstractEnterpriseFunction (virtual implementation of your method async e regular)
* callable by callFunction or callFunctionAsync. 

B) MessageHandler:
* The abstract is AbstractMessageHandler
* callable by callMessageHandler or callMessageHandlerAsync (every function are automatically called using the member type of FunctionContext)

C) EnterpriseListener:
* The abstract is AbstractListener
* The messages are dispatched by its method addContextToQueue


Every call to a classFunction has the mainClass (interface IInfoContext) and FunctionContext that permit to pass parameter from a function to another function.
With FunctionContext we can pass parameters string,class T,int,object ...

Every classFunction can be called in every part (pcl and android or pcl and iOS) of your project independently where it is registered
 
Defince Main Class (interface IInfoContext), register some functions and call them

```csharp
using CallerCore.MainCore;
...
public class MainCore : IInfoContext
{
public  CallerCoreMobile mmc { get; set; }

public MainCoreiOS ()
{
	TypeApplication = "iOS1";
	DebugMode = true;
	mmc = new CallerCoreMobile (this);
	Bootstrap();
}

public Bootstrap()
{
	mmc.RegisterAsFunction<Connectivity>();
	mmc.RegisterAsFunction<HandlerSystemVersion>();
	mmc.RegisterAsFunction<PrintCounter> (CallerCoreMobile.DEFAULT);

	mmc.RegisterAsListener<ListenerPrintString> (CallerCoreMobile.DEFAULT);

	ISampleSingleton iss=mmc.RegisterAsSingleton<ISampleSingleton, SampleSingleton>("one", "two");

	/**
	 * other examples as you can register functions in your PCL  project using type application to load functions/listeners/handlers independently where they are implemented
	 * mmc.RegisterAsFunction ("Droid1", "CallerCoreSample.Droid.Connectivity");
	 * mmc.RegisterAsFunction ("iOS1", "CallerCoreSample.iOS.Connectivity");
	 * mmc.RegisterAsFunction ("iOS1", "CallerCoreSample.iOS.Connectivity,CallerCoreSample.iOS");
	 * mmc.RegisterAsFunction ("Droid1", "CallerCoreSample.Droid.HandlerSystemVersion");
	 * mmc.RegisterAsFunction ("iOS1", "CallerCoreSample.iOS.HandlerSystemVersion");
	 * mmc.RegisterAsFunction<PrintCounter> (CallerCoreMobile.DEFAULT);  //usually functions implented in your PCL project are default (for both projects iOS & Android)
	 * mmc.RegisterAsListener<ListenerPrintString> (CallerCoreMobile.DEFAULT);
	 * mmc.RegisterAsSingleton<ISampleSingleton> ("iOS1", ref iss,"CallerCoreSample.iOS.SampleSingleton", "one", "two");
	 * mmc.RegisterAsSingleton<ISampleSingleton> ("Droid1", ref iss,"CallerCoreSample.Droid.SampleSingleton", "one", "two");
	**/
}

public exampleEnterpriseListener()
{
	FunctionContext fctx=new FunctionContext().AddType(MyListenerTypes.RLOG).AddParam("myparam1", "Hello").AddParam("myparam2",123);
	main.mmc.DispatchToListeners (fctx);
}

public async Task exampleEnterpriseFunction()
{
	await main.mmc.CallFunctionAsync<bool> (Connectivity.NAME);
}

public void exampleMessageHandler()
{
	mmc.CallMessageHandler (new FunctionContext (MyHandlerTypes.SV));
}	
```

Implement an Async Function

```csharp
	public class Connectivity: AbstractEnterpriseAsync<bool>
	{
		public static string NAME = "Connectivity";

		#region implemented abstract members of AbstractEnterpriseAsync

		public override async Task<bool> executeAsync (IInfoContext info, FunctionContext context)
		{
		 ...
		 
		}

		#endregion


		public override string getName()
		{
			return NAME;
		}
	}
```

Implement an andler

```csharp
	public class HandlerSystemVersion : AbstractMessageHandler
	{
		public static string NAME = "SystemVersion";
		public static string[] TARGET_TYPES = { "SV" };

		public override object execute(IInfoContext info, FunctionContext context)
		{
			...
			return null;
		}

		public override string getName()
		{
			return NAME;
		}

		public override string[] getTargetTypes()
		{
			return TARGET_TYPES;
		}
	}

```

Implement a Listener

```csharp

	public class ListenerPrintString : AbstractListenerRegular
	{

		public static string[] LISTENER_TYPES = { "RLOG","ADD" };
		public static string NAME = "ListenerPrintString";	
		MainCorePcl mainAdapter;

		public override void init(CallerCoreMobile main)
		{
			base.init(main);
			mainAdapter = (MainCorePcl)info;
		}

		#region implemented abstract members of AbstractListener

		public override string getName ()
		{
			return NAME;
		}

		public override string[] getListenerTypes ()
		{
			return LISTENER_TYPES;
		}

		#endregion

		#region implemented abstract members of AbstractListenerRegular

		public override object addContextToQueue (FunctionContext fctx)
		{
			log (LogLevel.Debug,"[Command] " + fctx.type);
			...
			return null;
		}

		#endregion


	}

```

We have different way to pass different objects to a function or to a listener or to an handler but it's necessary FunctionContext class
FunctionContext has 3 header:
type: string or enum (usually ID to dispatch FunctionContext to a listener or handler)
optionals are
	params_Capacity: int parameter (to allocate array size to pass parameters)
	msg: string parameter


```csharp

FunctionContext fctx=new FunctionContext().AddType(Enum.RLOG).AddParam(0, "Hello").AddParam(1,123);

FunctionContext fctx=new FunctionContext(Enum.RLOG));
fctx.setStringParam (0,"Hello");
fctx.setIntParam ("numberExample",123);

FunctionContext fctx=new FunctionContext("RLOG").AddParam("myparam", "Hello");
fctx.setParam (1,123);

FunctionContext fctx=FunctionContext.NewFunctionContext("RLOG","This is a message").AddParam(0, "Hello");

//To read parameters FunctionContext

string first_parameter=fctx.getStringParam ("myparam");
int second_parameter=fctx.getIntParam (1);
//or
string first_parameter=fctx.getObject<string>("myparam");
int second_parameter=(int)fctx.getParam (1);
```

## Other Resources

* [Component Documentation](https://github.com/BiscuitDev/CallerCore)
* [Support Forums](https://github.com/BiscuitDev/CallerCore)
* [Source Code Repository](https://github.com/BiscuitDev/CallerCore)
