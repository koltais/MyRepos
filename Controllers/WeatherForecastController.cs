using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
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
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("api/[controller]/getOther")]
        public IEnumerable<WeatherForecast> GetOther()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 45),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToList();
        }

        [HttpPost]
        [Route("api/[controller]/setParam")]
        public IActionResult SetParam(string sParam)
        {
            string s = Request.QueryString.ToString();
            var hd = Enumerable.ToDictionary(Request.Headers, x => x.Key, x => x.Value);

            _logger.LogInformation("SetParam worker running at: {time}", DateTimeOffset.UtcNow);
            _logger.LogInformation("SetParam QuersString: {querstring}", s);
            _logger.LogInformation("SetParam POSTed param: {param}", sParam);

            foreach (var i in hd)
            {
                _logger.LogInformation("Header ({key}): {head}", i.Key, i.Value);
            }

            return Ok();
        }
    }
}
