namespace Selp.Common.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	public class WorkflowException : Exception
	{
		public WorkflowException()
		{
		}

		public WorkflowException(string message) : base(message)
		{
		}

		public WorkflowException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected WorkflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}