using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
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
        
        [HttpGet("hans/{id}")]
        public ActionResult<string> GetHans(int id)
        {
            return Json(_gameService.Hanses[id]);
        }
        
        [HttpGet("hans")]
        public ActionResult<string> GetHanses()
        {
            return Json(_gameService.Hanses);
        }
        
    }
}