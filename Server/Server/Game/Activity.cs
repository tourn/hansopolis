using System.Runtime.InteropServices.WindowsRuntime;
using static Server.Game.LocationFeature;
namespace Server.Game
{

    delegate void UpdateActivity(Hans hans, Location location);

    class Activity
    {
        public LocationFeature[] LocationFeatures { get; set; }
        public UpdateActivity Update { get; set; }
        
        public bool Valid(Location location)
        {
            //TODO check if location contains the required prerequisites
            return true;
        }

        public void Run(Hans hans, Location location)
        {
            if (Valid(location))
            {
                Update(hans, location);
            };
        }
        
        public static Activity Tick => new Activity
        {
            LocationFeatures = new[] {Table},
            Update = (hans, location) =>
            {
                const int decay = 1;
                hans.Satiety -= decay;
                hans.Happy -= decay;
                hans.Energy -= decay;
            }
        };
        
        public static Activity Eat => new Activity
        {
            LocationFeatures = new[] {Table},
            Update = (hans, location) => { hans.Satiety += 10; }
        };
        
        public static Activity Play => new Activity
        {
            LocationFeatures = new[] {Playground, Table},
            Update = (hans, location) => { hans.Happy += 10; }
        };
        
        public static Activity Sleep => new Activity
        {
            LocationFeatures = new[] {Bed},
            Update = (hans, location) => { hans.Energy += 10; }
        };
        
        public static Activity Feint => new Activity
        {
            Update = (hans, location) =>
            {
                hans.Energy += 2;
                hans.Happy -= 5;
            }
        };
    }
    
}