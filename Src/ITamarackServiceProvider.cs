using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tamarack
{
    /// <summary>
    /// Interface ITamarackServiceProvider
    /// </summary>
    public interface ITamarackServiceProvider
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>System.Object.</returns>
        object GetService(Type serviceType);
    }
}
