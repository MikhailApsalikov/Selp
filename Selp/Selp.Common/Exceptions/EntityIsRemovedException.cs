namespace Selp.Common.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class EntityIsRemovedException : EntityNotFoundException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public EntityIsRemovedException()
		{
		}

		public EntityIsRemovedException(string message) : base(message)
		{
		}

		public EntityIsRemovedException(string message, Exception inner) : base(message, inner)
		{
		}

		protected EntityIsRemovedException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}