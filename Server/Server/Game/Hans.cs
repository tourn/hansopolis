using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Server.Game
{
    public class Hans
    {
        private const int StatMin = 0;
        private const int StatMax = 10;
        
        public string Name { get; set; }

        public Activity[] Activities => _activities.ToArray();

        public Location Location { get; set; }
        private int _satiety;
        private int _happy;
        private int _energy;
        private Queue<Activity> _activities = new Queue<Activity>();

        public Hans(string name)
        {
            this.Name = name;
        }

        public int Satiety
        {
            get => _satiety;
            set => _satiety = Clamp(value);
        }

        public int Happy
        {
            get => _happy;
            set => _happy = Clamp(value);
        }

        public int Energy
        {
            get => _energy;
            set => _energy = Clamp(value);
        }

        private static int Clamp(int value)
        {
            return Math.Min(Math.Max(value, StatMin), StatMax);
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
            Activity.Decay().Run(this); //running the decay at the end means stats will never be at their maximum. TODO: Think about this.
            TickActivityDuration();
        }

        private void TickActivityDuration()
        {
            CurrentActivity.Duration -= 1;
            if (CurrentActivity.Duration <= 0)
            {
                var dropped = _activities.Dequeue();
                //TODO log: ended activity
            }
        }
    }

}