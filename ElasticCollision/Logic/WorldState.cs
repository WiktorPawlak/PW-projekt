using System;
using System.Collections.Generic;
using System.Linq; // my beloved
using ElasticCollision.Data;
using ExtensionMethods;
namespace ElasticCollision.Logic
{
    public record WorldState(
         IEnumerable<BallLogic> Balls,
         Area Area
    )
    {
        private static readonly Random rng = new Random();

        public WorldState Proceed(double Δt)
        {
            var newBalls = Balls
                .AsParallel()
                .Select(ball => ball
                        .Collide(Balls)
                        .Collide(Area)
                        .Budge(Δt));
            return this with { Balls = new List<BallLogic>(newBalls) };
        }

        public WorldState AddBall(BallLogic ball)
        {
            return this with { Balls = Balls.Append(ball) };
        }
        public WorldState AddBall(double radius, double mass)
        {
            var location = Area.Shrink(radius).GetRandomLocation();

            double x = rng.NextDoubleInRange(-100, 100);
            double y = rng.NextDoubleInRange(-100, 100);

            var velocity = new Vector(x, y);
            return AddBall(new BallLogic(radius, mass, location, velocity));
        }
        public WorldState AddBalls(int count, double radius, double mass)
        {
            return Enumerable.Range(0, count).Aggregate(this, (state, _) => state.AddBall(radius, mass));
        }
    }
}
