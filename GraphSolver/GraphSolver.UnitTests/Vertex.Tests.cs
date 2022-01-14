using System.Linq;
using Xunit;

namespace GraphSolver
{
    public class Vertex_Tests
    {
        [Fact]
        public void WalkableEdges_ReturnsOnlyEdgesThatCanBeWalkedFromThisVertex()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var d = new Vertex("d");
            var e = new Vertex("e");
            var ab = new Edge("ab", a, b) { IsBidirectional = true  };
            var ca = new Edge("ca", c, a) { IsBidirectional = true  };
            var ad = new Edge("ad", a, d) { IsBidirectional = false };
            var ea = new Edge("ea", e, a) { IsBidirectional = false };
            var bc = new Edge("bc", b, c) { IsBidirectional = true  };

            var actual = a.WalkableEdges.ToList();

            Assert.Contains(ab, actual);
            Assert.Contains(ca, actual);
            Assert.Contains(ad, actual);
            Assert.DoesNotContain(ea, actual);
            Assert.DoesNotContain(bc, actual);
        }
    }
}
