using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.RequestModels.CustomerMessageReqDto
{
    public class CustomerMessageCreateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ServiceName { get; set; }
        public string? Message { get; set; }
    }
}