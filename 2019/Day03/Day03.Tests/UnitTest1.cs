using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Day03.Tests
{
    public class UnitTest1
    {
        
        [Fact]
        public void Test1()
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

            var allPoints = wires.SelectMany(w => w.Path);
            var allPointsGrouped = allPoints.GroupBy(p => p);

            var intersections = allPointsGrouped.Where(p => p.Count() > 1).Select(p => p.Key);

            Assert.True(intersections.Count() > 1);
        }
    }
}
