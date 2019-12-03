using Day02.IntCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day02
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var path = Path.Combine(AppContext.BaseDirectory, "input.txt");
            var inputFile = File.OpenRead(path);

            int[] intCode = { };

            using (var streamReader = new StreamReader(inputFile))
            {
                var intCodeFromFile = await streamReader.ReadToEndAsync();

                intCode = intCodeFromFile.Split(',').Select(s => Int32.Parse(s)).ToArray();
            }

            var intCodeCopy = new int[intCode.Length];
            intCode.CopyTo(intCodeCopy, 0);

            var restorePositions = new Dictionary<int, int>()
            {
                {1, 12},
                {2, 2}
            };

            intCodeCopy.RestoreIntCode(restorePositions);

            intCodeCopy.ProcessIntCode();

            Console.WriteLine(intCodeCopy[0]);

            var valuePair = intCode.FindNounVerb(19690720);

            if(valuePair != null)
            {
                Console.WriteLine($"Noun = {valuePair.Noun} Verb = {valuePair.Verb}");

                Console.WriteLine(100 * valuePair.Noun + valuePair.Verb);
            }

            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }
}
