using System;

namespace Server.Game
{
    public class Hans
    {
        public string Name { get; }
        private int _satiety;
        private int _happy;
        private int _energy;

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

        private int Clamp(int value)
        {
            return value; //TODO clamp
        }

        public Hans(string name)
        {
            this.Name = name;
        }

        public string Activity { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Satiety)}: {Satiety}, {nameof(Happy)}: {Happy}, {nameof(Energy)}: {Energy}, {nameof(Activity)}: {Activity}";
        }
    }

    public class Location
    {
        private string name;
        private LocationFeature[] LocationFeatures;

        public Location(string name, LocationFeature[] locationFeatures)
        {
            LocationFeatures = locationFeatures;
            this.name = name;
        }
    }

    //
    // ACTIVITIES
    //
    abstract class Activity
    {
        //TODO: do this in c# style
        public abstract LocationFeature[] GetPrerequisites();

        public bool Valid(Location location)
        {
            //TODO check if location contains the required prerequisites
            return true;
        }

        public void Run(Hans hans, Location location)
        {
            if (Valid(location))
            {
                DoRun(hans, location);
            };
        }

        protected abstract void DoRun(Hans hans, Location location);
        
    }

    class EatActivity : Activity
    {
        public override LocationFeature[] GetPrerequisites()
        {
            return new[] {LocationFeature.Table};
        }

        protected override void DoRun(Hans hans, Location location)
        {
            hans.Satiety += 10;
            // location.foodStuffs.remove(1);
        }

        public override string ToString()
        {
            return "EatActivity";
        }
    }

    //
    // Location features
    //
    
    public enum LocationFeature
    {
        Table, Bed
    }
}