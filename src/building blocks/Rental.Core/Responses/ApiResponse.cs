using Rental.Core.Interfaces;

namespace Rental.Core.Responses
{
    public class ApiResponse : IApiResponse, IResponse
    {
        public bool Success { get; }
        public IEnumerable<string> Messages { get; }
        public object Data { get; }

        public ApiResponse(bool success, IEnumerable<string> messages, object data)
        {
            Success = success;
            Messages = messages;
            Data = data;
        }
    }

}
