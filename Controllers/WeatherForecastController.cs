using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
            return Enumerable.Range(1, 6).Select(index => new WeatherForecast
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

            _logger.LogInformation("Status OK..");
            _logger.LogInformation("return..");

            return Ok();
        }

        [HttpGet]
        [Route("/getDT")]
        public IEnumerable<MyModel> GetDtJson()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonString = System.IO.File.ReadAllText(Environment.CurrentDirectory.ToString() + "\\my-model.json");
            var jsonModel = JsonSerializer.Deserialize<MyModel>(jsonString, options);

            jsonModel.MyString = "New test";
            jsonModel.MyDateTime1 = DateTime.UtcNow;
            jsonModel.MyInt = 9999999;
            jsonModel.MyAnotherModel.MyStringList = new List<string> { "coconut", "grain" };

            var modelJson = JsonSerializer.Serialize(jsonModel, options);
            System.IO.File.WriteAllText(Environment.CurrentDirectory.ToString() + "\\my-model.json", modelJson);

            return new List<MyModel>
            {
                jsonModel,jsonModel.MyAnotherModel
            };
        }

    }
}
