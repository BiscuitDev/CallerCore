/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>
using System.Threading.Tasks;

namespace CallerCore.MainCore
{



	/// <summary>
	/// Interfaccia comune per tutti i worker del sistema. Un worker � un thread
	/// che gestisce un sessione di un client o che svolge una singola operazione,
	/// come una <code>BusinessFunction</code>.
	/// 
	/// @author BiscuitDev
	/// </summary>
	public interface EnterpriseListener 
	{
		/// <summary>
		/// Inizializza tutte le variabili del Worker.
		/// </summary>
	    void init(CallerCoreMobile main);
	    object addContextToQueue(FunctionContext fctx);
		Task<object> addContextToGenericAsyncQueue(FunctionContext context);
		void setHasAsynMethod(bool hasasync);
		bool isAsyncMethod ();
		bool isYieldMethod();
	}


}