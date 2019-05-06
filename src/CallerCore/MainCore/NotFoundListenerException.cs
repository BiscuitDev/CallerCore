using System;

/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>
namespace CallerCore.MainCore
{

	/// <summary>
	/// @author BiscuitDev
	/// </summary>
	public class NotFoundListenerException : Exception
	{
		public NotFoundListenerException() : base()
		{
		}

		public NotFoundListenerException(string message) : base(message)
		{
		}
	}

}