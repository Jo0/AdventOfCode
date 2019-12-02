using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            string[] intCode = { };

            using (var streamReader = new StreamReader(inputFile))
            {
                var intCodeFromFile = await streamReader.ReadToEndAsync();

                intCode = intCodeFromFile.Split(',');
            }

            var restorePositions = new Dictionary<int, int>()
            {
                {1, 12},
                {2, 2}
            };

            intCode.RestoreIntCode(restorePositions);

            //intCode.ProcessIntCode();

            //Console.WriteLine(intCode[0]);

            var valuePair = intCode.FindValuePair(19690720);

            Console.WriteLine($"noun={valuePair.Noun} verb={valuePair.Verb}");

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

        public static void RestoreIntCode(this string[] intCode, Dictionary<int, int> restorePositions)
        {
            foreach(var position in restorePositions.Keys)
            {
                intCode[position] = restorePositions[position].ToString();
            }
        }

        public static void ProcessIntCode(this string[] intCode)
        {

            for (var i = 0; i < intCode.Length; i += 4)
            {
                Int32.TryParse(intCode[i], out var opCode);

                if(opCode == 99)
                {
                    break;
                }

                Int32.TryParse(intCode[i + 1], out var nounPos);
                Int32.TryParse(intCode[i + 2], out var verbPos);
                Int32.TryParse(intCode[i + 3], out var overridePos);

                var processedValue = intCode.ProcessOpCode(opCode, nounPos, verbPos, overridePos);

                if (processedValue == Int32.MinValue)
                {
                    break;
                }
            }
        }

        public static ValuePair FindValuePair(this string[] intCode, int valueToFind)
        {
            for (var i = 0; i < intCode.Length; i += 4)
            {
                Int32.TryParse(intCode[i], out var opCode);

                if (opCode == 99)
                {
                    break;
                }

                Int32.TryParse(intCode[i + 1], out var nounPos);
                Int32.TryParse(intCode[i + 2], out var verbPos);
                Int32.TryParse(intCode[i + 3], out var overridePos);

                var processedValue = intCode.ProcessOpCode(opCode, nounPos, verbPos, overridePos);

                if (processedValue == valueToFind)
                {
                    return new ValuePair
                    {
                        Noun = Int32.Parse(intCode[nounPos]),
                        Verb = Int32.Parse(intCode[verbPos])
                    };
                }
            }

            return new ValuePair();
        }

        public static int ProcessOpCode(this string[] intCode, int opCode, int nounPos, int verbPos, int overridePos)
        {
            var noun = Int32.Parse(intCode[nounPos]);
            var verb = Int32.Parse(intCode[verbPos]);
            int opVal = 0;

            switch (opCode)
            {
                case (1):
                    opVal = noun + verb;
                    intCode[overridePos] = opVal.ToString();
                    break;
                case (2):
                    opVal = noun * verb;
                    intCode[overridePos] = opVal.ToString();
                    break;
                default:
                    break;
            }

            return opVal;
        }
    }
}
