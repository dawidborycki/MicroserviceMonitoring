using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MockController : ControllerBase
    {        
        private readonly ILogger<MockController> _logger;
        private static int index = 0;

        public MockController(ILogger<MockController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            switch (index++ % 4)
            {
                default:
                case 0:
                    return Ok();

                case 1:
                    return BadRequest();

                case 2:
                    return NotFound();

                case 3:
                    _logger.LogError("Exception in Get");
                    throw new Exception("Some random error was generated");                
            }           
        }
    }
}
