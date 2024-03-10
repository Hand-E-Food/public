using Xunit;

namespace TableTennis.Models
{
    public class Player_ctor_Tests
    {
        [Fact(DisplayName = "Player.ctor")]
        public void Populates_properties()
        {
            string name = "Unit test name";
            string details = "Unit test details";
            Player target = new Player(name, details);
            Assert.Equal(name, target.Name);
            Assert.Equal(details, target.Details);
        }
    }
}
