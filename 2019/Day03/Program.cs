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

            var intersections = wires.FindIntersections(centralPort);

            var pointDistances = intersections.FindDistances(centralPort);

            var closestPoint = pointDistances.Values.Min();

            Console.WriteLine($"Distance to closest intersection = {closestPoint}");

            var stepsToIntersections = wires.FindShortestWire(intersections);

            var shortestWire = stepsToIntersections.Values.Min();
            Console.WriteLine($"Shortest wire = {shortestWire}");

            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }
}
