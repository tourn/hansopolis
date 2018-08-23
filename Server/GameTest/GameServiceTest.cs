using System;
using Server.Game;
using Server.Services;
using Xunit;
using static Server.Game.Stat;

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
        public void HansNeedsATableForEating()
        {
            var locations = new[]
            {
                new Location("Crummy restaurant without a table")
            };
            _gameService = new GameService()
            {
                Hanses = new[] {new Hans("Peter"){ Location = locations[0]}},
                Locations = locations
            };
            
            var hans = _gameService.Hanses[0];
            Assert.Throws<Exception>(() =>
            {
                _gameService.ScheduleActivity(hans, Activity.Eat(), _gameService.Locations[0]);
            });
            
            
        }

        [Fact]
        public void TicksDecaySomeStats()
        {
            _gameService = new GameService()
            {
                Hanses = new[] {new Hans("Peter")
                {
                    Stats = new StatDictionary()
                    {
                        {Satiety, 7},
                        {Energy, 4},
                        {Happy, 0},
                        {Health, 50},
                    }
                }},
            };
            
            _gameService.Tick();

            var hans = _gameService.Hanses[0];
            
            Assert.Equal(6, hans.Stats[Satiety]);
            Assert.Equal(3, hans.Stats[Energy]);
            Assert.Equal(0, hans.Stats[Happy]);
            Assert.Equal(50, hans.Stats[Health]);
            
        }
        
        [Fact]
        public void HansNeedsToWalkToTheRestaurant()
        {
            var locations = new[]
            {
                new Location("Home"),
                new Location("Restaurant")
                {
                    LocationFeatures = new[] {LocationFeature.Table}
                }
            };
            
            _gameService = new GameService()
            {
                Hanses = new[] {new Hans("Peter"){Location = locations[0]}},
                Locations = locations
            };
            
            var hans = _gameService.Hanses[0];
            _gameService.ScheduleActivity(hans, Activity.Eat(), _gameService.Locations[1]);
            Assert.Contains("Move", hans.CurrentActivity.Name);
            _gameService.Tick();
            Assert.Contains("Eat", hans.CurrentActivity.Name);
            _gameService.Tick();
            _gameService.Tick();
            _gameService.Tick();
            _gameService.Tick();
            _gameService.Tick();
            Assert.Contains("Idle", hans.CurrentActivity.Name);
            
            
        }
    }
}