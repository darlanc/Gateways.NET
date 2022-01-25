using System;

namespace Gateways.NET.ViewModels
{
    /// <summary>
    /// Order Column
    /// </summary>
    public class OrderColumn
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ascendant
        /// </summary>
        public bool Ascendant { get; set; } = true;
    }
}
