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
	public class RepeatedHandlerException : Exception
	{
		public RepeatedHandlerException() : base()
		{
		}

		public RepeatedHandlerException(string message) : base(message)
		{
		}
	}

}