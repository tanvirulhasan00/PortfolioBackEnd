using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            
            return Ok();
        }
    }
}