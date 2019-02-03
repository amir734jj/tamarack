using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Interface IPipeline
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IFuncFilter{T}" />
    public interface IPipeline<T> : IFuncFilter<T>
    {
        /// <summary>
        /// Adds the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IPipeline&lt;T&gt;.</returns>
        IPipeline<T> Add(IFuncFilter<T> filter);
        /// <summary>
        /// Adds the specified filter type.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>IPipeline&lt;T&gt;.</returns>
        IPipeline<T> Add(Type filterType);
        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="TFilter">The type of the t filter.</typeparam>
        /// <returns>IPipeline&lt;T&gt;.</returns>
        IPipeline<T> Add<TFilter>() where TFilter : IFuncFilter<T>;
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>T.</returns>
        T Execute();
    }
}
