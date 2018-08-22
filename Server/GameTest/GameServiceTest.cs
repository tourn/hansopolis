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
            var locations = new[]
            {
                new Location("Home")
                {
                    LocationFeatures = new[] {LocationFeature.Bed}
                },
                new Location("Restaurant")
                {
                    LocationFeatures = new[] {LocationFeature.Table}
                },
                new Location("Playground")
                {
                    LocationFeatures = new[] {LocationFeature.Playground}
                },
            };
            
            var hanses = new[]
            {
                new Hans("Peter"),
                new Hans("Rudolf")
            };

            _gameService = new GameService
            {
                Hanses = hanses,
                Locations = locations
            };
        }
        
        [Fact]
        public void getGetHanses()
        {
            Assert.Equal(2, _gameService.Hanses.Length);
        }
    }
}