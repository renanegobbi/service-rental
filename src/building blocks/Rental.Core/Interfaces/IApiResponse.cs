namespace Rental.Core.Interfaces
{
    public interface IApiResponse
    {
        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Returned messages
        /// </summary>
        IEnumerable<string> Messages { get; }

        /// <summary>
        /// Returned object
        /// </summary>
        object Data { get; }
    }
}
