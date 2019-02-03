using System;
using System.Collections.Generic;
using System.Linq;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Class FuncPipeline.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IPipeline{T}" />
    public class FuncPipeline<T> : IPipeline<T>
    {
        /// <summary>
        /// The service provider
        /// </summary>
        private readonly ITamarackServiceProvider serviceProvider;
        /// <summary>
        /// The filters
        /// </summary>
        private readonly IList<IFuncFilter<T>> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncPipeline{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public FuncPipeline(ITamarackServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            filters = new List<IFuncFilter<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncPipeline{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="filters">The filters.</param>
        protected FuncPipeline(ITamarackServiceProvider serviceProvider, IEnumerable<IFuncFilter<T>> filters)
        {
            this.serviceProvider = serviceProvider;
            this.filters = filters.ToList();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return filters.Count; }
        }

        /// <summary>
        /// Adds the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IPipeline&lt;T&gt;.</returns>
        public IPipeline<T> Add(IFuncFilter<T> filter)
        {
            var newFilters = this.filters.ToList();
            newFilters.Add(filter);
            return new FuncPipeline<T>(this.serviceProvider, newFilters);
        }

        /// <summary>
        /// Adds the specified filter type.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>IPipeline&lt;T&gt;.</returns>
        public IPipeline<T> Add(Type filterType)
        {
            return Add((IFuncFilter<T>)serviceProvider.GetService(filterType));
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="TFilter">The type of the t filter.</typeparam>
        /// <returns>IPipeline&lt;T&gt;.</returns>
        public IPipeline<T> Add<TFilter>() where TFilter : IFuncFilter<T>
        {
            return Add(typeof(TFilter));
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>T.</returns>
        public T Execute()
        {
            var tail = new Func<T>(() => { throw new EndOfChainException(); });
            return ((IFuncFilter<T>)this).Execute(tail);
        }

        /// <summary>
        /// Executes the specified execute next.
        /// </summary>
        /// <param name="executeNext">The execute next.</param>
        /// <returns>T.</returns>
        T IFuncFilter<T>.Execute(Func<T> executeNext)
        {
            var current = 0;
            Func<Func<T>> GetNext = null;
            GetNext = () => current < filters.Count
                ? () => filters[current++].Execute(GetNext())
                : executeNext;

            return GetNext().Invoke();
        }
    }
}