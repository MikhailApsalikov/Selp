namespace Selp.Controller
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using Common.Exceptions;
	using Entities;
	using Interfaces;
	using Selp.Entities;

	public abstract class SelpController<TModel, TEntity, TKey> : ApiController, ISelpController<TModel, TEntity, TKey>
		where TModel : ISelpEntitiy<TKey> where TEntity : ISelpEntitiy<TKey>
	{
		protected SelpController(ISelpRepository<TEntity, TKey> repository)
		{
			Repository = repository;
		}

		protected ISelpRepository<TEntity, TKey> Repository { get; }

		public abstract string ControllerName { get; }

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

		public virtual IHttpActionResult Get([FromUri] BaseFilter query)
		{
			try
			{
				IQueryable<TEntity> entities = Repository.GetByFilter(query);
				var models = new List<TModel>();
				foreach (TEntity entity in entities)
				{
					models.Add(MapEntityToModel(entity));
				}

				return Ok(models);
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		public virtual IHttpActionResult Post([FromBody] TModel value)
		{
			try
			{
				RepositoryModifyResult<TEntity> result = Repository.Create(MapModelToEntity(value));
				if (result.ModifiedEntity != null)
				{
					return Created(new Uri($"{ControllerName}/{result.ModifiedEntity.Id}", UriKind.Relative),
						MapEntityToModel(result.ModifiedEntity));
				}

				return Content(HttpStatusCode.InternalServerError, new ErrorList(result.Errors));
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

		public virtual IHttpActionResult Put(TKey id, [FromBody] TModel value)
		{
			try
			{
				value.Id = id;
				RepositoryModifyResult<TEntity> result = Repository.Update(id, MapModelToEntity(value));
				if (result.ModifiedEntity != null)
				{
					return Ok(MapEntityToModel(result.ModifiedEntity));
				}

				return Content(HttpStatusCode.InternalServerError, new ErrorList(result.Errors));
			}
			catch (Exception e)
			{
				return HandleException(e);
			}
		}

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

		[NonAction]
		protected abstract TModel MapEntityToModel(TEntity entity);

		[NonAction]
		protected abstract TEntity MapModelToEntity(TModel entity);
	}
}