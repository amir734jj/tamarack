using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Class PipelineExtensions.
    /// </summary>
    public static class PipelineExtensions
    {
        /// <summary>
        /// Convenience method to add a short-circuiting filter to the end of the chain
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="func">The function.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> Finally<T, TOut>(this IPipeline<T, TOut> pipeline, Func<T, TOut> func)
        {
            return pipeline.Add(new ShortCircuit<T, TOut>(func));
        }

        /// <summary>
        /// Finallies the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="action">The action.</param>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        public static IPublisher<T> Finally<T>(this IPublisher<T> pipeline, Action<T> action)
        {
            return pipeline.Add(new ActionShortCircuit<T>(action));
        }

        /// <summary>
        /// Finallies the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="action">The action.</param>
        /// <returns>IPipeline&lt;T&gt;.</returns>
        public static IPipeline<T> Finally<T>(this IPipeline<T> pipeline, Func<T> action)
        {
            return pipeline.Add(new FuncShortCircuit<T>(action));
        }

        /// <summary>
        /// Class ShortCircuit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IFilter{T, TOut}" />
        private class ShortCircuit<T, TOut> : IFilter<T, TOut>
        {
            /// <summary>
            /// The function
            /// </summary>
            private readonly Func<T, TOut> func;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShortCircuit{T, TOut}"/> class.
            /// </summary>
            /// <param name="func">The function.</param>
            public ShortCircuit(Func<T, TOut> func)
            {
                this.func = func;
            }

            /// <summary>
            /// Executes the specified context.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="executeNext">The execute next.</param>
            /// <returns>TOut.</returns>
            public TOut Execute(T context, Func<T, TOut> executeNext)
            {
                return func.Invoke(context);
            }
        }

        /// <summary>
        /// Class ActionShortCircuit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IActionFilter{T}" />
        private class ActionShortCircuit<T> : IActionFilter<T>
        {
            /// <summary>
            /// The function
            /// </summary>
            private readonly Action<T> func;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionShortCircuit{T}"/> class.
            /// </summary>
            /// <param name="func">The function.</param>
            public ActionShortCircuit(Action<T> func)
            {
                this.func = func;
            }

            /// <summary>
            /// Executes the specified context.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="executeNext">The execute next.</param>
            public void Execute(T context, Action<T> executeNext)
            {
                func.Invoke(context);
            }
        }

        /// <summary>
        /// Class FuncShortCircuit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IFuncFilter{T}" />
        private class FuncShortCircuit<T> : IFuncFilter<T>
        {
            /// <summary>
            /// The function
            /// </summary>
            private readonly Func<T> func;

            /// <summary>
            /// Initializes a new instance of the <see cref="FuncShortCircuit{T}"/> class.
            /// </summary>
            /// <param name="func">The function.</param>
            public FuncShortCircuit(Func<T> func)
            {
                this.func = func;
            }

            /// <summary>
            /// Executes the specified execute next.
            /// </summary>
            /// <param name="executeNext">The execute next.</param>
            /// <returns>T.</returns>
            public T Execute(Func<T> executeNext)
            {
                return func.Invoke();
            }
        }
    }
}