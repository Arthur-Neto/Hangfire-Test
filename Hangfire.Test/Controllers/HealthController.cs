using Microsoft.AspNetCore.Mvc;
using System;

namespace Hangfire.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private IBackgroundJobClient _backgroundJobs { get; set; }

        public HealthController(IBackgroundJobClient backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(true);
        }

        [HttpGet]
        public IActionResult Post()
        {
            return Ok(_backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!")));
        }
    }
}
