﻿using ExtensionMethods;
using System;
using System.Collections.Generic;
using static ElasticCollision.Data.MobileBall;

namespace ElasticCollision.Data
{
    public abstract class DataAPI
    {
        public abstract void AddBalls(int count, double radius, double mass);
        public static DataAPI CreateBallData()
        {
            return new BallData();
        }

        private class BallData : DataAPI
        {
            private static readonly Random rng = new Random();
            private static int _ballCounter = 0;
            public Area Area { get; }
            private CheckCollisionDelegate CheckCollision { get; set; }

            public BallData()
            {
                Vector _orientationPoint = Vector.vec(0, 0);
                Vector _worldDimensions = Vector.vec(500, 500);
                Area = Area.FromCorners(_orientationPoint, _worldDimensions);
            }

            public override void AddBalls(int count, double radius, double mass)
            {
                for (int i = 0; i < count; i++)
                {
                    AddBall(radius, mass);
                }
            }

            private void AddBall(double radius, double mass)
            {
                var location = Area.Shrink(radius).GetRandomLocation();

                double x = rng.NextDoubleInRange(-100, 100);
                double y = rng.NextDoubleInRange(-100, 100);

                var velocity = new Vector(x, y);

                Ball ball = new Ball(radius, mass, location, velocity);
                new MobileBall(ball, _ballCounter, CheckCollision);
                _ballCounter++;
            }
        }
    }
}
