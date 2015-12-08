namespace Selp.Common.Exceptions
{
	using System;


	/// <summary>
	/// Base expection of the framework
	/// </summary>
	public class SelpException : Exception
	{
		public SelpException()
		{
		}

		public SelpException(string message)
			: base(message)
		{
		}

		public SelpException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}