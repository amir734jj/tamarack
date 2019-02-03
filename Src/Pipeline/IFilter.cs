using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Interface IFilter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut">The type of the t out.</typeparam>
    public interface IFilter<T, TOut>
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="executeNext">The execute next.</param>
        /// <returns>TOut.</returns>
        TOut Execute(T context, Func<T, TOut> executeNext);
    }
}