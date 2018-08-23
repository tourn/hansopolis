using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using static Server.Game.LocationFeature;
namespace Server.Game
{

    //we might make Hans immutable and this function return the updated hans
    delegate void Update(Hans hans);

    public class Activity
    {
        private Activity(string name)
        {
            Name = name;
        }

        public LocationFeature[] LocationFeatures { get; set; } = {};
        private Update Update { get; set; }
        public string Name { get; }
        
        public bool Valid(Location location)
        {
            // at some point we might need to differentiate whether ALL or ONE of the features is required
            if (LocationFeatures.Length == 0)
            {
                return true;
            }

            foreach (var requiredFeature in LocationFeatures)
            {
                if (location.LocationFeatures.Contains(requiredFeature))
                {
                    return true;
                }
            }
            
            return false;
        }

        public void Run(Hans hans)
        {
            if (Valid(hans.Location))
            {
                Update(hans);
                hans.Activity = null;
            }
            else
            {
                //TODO exceptions really aren't the way i want to handle this here
                throw new Exception("Location is invalid");
            }
        }
        
        public static Activity Tick => new Activity("Tick")
        {
            Update = (hans) =>
            {
                const int decay = 1;
                hans.Satiety -= decay;
                hans.Happy -= decay;
                hans.Energy -= decay;
            }
        };

        public static Activity Move(Location location) => new Activity("Move")
        {
            Update = (hans) => { hans.Location = location; }
        };
        
        public static Activity Eat => new Activity("Eat")
        {
            LocationFeatures = new[] {Table},
            Update = (hans) => { hans.Satiety += 10; }
        };
        
        public static Activity Play => new Activity("Play")
        {
            LocationFeatures = new[] {Playground, Table},
            Update = (hans) => { hans.Happy += 10; }
        };
        
        public static Activity Sleep => new Activity("Sleep")
        {
            LocationFeatures = new[] {Bed},
            Update = (hans) => { hans.Energy += 10; }
        };
        
        public static Activity Feint => new Activity("Feint")
        {
            Update = (hans) =>
            {
                hans.Energy += 2;
                hans.Happy -= 5;
            }
        };
    }
    
}