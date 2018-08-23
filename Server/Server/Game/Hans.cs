using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game
{
    public enum Stat
    {
        Satiety, Happy, Energy, Health
    }

    public class StatDictionary : Dictionary<Stat, double>
    {
        private const double StatMin = 0;
        private const double StatMax = 100;
        
        public double this[Stat stat]
        {
            get => base[stat];
            set => base[stat] = Clamp(value);
        }
        
        public static double Clamp(double value)
        {
            return Math.Min(Math.Max(value, StatMin), StatMax);
        }

    }
    
    public class Hans
    {
        
        public string Name { get; set; }

        public Activity[] Activities => _activities.ToArray();

        public Location Location { get; set; }
        public StatDictionary Stats { get; set; } = new StatDictionary();
        private Queue<Activity> _activities = new Queue<Activity>();

        public Hans(string name)
        {
            this.Name = name;
            Stats[Stat.Satiety] = 70;
            Stats[Stat.Happy] = 70;
            Stats[Stat.Energy] = 70;
            Stats[Stat.Health] = 100;
        }

        public Activity CurrentActivity =>  _activities.Count > 0 ? _activities.Peek() : Activity.Idle(); 

        public void AddActivity(Activity activity, Location location)
        {
            if (!activity.Valid(location))
            {
                throw new Exception($"Cannot run activity {activity.Name} at location {location.Name}");
            }
            if (Location == null || !Location.Equals(location)) //TODO what kind of equality do we need here?
            {
                AddActivity(Activity.Move(location));
            }
            AddActivity(activity);
        }
        
        private void AddActivity(Activity activity)
        {
            _activities.Enqueue(activity);
        }

        public void ClearActivities()
        {
            _activities = new Queue<Activity>(Activities.Where(a => a.Cancellable));
        }

        public void Tick()
        {
            //TODO run some  triggers like checking low energy for forcing a Feint activity
            while (!CurrentActivity.Valid(Location))
            {
                var dropped = _activities.Dequeue();
                //TODO log: dropped activity because location is invalid
            }
            CurrentActivity.Run(this);
            if (CurrentActivity.TicksLeft <= 0)
            {
                var dropped = _activities.Dequeue();
                //TODO log: ended activity
            }
        }
    }

}