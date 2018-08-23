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
                new Hans("Peter")
                {
                    Location = locations[0]
                },
                new Hans("Rudolf")
                {
                    Location = locations[0]
                }
            };

            _gameService = new GameService
            {
                Hanses = hanses,
                Locations = locations
            };
        }

        [Fact]
        public void HansGottaEat()
        {
            _gameService = new GameService()
            {
                Hanses = new[] {new Hans("Peter")},
                Locations = new[]
                {
                    new Location("Restaurant")
                    {
                        LocationFeatures = new[] {LocationFeature.Table}
                    }
                }
            };
            
            var hans = _gameService.Hanses[0];
            Assert.Equal(0, hans.Satiety);
            
            _gameService.ScheduleActivity(hans, Activity.Eat, _gameService.Locations[0]);
            _gameService.Tick();
            
            Assert.Equal(9, hans.Satiety);
            
        }

        [Fact]
        public void HansNeedsATableForEating()
        {
            _gameService = new GameService()
            {
                Hanses = new[] {new Hans("Peter")},
                Locations = new[]
                {
                    new Location("Restaurant without a table")
                }
            };
            
            var hans = _gameService.Hanses[0];
            Assert.Throws<Exception>(() =>
            {
                _gameService.ScheduleActivity(hans, Activity.Eat, _gameService.Locations[0]);
                _gameService.Tick();
            });
            
            
        }

        [Fact]
        public void TicksDecayStats()
        {
            _gameService = new GameService()
            {
                Hanses = new[] {new Hans("Peter")
                {
                    Satiety = 7,
                    Energy = 4,
                    Happy = 0
                }},
            };
            
            _gameService.Tick();

            var hans = _gameService.Hanses[0];
            
            Assert.Equal(6, hans.Satiety);
            Assert.Equal(3, hans.Energy);
            Assert.Equal(0, hans.Happy);
            
        }
    }
}