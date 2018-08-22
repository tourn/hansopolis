using System;
using System.Reflection.Metadata;

namespace Server.Game
{
    public class Hans
    {
        private const int StatMin = 0;
        private const int StatMax = 10;
        
        public string Name { get; set; }
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

        private static int Clamp(int value)
        {
            return Math.Max(Math.Min(value, StatMin), StatMax);
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

}