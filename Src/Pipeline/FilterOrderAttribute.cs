using System;

namespace Tamarack.Pipeline
{
    /// <summary>
    /// Class FilterOrderAttribute.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class FilterOrderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }
    }
}
