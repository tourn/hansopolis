namespace Server.Game
{
    public class Coordinates
    {
        public int X { get; }
        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    
    public class Location
    {
        public string Name { get; set; }
        public LocationFeature[] LocationFeatures { get; set; } = { };
        public Coordinates Coordinates { get; }

        public string[] Actions
        {
            get
            {
                //TODO maybe we can loop over each activity and check whether it is a valid location for it?
                return new[] {"eat"};
            }
        }

        public Location(string name)
        {
            Name = name;
            Coordinates = new Coordinates(0,0);
        }

        public Location(string name, Coordinates coordinates)
        {
            Name = name;
            Coordinates = coordinates;
        }
    }

    //
    // Location features
    //
    
    public enum LocationFeature
    {
        Table, Bed, Playground
    }

}