using PatherySolver.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PatherySolver.UnitTests.Bot
{
    public class BotsEngine_Tests
    {
        public class TestCase : IXunitSerializable
        {
            private string[] expectedHeatMap;
            private string[] map;

            public bool ExpectedIsValid { get; private set; }
            public int[] ExpectedMoves { get; private set; }
            public string Reason { get; private set; }

            public override string ToString()
            {
                var result = Reason;
                if (!ExpectedIsValid)
                    result += ", fails";
                if (ExpectedMoves != null)
                    result += ", counts " + string.Join('+', ExpectedMoves) + " moves";
                if (expectedHeatMap != null)
                    result += ", generates the correct heat map";
                return result + ".";
            }

            public static implicit operator object[](TestCase testCase) => new object[] { testCase };

            public static TestCase Walking(params string[] map)
            {
                return new TestCase {
                    map = map,
                };
            }

            public TestCase OutputsMoves(params int[] moves)
            {
                ExpectedIsValid = true;
                ExpectedMoves = moves;
                return this;
            }

            public TestCase Fails()
            {
                ExpectedIsValid = false;
                return this;
            }

            public TestCase ProducesHeatMap(params string[] heatMap)
            {
                var length = heatMap[0].Length;
                if (heatMap.Skip(1).Any(line => line.Length != length))
                    throw new ArgumentException("All lines must have the same length.", nameof(heatMap));

                expectedHeatMap = heatMap;
                ExpectedIsValid = true;
                return this;
            }

            public TestCase Because(string reason)
            {
                Reason = reason;
                return this;
            }

            public void Deserialize(IXunitSerializationInfo info)
            {
                expectedHeatMap = info.GetValue<string[]>(nameof(expectedHeatMap));
                ExpectedIsValid = info.GetValue<bool>(nameof(ExpectedIsValid));
                ExpectedMoves = info.GetValue<int[]>(nameof(ExpectedMoves));
                map = info.GetValue<string[]>(nameof(map));
                Reason = info.GetValue<string>(nameof(Reason));
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(expectedHeatMap), expectedHeatMap);
                info.AddValue(nameof(ExpectedIsValid), ExpectedIsValid);
                info.AddValue(nameof(ExpectedMoves), ExpectedMoves);
                info.AddValue(nameof(map), map);
                info.AddValue(nameof(Reason), Reason);
            }

            public Map CreateMap() => new(0, map);

            public int[,] GetExpectedHeatMap()
            {
                if (expectedHeatMap == null)
                    return null;

                var result = new int[expectedHeatMap[0].Length, expectedHeatMap.Length];
                for (int y = 0; y < expectedHeatMap.Length; y++)
                {
                    var line = expectedHeatMap[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        result[x, y] = line[x] - '0';
                    }
                }
                return result;
            }
        }

        [Theory(DisplayName = nameof(BotsEngine.Walk))]
        [MemberData(nameof(Walk_GivesCorrectOutput_TestCases))]
        public async Task Walk_GivesCorrectOutput(TestCase because)
        {
            var map = because.CreateMap();
            var botsEngine = new BotsEngine(map);

            await botsEngine.Walk().ConfigureAwait(false);

            Assert.Equal(because.ExpectedIsValid, botsEngine.IsValid);

            if (because.ExpectedMoves != null)
                Assert.Equal(because.ExpectedMoves, map.Moves);

            var expectedHeatMap = because.GetExpectedHeatMap();
            if (expectedHeatMap != null)
            {
                var actualHeatMap = botsEngine.GetHeatMap().Array;
                Assert.Equal(expectedHeatMap, actualHeatMap);
            }
        }
        public static IEnumerable<object[]> Walk_GivesCorrectOutput_TestCases()
        {
            return new object[][] {

                TestCase.Walking(
                    "+++++",
                    "+> ++",
                    "+ + +",
                    "++ =+",
                    "+++++")
                    .Fails()
                    .Because("there is no path to the exit"),

                TestCase.Walking(
                    "+++++",
                    "+>  +",
                    "+{{{+",
                    "+= <+",
                    "+++++")
                    .Fails()
                    .Because("the green bot cannot walk through red gates"),

                TestCase.Walking(
                    "+++++",
                    "+> =+",
                    "+}}}+",
                    "+  <+",
                    "+++++")
                    .Fails()
                    .Because("the red bot cannot walk through green gates"),

                TestCase.Walking(
                    "+++++",
                    "+> /+",
                    "+ //+",
                    "+//=+",
                    "+++++")
                    .Fails()
                    .Because("all ice causes the bot to collide with a wall"),

                TestCase.Walking(
                    "+++++",
                    "+> 1+",
                    "+++ +",
                    "+!+=+",
                    "+++++")
                    .Fails()
                    .Because("the bot enters a teleporter and becomes trapped"),

                TestCase.Walking(
                    "+++++",
                    "+>  +",
                    "+   +",
                    "+  =+",
                    "+++++")
                    .OutputsMoves(4)
                    .Because("there is a simple path to the exit"),

                TestCase.Walking(
                    "+++++",
                    "+> .+",
                    "+ . +",
                    "+. =+",
                    "+++++")
                    .OutputsMoves(4)
                    .Because("bots can walk over fixed empty cells"),

                TestCase.Walking(
                    "+++++",
                    "+>  +",
                    "+++ +",
                    "+=  +",
                    "+++++")
                    .OutputsMoves(6)
                    .Because("the bot must walk around fixed walls"),

                TestCase.Walking(
                    "+++++",
                    "+>  +",
                    "+-- +",
                    "+=  +",
                    "+++++")
                    .OutputsMoves(6)
                    .Because("the bot must walk around user walls"),

                TestCase.Walking(
                    "+++++",
                    "+> !+",
                    "+++ +",
                    "+= 1+",
                    "+++++")
                    .OutputsMoves(8)
                    .Because("a teleporter can only be used once"),

                TestCase.Walking(
                    "+++++",
                    "+>!@+",
                    "+++ +",
                    "+=12+",
                    "+++++")
                    .OutputsMoves(12)
                    .Because("each teleporter can only be used once"),

                TestCase.Walking(
                    "+++++",
                    "+>  +",
                    "+   +",
                    "+===+",
                    "+++++")
                    .OutputsMoves(2)
                    .Because("the bot chooses the closest exit"),

                TestCase.Walking(
                    "+++++",
                    "+>>>+",
                    "+   +",
                    "+ = +",
                    "+++++")
                    .OutputsMoves(2)
                    .Because("the bot chooses the closest starting point"),

                TestCase.Walking(
                    "+++++",
                    "+> C+",
                    "+  A+",
                    "+B =+",
                    "+++++")
                    .OutputsMoves(12)
                    .Because("the bot walks to each checkpoint in order"),

                TestCase.Walking(
                    "+++++++",
                    "+>   A+",
                    "+   + +",
                    "+A  +=+",
                    "+++++++")
                    .OutputsMoves(10)
                    .Because("the bot walks to the closest checkpoint even if another gives a more efficient solution"),

                TestCase.Walking(
                    "+++++",
                    "+A >+",
                    "+++ +",
                    "+= <+",
                    "+++++")
                    .OutputsMoves(8, 10)
                    .Because("the bots can cross any starting point"),

                TestCase.Walking(
                    "+++++",
                    "+> =+",
                    "+ = +",
                    "+= A+",
                    "+++++")
                    .OutputsMoves(6)
                    .Because("a bot can cross an exit"),

                TestCase.Walking(
                    "+++++++++",
                    "+>     ++",
                    "+ ///// +",
                    "+    += +",
                    "+++++++++")
                    .OutputsMoves(7)
                    .Because("moving over ice is multiple moves"),

                TestCase.Walking(
                    "+++++++",
                    "+     +",
                    "+ 1 = +",
                    "+     +",
                    "+ >   +",
                    "+ !   +",
                    "+++++++")
                    .OutputsMoves(7)
                    .Because("bots move up before right"),

                TestCase.Walking(
                    "+++++++",
                    "+     +",
                    "+ = 1 +",
                    "+     +",
                    "+   > +",
                    "+   ! +",
                    "+++++++")
                    .OutputsMoves(7)
                    .Because("bots move up before left"),

                TestCase.Walking(
                    "+++++++",
                    "+   ! +",
                    "+   > +",
                    "+     +",
                    "+ = 1 +",
                    "+     +",
                    "+++++++")
                    .OutputsMoves(7)
                    .Because("bots move down before left"),

                TestCase.Walking(
                    "+++++++",
                    "+ !   +",
                    "+ > 1 +",
                    "+     +",
                    "+   = +",
                    "+     +",
                    "+++++++")
                    .OutputsMoves(7)
                    .Because("bots move right before down"),

                TestCase.Walking(
                    "+++++",
                    "++>}+",
                    "+<+ +",
                    "+{ =+",
                    "+++++")
                    .OutputsMoves(3, 3)
                    .Because("bots can walk through the same colour gates"),

                TestCase.Walking(
                    "+++++++",
                    "+>1  >+",
                    "+!+ +++",
                    "+++=+++",
                    "+++ +++",
                    "+>   >+",
                    "+++++++")
                    .OutputsMoves(6)
                    .Because("starting points are prioritised by y and x position"),

                TestCase.Walking(
                    "+++++++",
                    "++>+  +",
                    "+=/// +",
                    "++///++",
                    "+ /// +",
                    "+  +  +",
                    "+++++++")
                    .OutputsMoves(22)
                    .Because("the path crosses itself over ice"),

                TestCase.Walking(
                    "+++++",
                    "+=+A+",
                    "+>+ +",
                    "+B C+",
                    "+++++")
                    .ProducesHeatMap(
                    "00000",
                    "01010",
                    "01020",
                    "03430",
                    "00000")
                    .Because("the bot walks over cells many times"),

                TestCase.Walking(
                    "+++++",
                    "+>+<+",
                    "+   +",
                    "++=++",
                    "+++++")
                    .ProducesHeatMap(
                    "00000",
                    "00000",
                    "01210",
                    "00200",
                    "00000")
                    .Because("the bots add their heat together"),
            };
        }
    }
}
