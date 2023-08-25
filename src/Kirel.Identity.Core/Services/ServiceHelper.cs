using System.Linq.Expressions;
using Kirel.DTO;
using Kirel.Shared;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Helper static class with default services functions.
/// </summary>
public static class ServiceHelper
{
    /// <summary>
    /// Generate ordering method
    /// </summary>
    /// <param name="orderBy"> Order by field name</param>
    /// <param name="orderDirection"> Order direction </param>
    /// <typeparam name="TEntity"> Entity type </typeparam>
    /// <returns>Ordering function</returns>
    public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? GenerateOrderingMethod<TEntity>(string? orderBy, SortDirection orderDirection)
    {
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderingMethod = null;
        if (string.IsNullOrEmpty(orderBy)) return null;
        Expression<Func<TEntity, object>>? orderExpression = PredicateBuilder.ToLambda<TEntity>(orderBy);
        if (orderExpression == null) return orderingMethod;
        switch (orderDirection)
        {
            case SortDirection.Asc:
                orderingMethod = o => o.OrderBy(orderExpression);
                break;
            case SortDirection.Desc:
                orderingMethod = o => o.OrderByDescending(orderExpression);
                break;
        }
        return orderingMethod;
    }
}