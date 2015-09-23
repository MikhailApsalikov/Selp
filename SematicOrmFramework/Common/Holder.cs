﻿namespace SematicOrmFramework.Common
{
	internal sealed class Holder<T1, T2>
	{
		public T1 item1;
		public T2 item2;

		public Holder(T1 item1, T2 item2)
		{
			this.item1 = item1;
			this.item2 = item2;
		}
	}
}