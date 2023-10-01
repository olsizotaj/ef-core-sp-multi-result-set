using EFCore.ReadMultiResultSetFromSP.DBLayer;
using EFCore.ReadMultiResultSetFromSP.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.ReadMultiResultSetFromSP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IRepository Repo { get; set; }

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepository repo)
        {
            Repo = repo;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        
        [HttpGet(), Route("GetMultiResultSetFromSP")]
        public async Task<MainResult> GetMultiResultSetFromSp()
        {
            try
            {
                var spResultSets = await Repo.ExecuteMultiResultSetSpAsync<ResultSet1, ResultSet2, int, ResultSet4>(
                    "usp_getMultiResultSetFromSp", new Dictionary<string, object> { { "id", 1 }, { "name", "admin" } });

                var res = new MainResult
                {
                    ResultSet1 = spResultSets.Item1,
                    ResultSet2 = spResultSets.Item2,
                    ResultSet3 = spResultSets.Item3,
                    ResultSet4 = spResultSets.Item4
                };

                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        [HttpGet(), Route("GetMultiResultSetFromSP1")]
        public async Task<object> GetMultiResultSetFromSp1()
        {
            try
            {
                var spResultSets = await Repo.ExecuteMultiResultSetSpAsync<ResultSet1, ResultSet2, object, ResultSet4>(
                    "usp_getMultiResultSetFromSp", new Dictionary<string, object> { { "id", 1 }, { "name", "admin" } });

                var res = new
                {
                    ResultSet1 = spResultSets.Item1,
                    ResultSet2 = spResultSets.Item2,
                    ResultSet4 = spResultSets.Item4
                };

                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}