using Server.Game;
using Xunit;
using static Server.Game.Stat;

namespace GameTest
{
    public class StatsTest
    {
        [Fact]
        public void ClampInBounds()
        {
            Assert.Equal(StatDictionary.Clamp(50), 50);
        }
        
        [Fact]
        public void ClampAbove()
        {
            Assert.Equal(StatDictionary.Clamp(500), 100);
        }
        
        [Fact]
        public void ClampBelow()
        {
            Assert.Equal(StatDictionary.Clamp(-50), 0);
        }

    }
    
    public class HansTest
    {
        [Fact]
        public void HansGottaEat()
        {
            var restaurant = new Location("Restaurant")
            {
                LocationFeatures = new[] {LocationFeature.Table}
            };
            var hans = new Hans("Peter") {Location = restaurant};
            
            Assert.Equal(70, hans.Stats[Satiety]);
            hans.AddActivity(Activity.Eat(), restaurant);
            hans.Tick();
            
            Assert.Equal(86, hans.Stats[Satiety]);
            
        }
    }
}