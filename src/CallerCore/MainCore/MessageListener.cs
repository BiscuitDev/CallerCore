/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>
namespace CallerCore.MainCore
{

	/// <summary>
	/// Un message listener � un oggetto che pu� registrarsi come listener
	/// delle chiamate ad una particolare funzione del sistema.<b>
	/// Quando la funzione su cui il message listener si � registrato viene
	/// chiamata, tale funzione pu� chiamare il metodo <tt>dispatchMessageToListeners</tt>,
	/// che provvede a chiamare il metodo <tt>receivedMessageEvent</tt> di tutti i message
	/// listener registrati. 
	/// 
	/// @author BiscuitDev
	/// </summary>
	public interface MessageListener
	{
		/// <summary>
		/// Metodo chiamato quando la funzione su cui questo listener � registrato
		/// viene chiamata.
		/// </summary>
		/// <param name="ctx">	il contesto di esecuzione della chiamata. </param>
		void receivedMessageEvent(FunctionContext fctx);


	}

}