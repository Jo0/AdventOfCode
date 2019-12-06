using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day03
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var path = Path.Combine(AppContext.BaseDirectory, "input.txt");

            var wireTraces = new List<string>();

            using (var inputFile = File.OpenRead(path))
            using (var streamReader = new StreamReader(inputFile))
            {
                while (!streamReader.EndOfStream)
                {
                    wireTraces.Add(await streamReader.ReadLineAsync());
                }
            }

            var centralPort = new Point(0, 0);
            var wires = new List<Wire>();

            foreach (var wireTrace in wireTraces)
            {

                var traces = wireTrace.GetTraces();

                wires.Add(new Wire(centralPort, traces));
            }

            var intersections = wires.SelectMany(w => w.Path).GroupBy(p => p).Where(p => p.Count() > 1).Select(p => p.Key);

            foreach(var intersection in intersections)
            {
                Console.WriteLine($"X = {intersection.X}, Y = {intersection.Y}");
            }

            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }

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
            foreach(var trace in Traces)
            {
                var endOfPath = path.Last();

                var tracePath = endOfPath.TracePath(trace);

                path.AddRange(tracePath);
            }

            return path;
        }
    }

    public class Trace
    {
        public Direction Direction { get; set; }

        public int NumberOfSteps { get; set; }
    }

    public class Point
    {
        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var compareObj = (Point)obj;
            return this.X == compareObj.X && this.Y == compareObj.Y;
        }
    }

    public static class PointExtensions
    {
        public static List<Point> TracePath(this Point currentPoint, Trace trace)
        {
            var tracePath = new List<Point>();

            if(trace.Direction == Direction.Up)
            {
                for(int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X + i, currentPoint.Y));
                }
            }
            else if(trace.Direction == Direction.Down)
            {
                for (int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X - i, currentPoint.Y));
                }
            }
            else if (trace.Direction == Direction.Left)
            {
                for (int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X, currentPoint.Y - i));
                }
            }
            else if (trace.Direction == Direction.Right)
            {
                for (int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X, currentPoint.Y + i));
                }
            }
            else
            {
                throw new Exception("Unknown Direction");
            }

            return tracePath;
        }
    }

    public static class TraceExtensions
    {
        public static List<Trace> GetTraces(this string input)
        {
            var traces = new List<Trace>();

            var steps = input.Split(',');
            
            foreach (var step in steps)
            {
                traces.Add(new Trace
                {
                    Direction = (Direction)step[0],
                    NumberOfSteps = Int32.Parse(step.Substring(1))
                });
            }

            return traces;
        }
    }

    public enum Direction
    {
        Up = 'U',
        Down = 'D',
        Left = 'L',
        Right = 'R'
    }
}
