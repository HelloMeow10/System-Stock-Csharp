using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Contracts
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Succeeded { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; }

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T> { Succeeded = true, Data = data };
        }

        public static ApiResponse<T> Fail(string error)
        {
            return new ApiResponse<T> { Succeeded = false, Errors = new List<string> { error } };
        }

        public static ApiResponse<T> Fail(List<string> errors)
        {
            return new ApiResponse<T> { Succeeded = false, Errors = errors };
        }
    }
}
