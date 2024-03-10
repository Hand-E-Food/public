using PatherySolver.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace PatherySolver.UnitTests.Player
{
    public class PlayerEngine_Tests
    {
        public class TestCase : IXunitSerializable
        {
            private string[] map;
            private int walls;

            public int MinimumTotalMoves { get; private set; }
            public string Name { get; private set; }
            public TimeSpan? TimeOut { get; private set; }

            public override string ToString() => Name + " in " + string.Join('+', MinimumTotalMoves) + " moves.";

            public static implicit operator object[](TestCase testCase) => new object[] { testCase };

            public static TestCase Optimize(params string[] map)
            {
                return new TestCase {
                    map = map,
                };
            }

            public TestCase WithWalls(int walls)
            {
                this.walls = walls;
                return this;
            }

            public TestCase InMoves(int moves)
            {
                MinimumTotalMoves = moves;
                return this;
            }

            public TestCase Within(TimeSpan timeOut)
            {
                TimeOut = timeOut;
                return this;
            }

            public TestCase Named(string name)
            {
                Name = name;
                return this;
            }

            public void Deserialize(IXunitSerializationInfo info)
            {
                MinimumTotalMoves = info.GetValue<int>(nameof(MinimumTotalMoves));
                map = info.GetValue<string[]>(nameof(map));
                Name = info.GetValue<string>(nameof(Name));
                TimeOut = TimeSpan.Parse(info.GetValue<string>(nameof(TimeOut)));
                walls = info.GetValue<int>(nameof(walls));
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(MinimumTotalMoves), MinimumTotalMoves);
                info.AddValue(nameof(map), map);
                info.AddValue(nameof(Name), Name);
                info.AddValue(nameof(TimeOut), TimeOut.ToString());
                info.AddValue(nameof(walls), walls);
            }

            public Map CreateMap() => new(walls, map);
        }
        
        [Theory(DisplayName = nameof(PlayerEngine.Optimize))]
        [MemberData(nameof(Optimize_AchievesTheMinimumMoves_TestCases))]
        public async Task Optimize_AchievesTheMinimumMoves(TestCase game)
        {
            var map = game.CreateMap();
            var playerEngine = new PlayerEngine(map);

            var cancellation = game.TimeOut.HasValue
                ? new CancellationTokenSource(game.TimeOut.Value)
                : new CancellationTokenSource();

            playerEngine.OptimalSolutionChanged += (sender, e) =>
            {
                if (playerEngine.OptimalSolution.TotalMoves >= game.MinimumTotalMoves)
                    cancellation.Cancel();
            };

            await playerEngine.Optimize(cancellation.Token).ConfigureAwait(false);

            var expected = game.MinimumTotalMoves;
            var actual = playerEngine.OptimalSolution.TotalMoves;
            if (actual < expected)
                throw new AssertActualExpectedException(expected, actual, $"Optimization was {expected - actual} moves short of the high score.");
        }

        public static IEnumerable<object[]> Optimize_AchievesTheMinimumMoves_TestCases()
        {
            var SimpleTimeOut    = TimeSpan.FromSeconds(1);
            var NormalTimeOut    = TimeSpan.FromMinutes(5);
            var ComplexTimeOut   = TimeSpan.FromMinutes(20);
            var UnlimitedTimeOut = TimeSpan.FromMinutes(60);

            return new object[][] {

                TestCase.Optimize(
                    "+++++++++++++++++++++++++++++",
                    "+>                         ++",
                    "++   +   +                 =+",
                    "+>   2E               A    ++",
                    "++                         =+",
                    "+>        +                ++",
                    "++                 1 +     =+",
                    "+>     I          +  B   + ++",
                    "++       / 3  $     !      =+",
                    "+>   @          /          ++",
                    "++    4   +    H      +    =+",
                    "+>     #  /         +      ++",
                    "++               G+      + =+",
                    "+>                         ++",
                    "++                         =+",
                    "+>                       / ++",
                    "++          G              =+",
                    "+>      D + + /            ++",
                    "++ ++   F    C   /         =+",
                    "+>                         ++",
                    "+++++++++++++++++++++++++++++")
                    .WithWalls(999)
                    .InMoves(2975)
                    .Within(UnlimitedTimeOut)
                    .Named("15959 Ultra Complex Unlimited"),

                TestCase.Optimize(
                    "+++++++++++++++",
                    "++     ++    ++",
                    "++   ++      ++",
                    "+>      A  + ++",
                    "++           ++",
                    "++ +         ++",
                    "++           =+",
                    "+++++++++++++++")
                    .WithWalls(8)
                    .InMoves(43)
                    .Within(SimpleTimeOut)
                    .Named("15984 Simple"),

                TestCase.Optimize(
                    "+++++++++++++++++++",
                    "++   A    +  ++  =+",
                    "++  +           +=+",
                    "++  +        +   =+",
                    "++ +  +       +  =+",
                    "++           +   =+",
                    "++               =+",
                    "++      +        =+",
                    "++               =+",
                    "+> ++            =+",
                    "+++++++++++++++++++")
                    .WithWalls(14)
                    .InMoves(84)
                    .Within(NormalTimeOut)
                    .Named("15985 Normal"),

                TestCase.Optimize(
                    "+++++++++++++++++++++",
                    "+>  + B  +         =+",
                    "+> D               =+",
                    "+>           ++    =+",
                    "+>      /C      +  =+",
                    "+> +          1 +  =+",
                    "+>        +     ++ =+",
                    "+> +            /! =+",
                    "+>    A            =+",
                    "+>      E    /     =+",
                    "+++++++++++++++++++++")
                    .WithWalls(18)
                    .InMoves(291)
                    .Within(ComplexTimeOut)
                    .Named("15986 Complex"),

                TestCase.Optimize(
                    "++++++++++++++++++++",
                    "+>              A{=+",
                    "+>        }      {=+",
                    "+>  {            {=+",
                    "+> C+          + {=+",
                    "+} + +           B{+",
                    "+=}    {          <+",
                    "+=}              }<+",
                    "+=}   + ++   +  {+<+",
                    "+=}    +          <+",
                    "++++++++++++++++++++")
                    .WithWalls(13)
                    .InMoves(175)
                    .Within(ComplexTimeOut)
                    .Named("15987 Dualing Paths"),

                TestCase.Optimize(
                    "+++++++++++++++++++++++++++++",
                    "+>                         ++",
                    "++  A                      =+",
                    "+>   4    + +        /     ++",
                    "++              @     1    =+",
                    "+>         #   B      /    ++",
                    "++   +    F                =+",
                    "+>       $    G DH         ++",
                    "++                 /       =+",
                    "+>                         ++",
                    "++               E         =+",
                    "+>                    +    ++",
                    "++                         =+",
                    "+>           +  +          ++",
                    "++              +    /     =+",
                    "+>     +  3   +          2 ++",
                    "++          C              =+",
                    "+>             +      !    ++",
                    "++                    + I  =+",
                    "+>                         ++",
                    "+++++++++++++++++++++++++++++")
                    .WithWalls(999)
                    .InMoves(2897)
                    .Within(UnlimitedTimeOut)
                    .Named("15988 Ultra Complex Unlimited"),

                TestCase.Optimize(
                    "+++++++++++++++",
                    "++   +       ++",
                    "+>   +       ++",
                    "++         + ++",
                    "++   A   +   ++",
                    "++ +         ++",
                    "++           =+",
                    "+++++++++++++++")
                    .WithWalls(6)
                    .InMoves(40)
                    .Within(SimpleTimeOut)
                    .Named("15989 Simple"),

                TestCase.Optimize(
                    "+++++++++++++++++++",
                    "++  ++     A     =+",
                    "++               =+",
                    "++             + =+",
                    "++               =+",
                    "++               =+",
                    "++               =+",
                    "++     ++   B    =+",
                    "+> +             =+",
                    "+++      +       =+",
                    "+++++++++++++++++++")
                    .WithWalls(11)
                    .InMoves(85)
                    .Within(NormalTimeOut)
                    .Named("15990 Normal"),

                TestCase.Optimize(
                    "+++++++++++++++++++++",
                    "+>  +              =+",
                    "+>                 =+",
                    "+>      C          =+",
                    "+>                 =+",
                    "+>             +   =+",
                    "+> +         +   + =+",
                    "+>             A 1 =+",
                    "+>      !B         =+",
                    "+>++  +            =+",
                    "+++++++++++++++++++++")
                    .WithWalls(20)
                    .InMoves(206)
                    .Within(ComplexTimeOut)
                    .Named("15991 Complex"),

                TestCase.Optimize(
                    "++++++++++++++++++++",
                    "+>              + =+",
                    "+>   !            =+",
                    "+> +     A     +  =+",
                    "+>       1        =+",
                    "+> +  +           =+",
                    "+>                =+",
                    "+>                =+",
                    "+>        +   +   =+",
                    "+>        +       =+",
                    "+>                =+",
                    "+>         +      =+",
                    "+>      + +       =+",
                    "+>            +   =+",
                    "+>                =+",
                    "++++++++++++++++++++")
                    .WithWalls(30)
                    .InMoves(211)
                    .Within(ComplexTimeOut)
                    .Named("15992 Thirty")
            };
        }
    }
}
