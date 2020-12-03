using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day04
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var path = Path.Combine(AppContext.BaseDirectory, "input.txt");

            var ranges = new List<Range>();

            using (var inputFile = File.OpenRead(path))
            using (var streamReader = new StreamReader(inputFile))
            {
                while (!streamReader.EndOfStream)
                {
                    var rangeLine = await streamReader.ReadLineAsync();

                    var splitRangeLine = rangeLine.Split('-');
                    var rangeMinimum = Int32.Parse(splitRangeLine[0]);
                    var rangeMaximum = Int32.Parse(splitRangeLine[1]);

                    ranges.Add(new Range(rangeMinimum, rangeMaximum));
                }
            }

            var passwordRanges = new List<PasswordFinder>();
            foreach (var range in ranges)
            {
                passwordRanges.Add(new PasswordFinder(range.Minimum, range.Maximum));
            }

            foreach (var range in passwordRanges)
            {
                Console.WriteLine($"Range {range.Range.Minimum} - {range.Range.Maximum} contains {range.Passwords.Count} different passwords");
            }


            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }

    public class PasswordFinder
    {
        private List<int> _passwords;

        public PasswordFinder()
        {

        }

        public PasswordFinder(int minimum, int maximum)
        {
            Range = new Range(minimum, maximum);
            _passwords = new List<int>();
            FindPasswords();
        }

        public Range Range { get; set; }

        public bool FoundPasswords
        {
            get
            {
                return this.Passwords.Count > 0;
            }
        }

        public List<int> Passwords
        {
            get
            {
                return _passwords;
            }
        }

        private void FindPasswords()
        {
            for (var i = this.Range.Minimum; i <= this.Range.Maximum; i++)
            {
                if (ValidatePassword(i) == true)
                {
                    _passwords.Add(i);
                }
            }
        }

        private bool ValidatePassword(int password)
        {
            var passwordStr = password.ToString();
            var digits = new Dictionary<char, List<int>>();

            var actuallySixDigit = passwordStr.Length == 6;
            var matchingDigitsAlwaysInGroupsOfTwo = false;
            var alwaysIncrementingFromFirstDigit = true;

            if (actuallySixDigit)
            {
                for (var i = 0; i < passwordStr.Length; i++)
                {
                    if(i != passwordStr.Length - 1)
                    {
                        alwaysIncrementingFromFirstDigit = passwordStr[i] <= passwordStr[i + 1];
                    }

                    if (digits.ContainsKey(passwordStr[i]))
                    {
                        digits[passwordStr[i]].Add(i);
                    }
                    else
                    {
                        digits.Add(passwordStr[i], new List<int>() { i });
                    }

                    if (!alwaysIncrementingFromFirstDigit)
                    {
                        break;
                    }
                }

                if (alwaysIncrementingFromFirstDigit)
                {
                    var digitWithEvenOccurances = digits.Where(d => d.Value.Count == 2); //change to >=2 for part 1

                    if(digitWithEvenOccurances.Any())
                    {
                        matchingDigitsAlwaysInGroupsOfTwo = digitWithEvenOccurances.All(d =>
                        {
                            var serial = true;

                            var positions = d.Value;

                            for (int i = 0; i < positions.Count - 1; i++)
                            {
                                serial = positions[i] < positions[i + 1];

                                if (!serial)
                                {
                                    break;
                                }
                            }

                            return serial;
                        });
                    }
                }
            }

            return actuallySixDigit && matchingDigitsAlwaysInGroupsOfTwo && alwaysIncrementingFromFirstDigit;
        }
    }

    public class Range
    {
        public Range()
        {
            Minimum = 0;
            Maximum = 0;
        }

        public Range(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public int Minimum { get; set; }
        public int Maximum { get; set; }
    }
}
