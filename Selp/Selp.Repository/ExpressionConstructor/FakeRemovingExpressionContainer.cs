using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Selp.Repository.ExpressionConstructor
{
	internal class FakeRemovingExpressionContainer<TEntity>
	{
		private readonly Dictionary<string, Action<TEntity>> fakeDeleteCompiled = new Dictionary<string, Action<TEntity>>();

		private readonly Dictionary<string, Func<TEntity, bool>> isRemovedCompiled =
			new Dictionary<string, Func<TEntity, bool>>();

		private readonly Dictionary<string, Expression<Func<TEntity, bool>>> isRemovedExpressions =
			new Dictionary<string, Expression<Func<TEntity, bool>>>();


		public Expression<Func<TEntity, bool>> GetIsRemovedExpression(string fakeRemovingPropertyName)
		{
			lock (isRemovedExpressions)
			{
				if (!isRemovedExpressions.ContainsKey(fakeRemovingPropertyName))
				{
					var entityParamenter = Expression.Parameter(typeof (TEntity));
					isRemovedExpressions[fakeRemovingPropertyName] =
						Expression.Lambda<Func<TEntity, bool>>(
							Expression.Not(Expression.Property(entityParamenter, fakeRemovingPropertyName)), entityParamenter);
				}
			}

			return isRemovedExpressions[fakeRemovingPropertyName];
		}

		public Func<TEntity, bool> GetIsRemovedCompiledFunction(string fakeRemovingPropertyName)
		{
			lock (isRemovedCompiled)
			{
				if (!isRemovedCompiled.ContainsKey(fakeRemovingPropertyName))
				{
					isRemovedCompiled[fakeRemovingPropertyName] = GetIsRemovedExpression(fakeRemovingPropertyName).Compile();
				}
			}

			return isRemovedCompiled[fakeRemovingPropertyName];
		}

		public Action<TEntity> GetFakeDeleteCompiledFunction(string fakeRemovingPropertyName)
		{
			lock (fakeDeleteCompiled)
			{
				if (!fakeDeleteCompiled.ContainsKey(fakeRemovingPropertyName))
				{
					var entityParamenter = Expression.Parameter(typeof(TEntity));
					Expression<Action<TEntity>> lambda = Expression.Lambda<Action<TEntity>>(Expression.Assign(Expression.Property(entityParamenter, fakeRemovingPropertyName), Expression.Constant(true)), entityParamenter);
					fakeDeleteCompiled[fakeRemovingPropertyName] = lambda.Compile();
				}
			}

			return fakeDeleteCompiled[fakeRemovingPropertyName];
		}

		#region Singleton

		public static FakeRemovingExpressionContainer<TEntity> Instance { get; }

		static FakeRemovingExpressionContainer()
		{
			Instance = new FakeRemovingExpressionContainer<TEntity>();
		}

		private FakeRemovingExpressionContainer()
		{
		}

		#endregion
	}
}