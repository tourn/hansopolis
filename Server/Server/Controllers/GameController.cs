using System;
using Microsoft.AspNetCore.Mvc;
using Server.Game;
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
        
        [HttpGet("location")]
        public ActionResult<string> GetLocations()
        {
            return Json(_gameService.Locations);
        }
        
        [HttpPost("hans/{id}/do")]
        public ActionResult<string> DoActivity(int hansId, [FromBody] DoRequest body)
        {
            var hans = _gameService.Hanses[hansId];
            var activity = mapStringToActivity(body.Activity);
            var location = _gameService.Locations[body.Location];
            _gameService.ScheduleActivity(hans, activity, location);
            return Ok();
        }

        private Activity mapStringToActivity(String s)
        {
            switch (s)
            {
                case "eat": return Activity.Eat();
                case "sleep": return Activity.Sleep();
                case "play": return Activity.Play();
            }

            return null;
        }
        
        [HttpPost("tick")]
        public ActionResult<string> Tick()
        {
            _gameService.Tick();
            return Json(_gameService.Hanses);
        }
        
        
    }
}