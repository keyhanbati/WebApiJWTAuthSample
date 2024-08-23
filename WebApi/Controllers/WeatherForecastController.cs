using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    //[ApiController]
    [Authorize]
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

        [Route("SimpleGet")]
        [HttpGet]
        public IEnumerable<WeatherForecast> SimpleGet()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("Get")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get(WeatherForecast model)
        {
            var retVal = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();
            retVal.Add(model);
            return retVal.ToArray();
        }

        [Route("Post")]
        [HttpPost]
        public IEnumerable<WeatherForecast> Post(WeatherForecast model)
        {
            var retVal = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();
            retVal.Add(model);
            return retVal.ToArray();
        }
        [AllowAnonymous]
        [Route("PostFromBody")]
        [HttpPost]
        public IEnumerable<WeatherForecast> PostFromBody([FromBody] WeatherForecast model)
        {
            var retVal = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();
            retVal.Add(model);
            return retVal.ToArray();
        }
    }
}