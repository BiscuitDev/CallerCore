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
	public class FunctionNotFoundException : Exception
	{
		public FunctionNotFoundException() : base()
		{
		}

		public FunctionNotFoundException(string message) : base(message)
		{
		}
	}

}