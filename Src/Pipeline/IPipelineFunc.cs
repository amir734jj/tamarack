using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Interface IPipeline
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut">The type of the t out.</typeparam>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IFilter{T, TOut}" />
    public interface IPipeline<T, TOut> : IFilter<T, TOut>
    {
        /// <summary>
        /// Adds the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        IPipeline<T, TOut> Add(IFilter<T, TOut> filter);
        /// <summary>
        /// Adds the specified filter type.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        IPipeline<T, TOut> Add(Type filterType);
        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="TFilter">The type of the t filter.</typeparam>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        IPipeline<T, TOut> Add<TFilter>() where TFilter : IFilter<T, TOut>;
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }
        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>TOut.</returns>
        TOut Execute(T input);
    }
}
