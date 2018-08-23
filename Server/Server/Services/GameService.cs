using Server.Game;

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
                hans.Tick();
            }
        }

        public void ScheduleActivity(Hans hans, Activity activity, Location location)
        {
            hans.AddActivity(activity, location);
        }
    }
}