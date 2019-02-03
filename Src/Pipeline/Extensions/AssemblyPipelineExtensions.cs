using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tamarack.Pipeline.Extensions
{
    /// <summary>
    /// Class AssemblyPipelineExtensions.
    /// </summary>
    public static class AssemblyPipelineExtensions
    {
        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> AddAssembly<T, TOut>(this IPipeline<T, TOut> pipeline)
        {
            return AddAssembly(pipeline, Assembly.GetCallingAssembly(), t => true);
        }

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        public static IPublisher<T> AddAssembly<T>(this IPublisher<T> pipeline)
        {
            return AddAssembly(pipeline, Assembly.GetCallingAssembly(), t => true);
        }

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        public static IPublisher<T> AddAssembly<T>(this IPublisher<T> pipeline, Func<Type, bool> predicate)
        {
            return AddAssembly(pipeline, Assembly.GetCallingAssembly(), predicate);
        }

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> AddAssembly<T, TOut>(this IPipeline<T, TOut> pipeline, Func<Type, bool> predicate)
        {
            return AddAssembly(pipeline, Assembly.GetCallingAssembly(), predicate);
        }

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> AddAssembly<T, TOut>(this IPipeline<T, TOut> pipeline, Assembly assembly)
        {
            return AddAssembly(pipeline, assembly, t => true);
        }

        /// <summary>
        /// Adds the assembly of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <typeparam name="TOf">The type of the t of.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        public static void AddAssemblyOf<T, TOut, TOf>(this IPipeline<T, TOut> pipeline)
        {
            AddAssembly(pipeline, typeof(TOf).Assembly, t => true);
        }

        /// <summary>
        /// The pipeline types
        /// </summary>
        private static ConcurrentDictionary<Tuple<string, string, string>, List<Type>> _pipelineTypes = new ConcurrentDictionary<Tuple<string, string, string>, List<Type>>();

        /// <summary>
        /// The publisher types
        /// </summary>
        private static ConcurrentDictionary<Tuple<string, string>, List<Type>> _publisherTypes = new ConcurrentDictionary<Tuple<string, string>, List<Type>>();

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> AddAssembly<T, TOut>(
            this IPipeline<T, TOut> pipeline,
            Assembly assembly,
            Func<Type, bool> predicate)
        {
            var filterTypes = _pipelineTypes.GetOrAdd(Tuple.Create(assembly.FullName, typeof(T).FullName, typeof(TOut).FullName), q => assembly
                  .GetTypes()
                  .Where(t => typeof(IFilter<T, TOut>).IsAssignableFrom(t))
                  .Where(predicate)
                     .OrderBy(t => (((FilterOrderAttribute)Attribute.GetCustomAttribute(t, typeof(FilterOrderAttribute)))?.Order).GetValueOrDefault())
                  .ToList()); ;

            foreach (var filterType in filterTypes)
            {
                pipeline = pipeline.Add(filterType);
            }

            return pipeline;
        }

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IPublisher&lt;T&gt;.</returns>
        public static IPublisher<T> AddAssembly<T>(
    this IPublisher<T> pipeline,
    Assembly assembly,
    Func<Type, bool> predicate)
        {
            var filterTypes = _publisherTypes.GetOrAdd(Tuple.Create(assembly.FullName, typeof(T).FullName), q => assembly
                 .GetTypes()
                 .Where(t => typeof(IActionFilter<T>).IsAssignableFrom(t))
                 .Where(predicate)
                 .OrderBy(t => (((FilterOrderAttribute)Attribute.GetCustomAttribute(t, typeof(FilterOrderAttribute)))?.Order).GetValueOrDefault())
                 .ToList());

            foreach (var filterType in filterTypes)
            {
                pipeline = pipeline.Add(filterType);
            }

            return pipeline;
        }

        /// <summary>
        /// Adds the namespace.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="filterNamespace">The filter namespace.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> AddNamespace<T, TOut>(this IPipeline<T, TOut> pipeline, string filterNamespace)
        {
            return AddAssembly(pipeline, Assembly.GetCallingAssembly(), t => t.Namespace == filterNamespace);
        }

        /// <summary>
        /// Adds the namespace.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="filterNamespace">The filter namespace.</param>
        /// <returns>IPipeline&lt;T, TOut&gt;.</returns>
        public static IPipeline<T, TOut> AddNamespace<T, TOut>(
            this IPipeline<T, TOut> pipeline,
            Assembly assembly,
            string filterNamespace)
        {
            return AddAssembly(pipeline, assembly, t => t.Namespace == filterNamespace);
        }
    }
}