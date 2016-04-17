namespace Selp.Controller
{
	using System;
	using System.Net;
	using System.Web.Http;
	using Common.Entities;
	using Common.Exceptions;
	using Entities;
	using Interfaces;

	public abstract class SelpController<TModel, TEntity, TKey> : ApiController, ISelpController<TModel, TKey>
		where TModel : ISelpEntity<TKey> where TEntity : ISelpEntity<TKey>
	{
		protected SelpController(ISelpRepository<TModel, TEntity, TKey> repository)
		{
			Repository = repository;
		}

		protected ISelpRepository<TModel, TEntity, TKey> Repository { get; }

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
				return Ok(Repository.GetById(id));
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
				return Ok(Repository.GetByFilter(query));
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
				RepositoryModifyResult<TModel> result = Repository.Create(value);
				if (result.ModifiedEntity != null)
				{
					return Created(new Uri($"{ControllerName}/{result.ModifiedEntity.Id}", UriKind.Relative),
						result.ModifiedEntity);
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
				RepositoryModifyResult<TModel> result = Repository.Update(id, value);
				if (result.ModifiedEntity != null)
				{
					return Ok(result.ModifiedEntity);
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
	}
}