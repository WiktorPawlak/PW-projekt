using System;
using Xunit;
using static Vector;

namespace DataTest
{
    public class VectorTest
    {
        [Fact]
        public void TestCreation()
        {
            Vector v1 = vec(0, 0);
            Assert.Equal(0, v1.x);
            Assert.Equal(0, v1.y);
            Vector v2 = vec(5, -9);
            Assert.Equal(5, v2.x);
            Assert.Equal(-9, v2.y);
        }
        [Fact]
        public void TestAddition()
        {
            var a = vec(0, 0) + vec(0, 0);
            Assert.Equal(0, a.x);
            Assert.Equal(0, a.y);
            var b = vec(5, 5) + vec(5, 5);
            Assert.Equal(vec(10, 10), b);
        }
    }
}
