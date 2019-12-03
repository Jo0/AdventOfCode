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

            //var restorePositions = new Dictionary<int, int>()
            //{
            //    {1, 12},
            //    {2, 2}
            //};

            //intCode.RestoreIntCode(restorePositions);

            intCode.ProcessIntCode();

            Console.WriteLine(intCode[0]);

            //var valuePair = intCode.FindValuePair(19690720);

            //Console.WriteLine($"noun={valuePair.Noun} verb={valuePair.Verb}");

            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }


    public class ValuePair
    {
        public int Noun { get; set; }
        public int Verb { get; set; }
    }

    public static class IntCodeRunner
    {

        public static void RestoreIntCode(this int[] intCode, Dictionary<int, int> restorePositions)
        {
            foreach(var position in restorePositions.Keys)
            {
                intCode[position] = restorePositions[position];
            }
        }

        public static int ProcessIntCode(this int[] intCode, int? positionToGetValue = null)
        {

            for (var i = 0; i < intCode.Length; i += 4)
            {
                var opCode = intCode[i];

                if(opCode == 99)
                {
                    if (positionToGetValue.HasValue)
                        return intCode[positionToGetValue.GetValueOrDefault()];
                    else
                        break;

                }

                var nounPos = intCode[i + 1];
                var verbPos = intCode[i + 2];
                var overridePos = intCode[i + 3];

                intCode.ProcessOpCode(opCode, nounPos, verbPos, overridePos);
            }

            return 0;
        }

        public static ValuePair FindValuePair(this int[] intCode, int valueToFind)
        {
            for (var i = 0; i < intCode.Length; i += 4)
            {
                var intCodeCopy = intCode;
                
                if(!intCodeCopy.Equals(intCode))
                {
                    break;
                }

                var opCode = intCodeCopy[i];

                if (opCode == 99)
                {
                        break;
                }

                var nounPos = intCodeCopy[i + 1];
                var verbPos = intCodeCopy[i + 2];
                var overridePos = intCodeCopy[i + 3];

                var processedValue = intCodeCopy.ProcessOpCode(opCode, nounPos, verbPos, overridePos);

                if(processedValue == valueToFind)
                {
                    return new ValuePair
                    {
                        Noun = intCodeCopy[nounPos],
                        Verb = intCodeCopy[verbPos]
                    };
                }
            }

            return default;
        }

        public static int ProcessOpCode(this int[] intCode, int opCode, int nounPos, int verbPos, int overridePos)
        {
            var noun = intCode[nounPos];
            var verb = intCode[verbPos];
            int opVal = 0;

            switch (opCode)
            {
                case (1):
                    opVal = noun + verb;
                    intCode[overridePos] = opVal;
                    break;
                case (2):
                    opVal = noun * verb;
                    intCode[overridePos] = opVal;
                    break;
                default:
                    break;
            }

            return opVal;
        }
    }
}
