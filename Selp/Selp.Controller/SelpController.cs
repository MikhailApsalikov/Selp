namespace Selp.Controller
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using Common.Entities;
	using Common.Exceptions;
	using Entities;
	using Interfaces;

	public abstract class SelpController<TShortModel, TModel, TEntity, TKey> : ApiController, ISelpController<TModel, TKey>
		where TShortModel : ISelpEntity<TKey> where TModel : ISelpEntity<TKey> where TEntity : ISelpEntity<TKey>
	{
		protected SelpController(ISelpRepository<TEntity, TKey> repository)
		{
			Repository = repository;
		}

		protected ISelpRepository<TEntity, TKey> Repository { get; }

		public abstract string ControllerName { get; }

		[HttpDelete]
		public virtual IHttpActionResult Delete(TKey id)
		{
			try
			{
				Repository.Remove(id);
				return Ok();
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		[HttpGet]
		public virtual IHttpActionResult Get(TKey id)
		{
			try
			{
				return Ok(MapEntityToModel(Repository.GetById(id)));
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		[HttpGet]
		public virtual IHttpActionResult Get([FromUri] BaseFilter query)
		{
			try
			{
				int total;
				List<TShortModel> data = Repository.GetByFilter(query, out total).Select(MapEntityToShortModel).ToList();

				return Ok(new EntitiesListResult<TShortModel>()
				{
					Data = data,
					Page = query.Page ?? -1,
					PageSize = query.PageSize ?? -1,
					Total = total
				});
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		[HttpPost]
		public virtual IHttpActionResult Post([FromBody] TModel value)
		{
			try
			{
				RepositoryModifyResult<TEntity> result = Repository.Create(MapModelToEntity(value));
				if (result.ModifiedEntity != null)
				{
					return Created(new Uri($"{ControllerName}/{result.ModifiedEntity.Id}", UriKind.Relative),
						MapEntityToShortModel(result.ModifiedEntity));
				}

				return Content(HttpStatusCode.InternalServerError, new ErrorList(result.Errors));
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		[HttpPut]
		public virtual IHttpActionResult Put(TKey id, [FromBody] TModel value)
		{
			try
			{
				value.Id = id;
				RepositoryModifyResult<TEntity> result = Repository.Update(id, MapModelToEntity(value));
				if (result.ModifiedEntity != null)
				{
					return Ok(MapEntityToShortModel(result.ModifiedEntity));
				}

				return Content(HttpStatusCode.InternalServerError, new ErrorList(result.Errors));
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		protected abstract TModel MapEntityToModel(TEntity entity);

		protected abstract TEntity MapModelToEntity(TModel model);

		protected abstract TShortModel MapEntityToShortModel(TEntity entity);

		protected virtual IHttpActionResult HandleException(Exception e)
		{
			if (e is NotSupportedException)
			{
				return StatusCode(HttpStatusCode.MethodNotAllowed);
			}

			if (e is EntityNotFoundException)
			{
				return NotFound();
			}

			return InternalServerError(e);
		}
	}
}