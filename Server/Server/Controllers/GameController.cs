using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("hi")]
        public ActionResult<string> GetHi()
        {
            return _gameService.SayHi();
        }
        
        [HttpGet("hans")]
        public ActionResult<string> GetHanses()
        {
            return _gameService.Hanses[0].ToString();
        }
        
    }
}