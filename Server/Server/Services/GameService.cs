using System.Collections.Generic;
using Server.Game;
using static Server.Game.LocationFeature;

namespace Server.Services
{
    public class GameService : IGameService
    {
        public Location[] Locations { get; set; }
        public Hans[] Hanses { get; set; }

        public GameService()
        {
        }

        public string SayHi()
        {
            return "hi";
        }

        public void Tick()
        {
            foreach (var hans in Hanses)
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