using System.Linq.Expressions;

namespace Cervejaria.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Aplica o predicado a query somente quando a condicao for verdadeira,
        /// mantendo a composicao no IQueryable para que apenas os filtros
        /// efetivamente usados cheguem ao SQL.
        /// </summary>
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condicao,
            Expression<Func<T, bool>> predicado)
            => condicao ? source.Where(predicado) : source;
    }
}
