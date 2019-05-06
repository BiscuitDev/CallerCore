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
	public class RepeatedListenerNameException : Exception
	{
		public RepeatedListenerNameException() : base()
		{
		}

		public RepeatedListenerNameException(string message) : base(message)
		{
		}
	}

}