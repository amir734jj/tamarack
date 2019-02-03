using System;
using System.Collections.Generic;
using System.Linq;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Class Pipeline.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut">The type of the t out.</typeparam>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IPipeline{T, TOut}" />
    public class Pipeline<T, TOut> : IPipeline<T, TOut>
    {
        /// <summary>
        /// The service provider
        /// </summary>
        private readonly ITamarackServiceProvider serviceProvider;
        /// <summary>
        /// The filters
        /// </summary>
        private readonly IList<IFilter<T, TOut>> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline{T, TOut}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public Pipeline(ITamarackServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            filters = new List<IFilter<T, TOut>>();
        }

        public Pipeline()
            :this(new ActivatorServiceProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline{T, TOut}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="filters">The filters.</param>
        protected Pipeline(ITamarackServiceProvider serviceProvider, IEnumerable<IFilter<T, TOut>> filters)
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
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public IPipeline<T, TOut> Add(IFilter<T, TOut> filter)
        {
            var newFilters = filters.ToList();
            newFilters.Add(filter);
            return new Pipeline<T, TOut>(this.serviceProvider, newFilters);
        }

        /// <summary>
        /// Adds the specified filter type.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public IPipeline<T, TOut> Add(Type filterType)
        {
            return Add((IFilter<T, TOut>)serviceProvider.GetService(filterType));
        }


        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="TFilter">The type of the t filter.</typeparam>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public IPipeline<T, TOut> Add<TFilter>() where TFilter : IFilter<T, TOut>
        {
            return Add(typeof(TFilter));
        }

        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>TOut.</returns>
        public TOut Execute(T input)
        {
            var tail = new Func<T, TOut>(x => { throw new EndOfChainException(); });

            return ((IFilter<T, TOut>)this).Execute(input, tail);
        }

        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="executeNext">The execute next.</param>
        /// <returns>TOut.</returns>
        TOut IFilter<T, TOut>.Execute(T input, Func<T, TOut> executeNext)
        {
            var current = 0;
            Func<Func<T, TOut>> GetNext = null;
            GetNext = () => current < filters.Count
                ? x => filters[current++].Execute(x, GetNext())
                : executeNext;

            return GetNext().Invoke(input);
        }
    }
}