using System;

namespace Gateways.NET.Contracts
{
    /// <summary>
    /// Base entity
    /// </summary>
    /// <typeparam name="TKey">Generic primary key type</typeparam>
    public class BaseEntity<TKey>
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public TKey Id { get; set; }
    }
}
