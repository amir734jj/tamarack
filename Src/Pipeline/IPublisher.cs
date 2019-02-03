using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Interface IPublisher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IActionFilter{T}" />
    public interface IPublisher<T> : IActionFilter<T>
    {
        /// <summary>
        /// Adds the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        IPublisher<T> Add(IActionFilter<T> filter);
        /// <summary>
        /// Adds the specified filter type.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        IPublisher<T> Add(Type filterType);
        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="TFilter">The type of the t filter.</typeparam>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        IPublisher<T> Add<TFilter>() where TFilter : IActionFilter<T>;
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }
        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        void Execute(T input);
    }
}
