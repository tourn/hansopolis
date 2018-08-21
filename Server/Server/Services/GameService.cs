using System.Collections.Generic;
using Server.Game;
using static Server.Game.LocationFeature;

namespace Server.Services
{
    public class GameService : IGameService
    {
        private Location[] locations;
        private Hans[] hanses;
        
        public GameService()
        {
            locations = new[]
            {
                new Location("Home", new[] {Bed}),
                new Location("Restaurant", new[] {Table})
            };
            
            var hans1 = new Hans("Peter");
            var hans2 = new Hans("Rudolf");

            hanses = new[] {hans1, hans2};
        }

        public string SayHi()
        {
            return "hi";
        }

        public Hans[] Hanses => hanses;

        public void Tick()
        {
            foreach (var hans in hanses)
            {
                ProcessCurrentActivity(hans);
            }
        }

        private void ProcessCurrentActivity(Hans hans)
        {
            throw new System.NotImplementedException();
        }
    }
}