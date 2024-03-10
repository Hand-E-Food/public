using System;
using System.Collections.Generic;
using Xunit;

namespace Quanta
{
    public class Unit_Tests
    {
        [Fact(DisplayName = "new Unit")]
        public void Constructor_populates_all_fields()
        {
            Dimension dimension = new Dimension("Unit Test");
            string abbreviation = "a";
            string singularName = "alpha";
            string pluralName = "alphas";
            double ratioToSI = 1.5;

            Unit target = new Unit(dimension, abbreviation, singularName, pluralName, ratioToSI);

            Assert.Same(dimension, target.Dimension);
            Assert.Equal(abbreviation, target.Abbreviation);
            Assert.Equal(singularName, target.SingularName);
            Assert.Equal(pluralName, target.PluralName);
            Assert.Equal(ratioToSI, target.RatioToSI);
            Assert.Contains(target, dimension.Units);
        }

        [Theory(DisplayName = "new Unit throws ArgumentNullException"), MemberData("Constructor_throws_ArgumentNullException_for_null_arguments_Data")]
        public void Constructor_throws_ArgumentNullException_for_null_arguments(Dimension dimension, string abbreviation, string singularName, string pluralName, string paramName)
        {
            double ratioToSI = 1.5;
            var ex = Assert.Throws<ArgumentNullException>(() => new Unit(dimension, abbreviation, singularName, pluralName, ratioToSI));
            Assert.Equal(paramName, ex.ParamName);
        }
        public static IEnumerable<object[]> Constructor_throws_ArgumentNullException_for_null_arguments_Data()
        {
            yield return new object[] { null                      , "a" , "alpha", "alphas", "dimension"    };
            yield return new object[] { new Dimension("Unit Test"), null, "alpha", "alphas", "abbreviation" };
            yield return new object[] { new Dimension("Unit Test"), "a" , null   , "alphas", "singularName" };
            yield return new object[] { new Dimension("Unit Test"), "a" , "alpha", null    , "pluralName"   };
        }

        [Fact(DisplayName = "new Unit throws ArgumentException")]
        public void Constructor_throws_ArgumentException_for_invalid_arguments()
        {
            Dimension dimension = new Dimension("Unit Test");
            string abbreviation = "a";
            string singularName = "alpha";
            string pluralName = "alphas";
            double ratioToSI = 0;

            var ex = Assert.Throws<ArgumentNullException>(() => new Unit(dimension, abbreviation, singularName, pluralName, ratioToSI));
            Assert.Equal("ratioToSI", ex.ParamName);
        }

        //TODO: Test Unit.Equals

        //TODO: Test Unit.GetHashCode
    }
}
