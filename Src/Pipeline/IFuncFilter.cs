using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Interface IFuncFilter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFuncFilter<T>
    {
        /// <summary>
        /// Executes the specified execute next.
        /// </summary>
        /// <param name="executeNext">The execute next.</param>
        /// <returns>T.</returns>
        T Execute(Func<T> executeNext);
    }
}
