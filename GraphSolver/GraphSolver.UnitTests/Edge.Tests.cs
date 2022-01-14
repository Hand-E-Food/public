using System;
using Xunit;

namespace GraphSolver
{
    public class Edge_Tests
    {
        [Fact]
        public void CanWalkFrom_StartVertexWhenBidirectional_IsAllowed()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b) { IsBidirectional = true };

            var actual = ab.CanWalkFrom(a);

            Assert.True(actual);
        }

        [Fact]
        public void CanWalkFrom_StartVertexWhenUnidirectional_IsAllowed()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b) { IsBidirectional = false };

            var actual = ab.CanWalkFrom(a);

            Assert.True(actual);
        }

        [Fact]
        public void CanWalkFrom_EndVertexWhenBidirectional_IsAllowed()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b) { IsBidirectional = true };

            var actual = ab.CanWalkFrom(b);

            Assert.True(actual);
        }

        [Fact]
        public void CanWalkFrom_EndVertexWhenUnidirectional_IsNotAllowed()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b) { IsBidirectional = false };

            var actual = ab.CanWalkFrom(b);

            Assert.False(actual);
        }

        [Fact]
        public void CanWalkFrom_OtherVertexWhenBidirectional_IsNotAllowed()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var ab = new Edge("ab", a, b) { IsBidirectional = true };

            var actual = ab.CanWalkFrom(c);

            Assert.False(actual);
        }

        [Fact]
        public void CanWalkFrom_OtherVertexWhenUnidirectional_IsNotAllowed()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var ab = new Edge("ab", a, b) { IsBidirectional = false };

            var actual = ab.CanWalkFrom(c);

            Assert.False(actual);
        }

        [Fact]
        public void GetOtherVertex_WhenWalkingFromStartVertex_ReturnEndVertex()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b);

            var actual = ab.GetOtherVertex(a);
            var expected = b;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherVertex_WhenWalkingFromEndVertex_ReturnStartVertex()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b);

            var actual = ab.GetOtherVertex(b);
            var expected = a;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherVertex_WhenEdgeIsALoop_ReturnsOnlyVertex()
        {
            var a = new Vertex("a");
            var aa = new Edge("aa", a, a);

            var actual = aa.GetOtherVertex(a);
            var expected = a;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetOtherVertex_WhenWalkingFromVertexNotJoinedToThisEdge_ThrowsArgumentException()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var ab = new Edge("ab", a, b);

            Assert.ThrowsAny<ArgumentException>(() => ab.GetOtherVertex(c));
        }

        [Fact]
        public void GetOtherVertex_WhenWalkingFromNull_ThrowsArgumentNullException()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b);

            var ex = Assert.ThrowsAny<ArgumentNullException>(() => ab.GetOtherVertex(null));
        }

        [Fact]
        public void Constructor_AddsEdgeToVertices()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var ab = new Edge("ab", a, b);
            var bc = new Edge("bc", b, c);
            var ca = new Edge("ca", c, a);

            Assert.Contains(ab, a.Edges);
            Assert.Contains(ca, a.Edges);
            Assert.Contains(ab, b.Edges);
            Assert.Contains(bc, b.Edges);
            Assert.Contains(ca, c.Edges);
            Assert.Contains(bc, c.Edges);
            Assert.DoesNotContain(bc, a.Edges);
            Assert.DoesNotContain(ca, b.Edges);
            Assert.DoesNotContain(ab, c.Edges);
        }
    }
}
