using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Day03.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void GetDictinctIntersections()
        {
            var wireTraces = new List<string>()
            {
                "R8,U5,L5,D3",
                "U7,R6,D4,L4"
            };
            var wires = new List<Wire>();
            var point = new Point();

            foreach (var wireTrace in wireTraces)
            {
                var traces = wireTrace.GetTraces();

                wires.Add(new Wire(point, traces));
            }

            var lPoints = wires.Select(w => w.Path);
            var allPoints = lPoints.SelectMany(p => p);
            var allPointsGrouped = allPoints.GroupBy(p => p, new PointComparer());

            var intersections = allPointsGrouped.Where(p => p.Count() > 1 && !p.First().Equals(point)).Select(p => p.Key);


            Assert.True(intersections.Count() > 1);
        }

        [Fact]
        public void GetClosestDistance6()
        {
            var wireTraces = new List<string>()
            {
                "R8,U5,L5,D3",
                "U7,R6,D4,L4"
            };

            var wires = new List<Wire>();
            var point = new Point();

            foreach (var wireTrace in wireTraces)
            {
                var traces = wireTrace.GetTraces();

                wires.Add(new Wire(point, traces));
            }

            var intersections = wires.Select(w => w.Path).FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestDistance = distances.Values.Min();

            Assert.Equal(6,closestDistance);
        }

        [Fact]
        public void GetClosestDistance159()
        {
            var wireTraces = new List<string>()
            {
                "R75,D30,R83,U83,L12,D49,R71,U7,L72",
                "U62,R66,U55,R34,D71,R55,D58,R83"
            };

            var wires = new List<Wire>();
            var point = new Point();

            foreach (var wireTrace in wireTraces)
            {
                var traces = wireTrace.GetTraces();

                wires.Add(new Wire(point, traces));
            }

            var intersections = wires.Select(w => w.Path).FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestDistance = distances.Values.Min();

            Assert.Equal(159, closestDistance);
        }

        [Fact]
        public void GetClosestDistance135()
        {
            var wireTraces = new List<string>()
            {
                "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
                "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"
            };

            var wires = new List<Wire>();
            var point = new Point();

            foreach (var wireTrace in wireTraces)
            {
                var traces = wireTrace.GetTraces();

                wires.Add(new Wire(point, traces));
            }

            var intersections = wires.Select(w => w.Path).FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestDistance = distances.Values.Min();

            Assert.Equal(135, closestDistance);
        }

        [Fact]
        public void CheckDistances()
        {
            var intersections = new List<Point>(){
                new Point (5,5),
                new Point (-5,5),
                new Point (-5,-5),
                new Point (5,-5),
            };

            var distances = intersections.FindDistances(new Point(0,0));

            Assert.True(distances.Values.All(d => d.Equals(10)));
        }
    }
}
