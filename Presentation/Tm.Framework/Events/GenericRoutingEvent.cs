using Microsoft.AspNetCore.Routing;

namespace Tm.Framework.Events
{
    /// <summary>
    /// Represents an event that occurs when a generic route is processed and no default handlers are found
    /// </summary>
    public class GenericRoutingEvent
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="values">Route values</param>
        /// <param name="urlRecord">Found URL record</param>
        public GenericRoutingEvent(RouteValueDictionary values)
        {
            RouteValues = values;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets route values
        /// </summary>
        public RouteValueDictionary RouteValues { get; private set; }

        #endregion
    }
}