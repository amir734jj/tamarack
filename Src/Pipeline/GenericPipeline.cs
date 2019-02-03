using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Class GenericPipeline.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IFilter{T, K}" />
    public class GenericPipeline<T, K> : IFilter<T, K>
    {
        /// <summary>
        /// The callback
        /// </summary>
        private Func<T, Func<T, K>, K> callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericPipeline{T, K}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public GenericPipeline(Func<T, Func<T, K>, K> callback)
        {
            // TODO: Complete member initialization
            this.callback = callback;
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="executeNext">The execute next.</param>
        /// <returns>TOut.</returns>
        public K Execute(T context, Func<T, K> executeNext)
        {
            return callback(context, executeNext);
        }
    }

    /// <summary>
    /// Class GenericPipeline.
    /// </summary>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IFilter{T, K}" />
    public static class GenericPipeline
    {
        /// <summary>
        /// Creates the specified callback.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="callback">The callback.</param>
        /// <returns>GenericPipeline&lt;T, K&gt;.</returns>
        public static GenericPipeline<T, K> Create<T, K>(Func<T, Func<T, K>, K> callback)
        {
            return new GenericPipeline<T, K>(callback);
        }
    }
}
