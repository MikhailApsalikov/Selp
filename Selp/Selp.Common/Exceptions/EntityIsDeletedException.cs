namespace Selp.Common.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class EntityIsDeletedException : EntityNotFoundException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public EntityIsDeletedException()
		{
		}

		public EntityIsDeletedException(string message) : base(message)
		{
		}

		public EntityIsDeletedException(string message, Exception inner) : base(message, inner)
		{
		}

		protected EntityIsDeletedException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}