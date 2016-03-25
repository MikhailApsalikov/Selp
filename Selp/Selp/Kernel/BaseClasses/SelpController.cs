namespace Selp.Kernel.BaseClasses
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Net;
	using System.Web.Http;
	using Classes;
	using Common.Exceptions;
	using Interfaces;


	// think about hidden (deleted)
	public abstract class SelpController<T, TKey, TModel> : ApiController
	   where T : class, ISelpEntity<TKey>
	   where TModel : ISelpEntity<TKey>
	{
		protected readonly ISelpRepository<T, TKey> repository;

		protected abstract string ControllerName { get; }

		protected SelpController(ISelpRepository<T, TKey> repository)
		{
			this.repository = repository;
		}

		public virtual IHttpActionResult Get([FromUri]BaseFilterQuery query)
		{
			try
			{
				var source = FilterByOrganizationalUnit(FilterHidden(query != null ? ApplyFilter(_repository, query) : _repository.GetAll()), query);
				if (query != null && query.IsFilterSpecified())
				{
					query.CheckAndFixFilterParams();

					return Ok(Pagination.Get<T, TModel>(
								source,
								query.Page.Value,
								query.PageSize.Value,
								Mapper.Map<TModel>,
								query.SortField,
								query.SortDirection));
				}
				return Ok(source.Select(Mapper.Map<TModel>));
			}
			catch (Exception exc)
			{
				return HandleError(exc);
			}
		}

		// GET api/<controller>/5
		public virtual IHttpActionResult Get(TKey id)
		{
			try
			{
				var entity = _repository.GetById(id);
				CheckHiddenItemAccess(entity);

				return Ok(Mapper.Map<TModel>(entity));
			}
			catch (EntityNotFoundException)
			{
				return NotFound();
			}
			catch (Exception exc)
			{
				_logger.Warn(exc);
				return InternalServerError();
			}
		}

		// POST api/<controller>
		public virtual IHttpActionResult Post([FromBody]TModel value)
		{
			try
			{
				value = Mapper.Map<TModel>(_repository.Add(UpdateOrganizationalUnit(Mapper.Map<T>(value))));

				return Created(
					new Uri(String.Format("{0}/{1}", ControllerName, value.Id), UriKind.Relative),
					value);
			}
			catch (Exception exc)
			{
				return HandleError(exc);
			}
		}

		// PUT api/<controller>/5
		public virtual IHttpActionResult Put(TKey id, [FromBody]TModel value)
		{
			try
			{
				VerifyOrganizationalUnitBoundry(id);

				value.Id = id;
				return Ok(Mapper.Map<TModel>(_repository.Update(Mapper.Map<T>(value))));
			}
			catch (Exception exc)
			{
				return HandleError(exc, true);
			}
		}

		// DELETE api/<controller>/5
		public virtual IHttpActionResult Delete(TKey id)
		{
			try
			{
				_repository.RemoveById(id);
				return Ok();
			}
			catch (Exception exc)
			{
				return HandleError(exc);
			}
		}



		protected abstract IQueryable<T> ApplyFilter(IRepository<T, TKey> repository, BaseFilterQuery query);

		protected virtual IQueryable<T> FilterHidden(IQueryable<T> source)
		{
			return !User.IsSupervisor() && !User.IsAdministrator() && !User.IsSuperAdministrator() && IsNotHiddenExpression != null
						? source.Where(IsNotHiddenExpression)
						: source;
		}

		protected virtual IQueryable<T> FilterByOrganizationalUnit(IQueryable<T> source, BaseFilterQuery query)
		{
			if (!IsEntityWithOrganizationalUnit)
				return source;

			var organizationalUnit = OrganizationalUnitLogicHelper.TryGetOrganizationalUnitId(query, User.GetUser());
			return organizationalUnit.HasValue
				? source.Where(CreateOrganizationalUnitFilterExpression(organizationalUnit.Value))
				: source;
		}

		protected virtual void CheckHiddenItemAccess(T entity)
		{
			if (!PermissionsChecker.CheckEntityHiddenAccess(User.GetUser(), entity))
				throw new EntityNotFoundException();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_repository.Dispose();
		}

		protected T UpdateOrganizationalUnit(T entity)
		{
			var entityWithOU = entity as IEntityWithOrganizationalUnit;
			if (entityWithOU != null)
			{
				OrganizationalUnitLogicHelper.UpdateEntityOrganizationalUnit(entityWithOU, User.GetUser());
			}
			return entity;
		}

		protected void VerifyOrganizationalUnitBoundry(TKey key)
		{
			if (!IsEntityWithOrganizationalUnit)
				return;

			var user = User.GetUser();
			var item = _repository.GetByIdAsNoTracking(key);
			if (!user.IsSuperAdministrator() && ((IEntityWithOrganizationalUnit)item).OrganizationalUnitId != user.OrganizationalUnitId)
			{
				throw new PermissionsException();
			}
		}

		protected IHttpActionResult HandleError(Exception exc, bool isUpdate = false)
		{
			var result = ErrorHandler.Handle(exc, isUpdate);
			switch (result.Item1)
			{
				case HttpStatusCode.NotFound:
					return NotFound();
				case HttpStatusCode.BadRequest:
					_logger.Warn(exc);
					if (!String.IsNullOrEmpty(result.Item2))
					{
						return BadRequest(result.Item2);
					}
					return BadRequest();
				default:
					_logger.Warn(exc);
					return InternalServerError();
			}
		}

		private static Expression<Func<T, bool>> CreateOrganizationalUnitFilterExpression(int organizationalUnit)
		{
			var type = typeof(T);
			var parameter = Expression.Parameter(type);
			return Expression.Lambda<Func<T, bool>>(
						Expression.Equal(Expression.Property(parameter, "OrganizationalUnitId"),
										 Expression.Constant(organizationalUnit, typeof(int))),
						parameter);
		}
	}