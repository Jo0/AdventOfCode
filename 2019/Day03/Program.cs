using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            var wires = new List<Wire>();

            foreach (var wireTrace in wireTraces)
            {
                var wire = new Wire();

                var traces = wireTrace.Split(',');

                foreach (var trace in traces)
                {
                    wire.Traces.Add(new Trace
                    {
                        Direction = (Direction)trace[0],
                        NumberOfSteps = Int32.Parse(trace.Substring(1))
                    });
                }

                wires.Add(wire);
            }


            
            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }

    public class Wire
    {
        public Wire()
        {
            Traces = new List<Trace>();
        }

        public List<Trace> Traces { get; set; }
    }

    public class Trace
    {
        public Direction Direction { get; set; }

        public int NumberOfSteps { get; set; }
    }

    public enum Direction
    {
        Up = 'U',
        Down = 'D',
        Left = 'L',
        Right = 'R'
    }
}
