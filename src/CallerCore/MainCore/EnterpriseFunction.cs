/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>

using System.Threading.Tasks;
namespace CallerCore.MainCore
{




	/// <summary>
	/// Interfaccia per tutte le funzioni del sistema che ne imlpementano la business logic.
	/// 
	/// @author BiscuitDev
	/// </summary>
	public interface EnterpriseFunction
	{
		/// <summary>
		/// Inizializza la funzione.
		/// </summary>
		/// <param name="fullName">	il nome completo della funzione. </param>
		/// <param name="server">	la classe principale del sistema. </param>
		void Load(string fullName, CallerCoreMobile main);
		string getName();
		/// <summary>
		/// Esegue la funzione stessa.
		/// </summary>
		/// <param name="worker">		il worker thread che esegue la funzione. </param>
		/// <param name="context">		il contesto della chiamata.
		/// 
		/// @return				il risultato.
		/// </param>
		/// <exception cref="Exception">	se si verifica un errore. </exception>
  		 object execute(IInfoContext info, FunctionContext context);
         Task<T> executeGenericAsync<T>(IInfoContext info, FunctionContext context);
		 void setHasAsynMethod(bool hasasync);
		 bool isAsyncMethod ();
		 bool isYieldMethod();
		/// <summary>
		/// Imposta una propriet� della funzione.
		/// </summary>
		/// <param name="name">	il nome della propriet�. </param>
		/// <param name="value">	il valore della propriet�. </param>

	}

}