﻿namespace Selp.Interfaces
{
	using System.Web.Http;
	using Entities;

	// Зоны ответственности: ошибки, маппинг сущностей
	public interface ISelpController<in TModel, TEntity, in TKey> where TModel : ISelpEntitiy<TKey>
		where TEntity : ISelpEntitiy<TKey>
	{
		string ControllerName { get; }

		IHttpActionResult Get([FromUri] BaseFilter query);
		IHttpActionResult Get(TKey id);
		IHttpActionResult Post([FromBody] TModel value);

		IHttpActionResult Put(TKey id, [FromBody] TModel value);

		IHttpActionResult Delete(TKey id);
	}
}