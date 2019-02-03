using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Interface IActionFilter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IActionFilter<T>
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="executeNext">The execute next.</param>
        void Execute(T context, Action<T> executeNext);
    }
}
