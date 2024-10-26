using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Portfolio.Models.ApiResponseModels.ApiResponse
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string>? Message { get; set; }
        public object? Result { get; set; }
        public List<T>? ResultsList { get; set; }
    }
}