using Gateways.NET.Contracts;


namespace Gateways.NET.DTOs
{
    /// <summary>
    /// Pagination View Model
    /// </summary>
    public class PaginationViewModel : IPaginationModel
    {
        /// <summary>
        /// Page
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
