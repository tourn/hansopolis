using Microsoft.AspNetCore.Mvc;
using Server.Game;

namespace Server.Services
{
    public interface IGameService
    {
        Hans[] Hanses { get; set; }
        Location[] Locations { get; set; }
        void Tick();
        void ScheduleActivity(Hans hans, Activity activity, Location location);
    }
}