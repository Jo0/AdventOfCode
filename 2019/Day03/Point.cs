using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day03
{
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
            var compare = obj as Point;
            return (this.X == compare.X) && (this.Y == compare.Y);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }


    public class PointComparer : IEqualityComparer<Point>
    {
        public bool Equals(Point a, Point b)
        {
            return (a.X == b.X) && (a.Y == b.Y);
        }

        public int GetHashCode(Point point)
        {
            return point.X.GetHashCode()
                ^ point.Y.GetHashCode();
        }
    }

    public static class PointExtensions
    {
        public static List<Point> TracePath(this Point currentPoint, Trace trace)
        {
            var tracePath = new List<Point>();

            if (trace.Direction == Direction.Up)
            {
                for (int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X, currentPoint.Y + i));
                }
            }
            else if (trace.Direction == Direction.Down)
            {
                for (int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X, currentPoint.Y - i));
                }
            }
            else if (trace.Direction == Direction.Left)
            {
                for (int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X - i, currentPoint.Y));
                }
            }
            else if (trace.Direction == Direction.Right)
            {
                for (int i = 1; i <= trace.NumberOfSteps; i++)
                {
                    tracePath.Add(new Point(currentPoint.X + i, currentPoint.Y));
                }
            }
            else
            {
                throw new Exception("Unknown Direction");
            }

            return tracePath;
        }

        public static List<Point> FindIntersections(this IEnumerable<List<Point>> points, Point startingPoint)
        {
            var intersections = points.SelectMany(p => p)
                                      .GroupBy(p => p, new PointComparer())
                                      .Where(p => p.Count() > 1 && !p.First().Equals(startingPoint))
                                      .Select(p => p.Key);

            return intersections.ToList();
        }
    }
}
