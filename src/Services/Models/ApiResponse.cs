using System.Text.Json.Serialization;

namespace Services.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ApiError Error { get; }

        private ApiResponse(bool success, T data, ApiError error)
        {
            Success = success;
            Data = data;
            Error = error;
        }

        public static ApiResponse<T> CreateSuccess(T data)
        {
            return new ApiResponse<T>(true, data, null);
        }

        public static ApiResponse<T> CreateFailure(string errorCode, string errorMessage)
        {
            return new ApiResponse<T>(false, default, new ApiError(errorCode, errorMessage));
        }
    }
}
