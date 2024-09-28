using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Portfolio.Models.PortfolioModels
{
    public class Person
    {
        public int PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Designation { get; set; }
    }
}