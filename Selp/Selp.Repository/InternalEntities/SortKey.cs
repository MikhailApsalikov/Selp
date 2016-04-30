namespace Selp.Repository.InternalEntities
{
	using System;

	internal sealed class SortKey
	{
		private readonly int _hashCode;
		private readonly string _property;
		private readonly Type _type;

		internal SortKey(Type type, string property)
		{
			_type = type;
			_property = property;

			_hashCode = CalculateHashCode(type, property);
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;

			if (ReferenceEquals(null, obj) || obj.GetType() != typeof(SortKey))
				return false;

			return Equals((SortKey) obj);
		}

		public bool Equals(SortKey other)
		{
			if (ReferenceEquals(this, other))
				return true;

			if (ReferenceEquals(null, other))
				return false;

			return Equals(_type, other._type)
			       && string.Equals(_property, other._property, StringComparison.Ordinal);
		}

		private static int CalculateHashCode(Type type, string property)
		{
			unchecked
			{
				int result = type.GetHashCode();
				if (!string.IsNullOrEmpty(property))
				{
					result = result*397 + property.GetHashCode();
				}
				return result;
			}
		}
	}
}