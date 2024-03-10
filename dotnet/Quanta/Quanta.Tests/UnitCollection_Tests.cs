using System;
using System.Collections.Generic;
using Xunit;

namespace Quanta
{
    public class UnitCollection_Tests
    {
        [Fact]
        public void SIUnit_is_set_correctly()
        {
            var dimension = new Dimension("Unit Test");
            var target = new UnitCollection();
            target.Add(new Unit(dimension, "a", "alpha", "alphas", 0.5));
            target.Add(new Unit(dimension, "b", "beta" , "betas" , 1.0));
            target.Add(new Unit(dimension, "c", "gamma", "gammas", 1.5));

            Unit actual = target.SIUnit;
            Assert.Equal(1.0, actual.RatioToSI);
        }
    }
}
