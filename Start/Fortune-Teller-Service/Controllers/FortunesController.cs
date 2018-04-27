
using Fortune_Teller_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Internal;

namespace Fortune_Teller_Service.Controllers
{
    [Route("api/[controller]")]
    public class FortunesController : Controller
    {
        ILogger<FortunesController> _logger;
        private readonly IFortuneRepository _fortuneRepository;


        public FortunesController(ILogger<FortunesController> logger, IFortuneRepository fortuneRepository)
        {
            _logger = logger;
            _fortuneRepository = fortuneRepository;
        }


        // GET: api/fortunes/all
        [HttpGet("all")]
        public async Task<List<Fortune>> AllFortunesAsync()
        {
            _logger?.LogDebug("AllFortunesAsync");

            var fortunes = await _fortuneRepository.GetAllAsync();
            var result = new List<Fortune>();
            foreach (var fortuneEntity in fortunes)
            {
                result.Add(new Fortune { Id = fortuneEntity.Id, Text = fortuneEntity.Text });
            }

            return result;
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public async Task<Fortune> RandomFortuneAsync()
        {
            _logger?.LogDebug("RandomFortuneAsync");
            var randomFortune = await _fortuneRepository.RandomFortuneAsync();

            return new Fortune {Id = randomFortune.Id, Text = randomFortune.Text};
        }
    }
}
