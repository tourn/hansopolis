using System;
using System.Collections.Generic;
using System.Linq;
using static Server.Game.LocationFeature;
namespace Server.Game
{
    using Stats = Dictionary<Stat, double>;

    //we might make Hans immutable and this function return the updated hans
    public delegate void Update(Hans hans);
    
    /**
     * When nothing happens, stats of a Hans decay, meaning it gets more hungry, tired an less happy over time.
     * 
     * each activity can incur a change of stats over its duration. The numbers define the total stat change that will have happened on completing the activity
     * i.e.
     * 
     * eat {
     *   duration: 8 ticks
     *   satiety: +80
     * }
     * 
     * when an activity does not touch a stat, decay it by a default value => happy/energy would be decayed 8 times
     * when an activity updates a stat, do not decay it, => satiety will be exactly 80 more than it was at the start of the eat activity
     *
     * timings for modifying stats for an activity with a duration:
     * - linearly:      add a fraction of the end rewards on each tick
     * - exponentially: add a bit of the end result in the first few ticks, and more the longer the activity goes. this punishes cancelling out of activities
     * - at the end:    modify the stats when the activity is done. This probably doesn't make sense, as waking a Hans in 9/10 ticks of its sleep is leaving it as tired as before.
     *
     *
     * beside from changing stats, activities can also do other things:
     * - award items that have been gathered/crafted
     * - spawn other, forced activities like
     *   - going around stealing wallets can turn into being arrested
     *   - randomly waking up from a nightmare
     *   - being bullied off the playground
     *   - being attacked by a bear in the woods
     */


    public class Activity
    {
        private readonly Dictionary<Stat, double> DefaultDecay = new Dictionary<Stat, double>
        {
            {Stat.Satiety, -1},
            {Stat.Happy, -1},
            {Stat.Energy, -1},
            {Stat.Health, 0}
        };
            
        private Activity(string name,
            LocationFeature[] requiredFeatures = null,
            bool selectable = true,
            bool cancellable = true,
            Stats statChanges = null,
            int duration = 1,
            Update onComplete = null
        )
        {
            Name = name;
            LocationFeatures = requiredFeatures ?? new LocationFeature[]{};
            Selectable = selectable;
            Cancellable = cancellable;
            StatChanges = statChanges ?? new Stats();
            InitialDuration = duration;
            TicksLeft = duration;
            OnComplete = onComplete ?? (hans => { });

            foreach (var stat in (Stat[]) Enum.GetValues(typeof(Stat)))
            {
                if (!StatChanges.ContainsKey(stat))
                {
                    StatChanges.Add(stat, DefaultDecay[stat] * duration);
                }
            }
            
        }

        public Update OnComplete { get; set; }
        public int TicksLeft { get; set; }
        public int InitialDuration { get; set; }
        public Stats StatChanges { get; set; }
        public LocationFeature[] LocationFeatures { get; set; } = {};
        public string Name { get; }
        public bool Selectable { get; set; } = true;
        public bool Cancellable { get; set; } = true;
            
        
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
            if (TicksLeft > 0)
            {
                TicksLeft -= 1;
                UpdateStats(hans);
                if (TicksLeft == 0)
                {
                    OnComplete(hans);
                }
            }
            else
            {
                throw new Exception("Activity has no more ticks left, should not be run");
            }
        }

        private void UpdateStats(Hans hans)
        {
            //for now we only do linear stat updates
            foreach (var (stat, value) in StatChanges)
            {
                var amount = value / InitialDuration;
                hans.Stats[stat] += amount;
            }
        }
        
        //
        // activity implementations
        //

        public static Activity Idle() => new Activity(
            name: "Idle",
            duration: 999,
            selectable: false
        );

        public static Activity Move(Location location) => new Activity(
            name: $"Move to {location.Name}",
            cancellable: false,
            onComplete: hans => { hans.Location = location; }
        );

        public static Activity Eat() => new Activity(
            name: "Eat",
            requiredFeatures: new[] {Table},
            duration: 5,
            statChanges: new Stats
            {
                {Stat.Satiety, 80}
            }
        );

        public static Activity Play() => new Activity(
            name: "Play",
            requiredFeatures: new[] {Playground, Table},
            duration: 5,
            statChanges: new Stats
            {
                {Stat.Happy, 80}
            }
        );

        public static Activity Sleep() => new Activity(
            name: "Sleep",
            requiredFeatures: new[] {Bed},
            duration: 10,
            statChanges: new Stats
            {
                {Stat.Energy, 80},
                {Stat.Happy, -10},
                {Stat.Satiety, -10},
            }
        );

        public static Activity Feint() => new Activity(
            name: "Feint",
            selectable: false,
            cancellable: false,
            duration: 10,
            statChanges: new Stats
            {
                {Stat.Energy, 40},
                {Stat.Happy, -20},
                {Stat.Satiety, -10},
            }
        );
    }
    
}