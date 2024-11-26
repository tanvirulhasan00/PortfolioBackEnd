using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Portfolio.Models.ResponseModels.ApiResponseModels
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public List<T>? Data { get; set; }
        public T? SingleData { get; set; }
        public object? Error { get; set; }
    }
}