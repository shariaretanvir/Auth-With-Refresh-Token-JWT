using AuthApp.Filters;
using AuthApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Demo")]
        public IActionResult Demo()
        {
            return Ok("Done");
        }

        [HttpGet]
        //[ServiceFilter(typeof(CommonRequestFilter))]
        //[ServiceFilter(typeof(CustomResultFilterAttribute))]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                //Date = DateTime.Now.AddDays(index),
                //TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("save")]
        [HttpPost]
        [ServiceFilter(typeof(LoggingFilterAttribute))]
        //[ServiceFilter(typeof(ModelValidationFilterAttribute))]
        //[ServiceFilter(typeof(CustomResultFilterAttribute))]
        public IActionResult save([FromBody] LoginModel loginModel)
        {
            return Ok("Save SUccessfully");
        }

        [Route("save1")]
        [HttpPost]
        //[ServiceFilter(typeof(LoggingFilterAttribute))]
        public IActionResult save1([FromBody] LoginModel loginModel)
        {
            return Ok("Save SUccessfully 1");
        }
    }
}
