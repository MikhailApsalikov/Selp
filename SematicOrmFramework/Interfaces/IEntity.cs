﻿namespace SematicOrmFramework.Interfaces
{
	public interface IEntity<TId>
	{
		TId Id { get; set; }
	}
}