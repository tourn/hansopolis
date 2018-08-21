using System;
using Server.Game;
using Server.Services;
using Xunit;

namespace GameTest
{
    public class GameServiceTest
    {
        private IGameService _gameService;
        
        public GameServiceTest()
        {
            _gameService = new GameService();
            
            var locations = new[]
            {
                new Location("Home", new[] {LocationFeature.Bed}),
                new Location("Restaurant", new[] {LocationFeature.Table})
            };
            
            var hans1 = new Hans("Peter");
            var hans2 = new Hans("Rudolf");

            var hanses = new[] {hans1, hans2};

            _gameService.Hanses = hanses;
            _gameService.Locations = locations;

        }
        
        [Fact]
        public void getGetHanses()
        {
            Assert.Equal(2, _gameService.Hanses.Length);
        }
    }
}