using System;
using System.Collections.Generic;
using System.Linq;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Class ActionPipeline.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Nop.Core.Infrastructure.Tamarack.Pipeline.IPublisher{T}" />
    public class ActionPipeline<T> : IPublisher<T>
    {
        /// <summary>
        /// The service provider
        /// </summary>
        private readonly ITamarackServiceProvider serviceProvider;
        /// <summary>
        /// The filters
        /// </summary>
        private readonly IList<IActionFilter<T>> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionPipeline{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ActionPipeline(ITamarackServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            filters = new List<IActionFilter<T>>();
        }

        public ActionPipeline()
            :this(new ActivatorServiceProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionPipeline{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="filters">The filters.</param>
        protected ActionPipeline(ITamarackServiceProvider serviceProvider, IEnumerable<IActionFilter<T>> filters)
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
        /// <returns>IPublisher&lt;T&gt;.</returns>
        public IPublisher<T> Add(IActionFilter<T> filter)
        {
            var newFilters = this.filters.ToList();
            newFilters.Add(filter);
            return new ActionPipeline<T>(this.serviceProvider, newFilters);
        }

        /// <summary>
        /// Adds the specified filter type.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        public IPublisher<T> Add(Type filterType)
        {
            return Add((IActionFilter<T>)serviceProvider.GetService(filterType));
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="TFilter">The type of the t filter.</typeparam>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        public IPublisher<T> Add<TFilter>() where TFilter : IActionFilter<T>
        {
            return Add(typeof(TFilter));
        }

        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Execute(T input)
        {
            var tail = new Action<T>((t) => { });
            ((IActionFilter<T>)this).Execute(input, tail);
        }

        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="executeNext">The execute next.</param>
        void IActionFilter<T>.Execute(T input, Action<T> executeNext)
        {
            var current = 0;
            Func<Action<T>> GetNext = null;
            GetNext = () => current < filters.Count
                ? x => filters[current++].Execute(x, GetNext())
                : executeNext;

            GetNext().Invoke(input);
        }
    }
}
