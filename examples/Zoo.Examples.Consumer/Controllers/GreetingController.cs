using Microsoft.AspNetCore.Mvc;
using Zoo.Examples.Contract.Services;

namespace Zoo.Examples.Consumer.Controllers
{
    [ApiController]
    [Route("/api/greeting")]
    [Produces("application/json")]
    public class GreetingController : Controller
    {
        private readonly IGreetingService _greetingService;

        public GreetingController(IGreetingService greetingService)
        {
            _greetingService = greetingService;
        }

        [HttpGet]
        public string Index([FromQuery] string name)
        {
            return _greetingService.SayHi(name);
        }
    }
}