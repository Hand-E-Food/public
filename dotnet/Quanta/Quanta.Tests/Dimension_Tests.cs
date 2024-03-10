using System;
using Xunit;

namespace Quanta
{
    public class Dimension_Tests
    {
        [Fact(DisplayName = "new Dimension")]
        public void Constructor_populates_fields()
        {
            string name = "Unit Test";

            var target = new Dimension(name);

            Assert.Equal(name, target.Name);
            Assert.NotNull(target.Units);
        }

        [Fact(DisplayName = "new Dimension throws ArgumentNullException")]
        public void Constructor_throws_ArgumentNullException_when_argument_is_null()
        {
            string name = null;
            Assert.Throws<ArgumentNullException>(() => new Dimension(name));
        }

        [Fact(DisplayName = "Dimension[string]")]
        public void Default_property_returns_named_Unit()
        {
            var target = new Dimension("Test");
            var units = new[] {
                new Unit(target, "a", "alpha", "alphas", 0.5),
                new Unit(target, "b", "beta" , "betas" , 1.5),
                new Unit(target, "c", "gamma", "gammas", 1.0),
            };

            var expected = units[1];
            var actual = target["b"];
            Assert.Same(expected, actual);
        }

        [Fact(DisplayName = "Dimension.SIUnit")]
        public void SIUnit_returns_unit_with_ratio_of_1()
        {
            var target = new Dimension("Test");
            var units = new[] {
                new Unit(target, "a", "alpha", "alphas", 0.5),
                new Unit(target, "c", "gamma", "gammas", 1.0),
                new Unit(target, "b", "beta" , "betas" , 1.5),
            };

            var expected = units[1];
            var actual = target.SIUnit;
            Assert.Same(expected, actual);
        }
    }
}
