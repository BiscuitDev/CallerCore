/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>
namespace CallerCore.MainCore
{

	/// <summary>
	/// @author BiscuitDev
	/// </summary>
	public interface MessageHandler : EnterpriseFunction
	{
		/// <summary>
		/// Restituisce la lista dei tipi di messaggio supportati.
		/// </summary>
		/// <returns> la lista dei tipi di messaggio supportati. </returns>
        string[] getTargetTypes();

		/// <summary>
		/// Restituisce <tt>true</tt> se questo handler esegue il salvataggio nella cache delle risposte
		/// da riutilizzare in caso di messaggio duplicato, cio� con lo stesso numero di trasmissione.
		/// </summary>
		/// <returns> <tt>true</tt> se questo handler esegue il salvataggio nella cache delle risposte; <tt>false</tt> altrimenti. </returns>
		//bool checkRepeatedNumber();

        
    }

}