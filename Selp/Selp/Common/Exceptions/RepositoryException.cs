namespace Selp.Common.Exceptions
{
	using System;

	public class RepositoryException : SelpException
	{
		public RepositoryException()
		{
		}

		public RepositoryException(string message)
			: base(message)
		{
		}

		public RepositoryException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}