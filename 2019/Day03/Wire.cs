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
}
