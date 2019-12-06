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

            var intersections = wires.FindIntersections(point);

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

            var intersections = wires.FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestDistance = distances.Values.Min();

            Assert.Equal(6,closestDistance);
        }


        [Fact]
        public void GetSteps30()
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

            var intersections = wires.FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestPoint = distances.OrderBy(d => d.Value).First().Key;

            var shortestWire = wires.FindShortestWire(intersections).Values.Min();

            Assert.Equal(30, shortestWire);
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

            var intersections = wires.FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestDistance = distances.Values.Min();

            Assert.Equal(159, closestDistance);
        }

        [Fact]
        public void GetSteps610()
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

            var intersections = wires.FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestPoint = distances.OrderBy(d => d.Value).First().Key;

            var shortestWire = wires.FindShortestWire(intersections).Values.Min();

            Assert.Equal(610, shortestWire);
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

            var intersections = wires.FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestDistance = distances.Values.Min();

            Assert.Equal(135, closestDistance);
        }

        [Fact]
        public void GetSteps410()
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

            var intersections = wires.FindIntersections(point);
            var distances = intersections.FindDistances(point);

            var closestPoint = distances.OrderBy(d => d.Value).First().Key;

            var shortestWire = wires.FindShortestWire(intersections).Values.Min();

            Assert.Equal(410, shortestWire);
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
