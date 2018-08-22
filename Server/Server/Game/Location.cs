namespace Server.Game
{
    public class Location
    {
        public string Name { get; set; }
        public LocationFeature[] LocationFeatures { get; set; }

        public Location(string name)
        {
            Name = name;
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