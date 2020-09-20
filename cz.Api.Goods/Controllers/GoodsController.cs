using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cz.Api.Goods.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GoodsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Goods1", "Goods2", "Goods3", "Goods4", "Goods5", "Goods6", "Goods7", "Goods8", "Goods9", "Goods10"
        };

        private readonly ILogger<GoodsController> _logger;

        public GoodsController(ILogger<GoodsController> logger)
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
    }
}
