using System;
using CallerCoreSample.PCL;

namespace CallerCoreSample.iOS
{
	public class SampleSingleton : ISampleSingleton
	{
		private string one;
		private string two;
		public SampleSingleton (string one,string two)
		{
			this.one = one;
			this.two = two;
		}

		#region ISampleSingleton implementation
		public string SampleResult ()
		{
			return one+" - "+two;
		}
		#endregion
	}
}

