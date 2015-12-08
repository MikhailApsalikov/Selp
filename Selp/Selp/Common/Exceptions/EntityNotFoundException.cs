namespace Selp.Common.Exceptions
{
	using System;

	public class EntityNotFoundException : SelpException
	{
		public EntityNotFoundException()
		{
		}

		public EntityNotFoundException(string message)
			: base(message)
		{
		}

		public EntityNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}