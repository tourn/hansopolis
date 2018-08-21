using Microsoft.AspNetCore.Mvc;
using Server.Game;

namespace Server.Services
{
    public interface IGameService
    {
        string SayHi();
        Hans[] Hanses { get; set; }
        Location[] Locations { get; set; }
    }
}