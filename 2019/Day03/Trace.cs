using System;
using System.Collections.Generic;
using System.Text;

namespace Day03
{
    public enum Direction
    {
        Up = 'U',
        Down = 'D',
        Left = 'L',
        Right = 'R'
    }

    public class Trace
    {
        public Direction Direction { get; set; }

        public int NumberOfSteps { get; set; }
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
}
