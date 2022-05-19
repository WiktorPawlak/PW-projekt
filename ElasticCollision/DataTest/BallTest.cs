using System;
using ElasticCollision.Data;
using Xunit;
using static ElasticCollision.Data.Vector;

namespace DataTest
{
    public class BallTest
    {
        [Fact]
        public void TestModificationWorksTheWayIThinkItWorks()
        {
            Ball a = new(10, 20, vec(1, 2), vec(3, 4));
            var b = a with { Location = vec(10, 20) };
            Assert.Equal(vec(1, 2), a.Location);
            Assert.Equal(vec(10, 20), b.Location);
            Assert.Equal(vec(3, 4), b.Velocity);

        }
        [Fact]
        public void TestBallsTouching()
        {
            Ball a = new(5, 0, vec(0, 0), vec(0, 0));
            Ball d = new(15, 0, vec(0, 0), vec(0, 0));
            Ball b = new(5, 0, vec(9, 1), vec(0, 0));
            Ball c = new(5, 0, vec(18, 0), vec(0, 0));
            Assert.True(a.Touching(d));
            Assert.True(a.Touching(b));
            Assert.True(b.Touching(c));
            Assert.False(a.Touching(c));
            Assert.True(d.Touching(c));
        }

        [Fact]
        public void TestBallingArea()
        {
            Ball a = new(5, 0, vec(0, 0), vec(0, 0));
            Ball b = new(5, 0, vec(20, 20), vec(0, 0));
            Ball d = new(15, 0, vec(0, 0), vec(0, 0));
            Area ar1 = Area.FromCorners(vec(-10, -10), vec(10, 10));
            Assert.True(a.Within(ar1));
            Assert.False(b.Within(ar1));
            Assert.False(d.Within(ar1));
        }
        [Fact]
        public void TestMovement()
        {
            Ball a = new(5, 0, vec(0, 0), vec(0, 0));
            Ball b = new(5, 0, vec(0, 0), vec(1, 0));
            Ball c = new(5, 0, vec(5, 0), vec(0, 5));
            Assert.Equal(a.Budge(0).Location, vec(0, 0));
            Assert.Equal(a.Budge(20).Location, vec(0, 0));
            Assert.Equal(b.Budge(0).Location, vec(0, 0));
            Assert.Equal(b.Budge(1).Location, vec(1, 0));
            Assert.Equal(b.Budge(10).Location, vec(10, 0));
            Assert.Equal(c.Budge(10).Location, vec(5, 50));
        }

        [Fact]
        public void TestKEcalculation()
        {
            Ball b = new(10, 10, vec(0, 0), vec(1, 0));
            Assert.Equal(5, b.KineticEnergy);
        }

        [Fact]
        public void TestMomentum()
        {
            Ball a = new(10, 10, vec(0, 0), vec(1, 0));
            Ball b = new(10, 10, vec(0, 0), vec(0, 0));
            Ball c = new(1, 1, vec(0, 0), vec(0, 0));
            Assert.Equal(vec(10, 0), a.Momentum);
            Assert.Equal(vec(0, 0), b.Momentum);
            Assert.Equal(vec(1, 0), b.ApplyImpulse(vec(10, 0)).Velocity);
            Assert.Equal(vec(0, 0), a.ApplyImpulse(vec(-10, 0)).Velocity);
            Assert.Equal(vec(10, 0), c.ApplyImpulse(vec(10, 0)).Velocity);
        }

        [Fact]
        public void TestBallApproaching()
        {
            Ball a = new(10, 10, vec(0, 0), vec(0, 0));
            Ball b_f = new(10, 10, vec(-1, 5), vec(1, 0));
            Ball b_b = new(10, 10, vec(-1, 5), vec(-1, 0));
            Ball a_f = new(10, 10, vec(1, 0), vec(1, 0));
            Ball a_b = new(10, 10, vec(1, 0), vec(-10, 0));

            Assert.True(b_f.Approaching(a));
            Assert.True(a_b.Approaching(a));

            Assert.False(a.Approaching(a));
            Assert.False(a_f.Approaching(a_f));

            Assert.False(b_b.Approaching(a));
            Assert.False(a_f.Approaching(a));
        }


        [Fact]
        public void TestBallImpact() //unu
        {
            Ball a = new(10, 10, vec(-4, 0), vec(1, 0));
            Ball b = new(10, 10, vec(4, 0), vec(-1, 0));
            Ball c = new(10, 10, vec(4, 3), vec(-1, 0));
            Assert.Equal(a.CollisionImpulse(b), -b.CollisionImpulse(a));
            Assert.NotEqual(a.CollisionImpulse(b), a.CollisionImpulse(c));
            Assert.True(a.CollisionImpulse(c).Y < 0);
            Assert.Equal(a.CollisionImpulse(b), vec(-20, 0));
        }
    }
}
