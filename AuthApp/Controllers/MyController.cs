using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly ILogger<MyController> logger;

        public MyController(ILogger<MyController> logger)
        {
            this.logger = logger;
        }

        [HttpGet, Authorize()]
        public IActionResult GetMyData(int flag)
        {
            try
            {
                logger.LogInformation("Into GetMydata method");
                var obj= new object();
                if (flag == 1)
                {
                    throw new System.Exception("Autp generated error");
                }
                else
                {
                    obj = new
                    {
                        Name = "Akash",
                        ID = 100
                    };
                }
                logger.LogInformation("Data fetched successfully");
                return Ok(obj);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
