using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day03
{
    public class Wire
    {
        private Point _centralPort;
        private List<Point> _path;

        public Wire(Point centralPort)
        {
            Traces = new List<Trace>();
            _centralPort = centralPort;
        }

        public Wire(Point centralPort, List<Trace> traces)
        {
            Traces = traces;
            _centralPort = centralPort;
            _path = GenerateTracePath(centralPort);
        }

        public List<Trace> Traces { get; set; }

        public List<Point> Path
        {
            get
            {
                return _path ?? GenerateTracePath(_centralPort);
            }
        }

        private List<Point> GenerateTracePath(Point startingPoint)
        {
            var path = new List<Point>();
            path.Add(startingPoint);
            foreach (var trace in Traces)
            {
                var endOfPath = path.Last();

                var tracePath = endOfPath.TracePath(trace);

                path.AddRange(tracePath);
            }

            return path;
        }
    }

    public static class WireExtension
    {
        public static List<Point> FindIntersections(this IEnumerable<Wire> wires, Point startingPoint)
        {
            var intersections = wires.Select(w => w.Path.Distinct().ToList()).FindIntersections(startingPoint);

            return intersections;
        }

        public static int StepsToPoint(this Wire wire, Point endPoint)
        {
            if (!wire.Path.Contains(endPoint, new PointComparer()))
            {
                return 0;
            }

            var steps = 0;

            foreach (var point in wire.Path)
            {
                if (!point.Equals(endPoint))
                {
                    steps++;
                }
                else
                {
                    break;
                }
            }

            return steps;
        }

        public static int StepsToPointForWires(this IEnumerable<Wire> wires, Point endPoint)
        {
            var steps = 0;

            foreach (var wire in wires)
            {
                steps += wire.StepsToPoint(endPoint);
            }

            return steps;
        }

        public static Dictionary<Point, int> FindShortestWire(this IEnumerable<Wire> wires, IEnumerable<Point> intersections)
        {
            var stepsPerIntersection = new Dictionary<Point, int>();


            foreach (var point in intersections)
            {
                if (!stepsPerIntersection.ContainsKey(point))
                {
                    var steps = 0;
                    
                    foreach (var wire in wires)
                    {
                        steps += wire.StepsToPoint(point);
                    }
                    
                    stepsPerIntersection.Add(point, steps);
                }
            }

            return stepsPerIntersection;
        }
    }
}
