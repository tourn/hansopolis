using System.Collections.Generic;
using Server.Game;
using static Server.Game.LocationFeature;

namespace Server.Services
{
    public class GameService : IGameService
    {
        public Location[] Locations { get; set; }
        public Hans[] Hanses { get; set; }

        public void Tick()
        {
            foreach (var hans in Hanses)
            {
                hans.Activity?.Run(hans);
                Activity.Tick.Run(hans);
            }
        }

        public void ScheduleActivity(Hans hans, Activity activity, Location location)
        {
            hans.Activity = activity;
            hans.Location = location;
        }
    }
}