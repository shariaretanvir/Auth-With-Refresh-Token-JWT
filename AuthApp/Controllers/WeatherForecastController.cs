using AuthApp.Filters;
using AuthApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache memoryCache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            this.memoryCache = memoryCache;
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

        [HttpGet, Route("GetAll/{name}")]
        //[ServiceFilter(typeof(CacheFilterAttribute))]
        public async Task<IActionResult> GetAll(string name)
        {
            var cacheKey = "weathers-"+name;
            if (!memoryCache.TryGetValue(cacheKey, out List<WeatherForecast> cacheWeathers))
            {
                cacheWeathers = new List<WeatherForecast>
                {
                    new WeatherForecast
                    {
                       Summary="Hot"
                    },
                    new WeatherForecast
                    {
                        Summary="Medium"
                    },
                    new WeatherForecast
                    {
                        Summary="Cool"
                    },
                    new WeatherForecast
                    {
                        Summary="Ice"
                    }
                };
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                };
                memoryCache.Set(cacheKey, cacheWeathers, cacheOptions);
                await Task.Delay(3000);
            }            
            return Ok(cacheWeathers);
        }
    }
}
