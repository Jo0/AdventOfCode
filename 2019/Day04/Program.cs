using System;

namespace Day04
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var path = Path.Combine(AppContext.BaseDirectory, "input.txt");

            var ranges = List<Range>();

            using (var inputFile = File.OpenRead(path))
            using (var streamReader = new StreamReader(inputFile))
            {
                while (!streamReader.EndOfStream)
                {
                    var rangeLine = await streamReader.ReadLineAsync();

                    foreach (var range in rangeLine)
                    {
                        var ranges = range.Split('-').First();
                        var rangeMinimum = ranges[0];
                        var rangeMaximum = ranges[1];

                        ranges.Add(new Range(rangeMinimum, rangeMaximum));
                    }
                }
            }

            var passwords = new List<Password>();
            foreach (var range in ranges)
            {
                passwords.Add(new Password(range.Minimum, range.Maximum));
            }




        }
    }
    public class Password
    {
        public Password()
        {

        }

        public Password(int minimum, int maximum)
        {
            Range = new Range(minimum, maximum);
        }


        public Range Range { get; set; }
        public int Password
        {
            get
            {
                return FindPassword();
            }
        }

        private int FindPassword()
        {
            for (var i = this.Range.Minimum; i <= this.Range.Maximum; i++)
            {
                if (ValidatePasswor(i) == true)
                {
                    return int;
                }
            }

            return -1;
        }

        private bool ValidatePassword(int password)
        {
            if(password > this.Range.Minimum && password < this.Range.Maximum)
            {
                var passwordStr = password.ToString();
                var actuallySixDigit = passwordStr.Length == 6;

                if(actuallySixDigit){

                    var twoSameAdjcentDigits = false;
                    var alwaysIncrementingFromFirstDigit = false;

                    for(var i = 0; i < passwordStr.Length - 1; i++)
                    {

                        twoSameAdjcentDigits = passwordStr[i].Equals(passwordStr[i+1]);
                        alwaysIncrementingFromFirstDigit = passwordStr[i] < passwordStr[i+1];
                    }

                    if(twoSameAdjcentDigits && alwaysIncrementingFromFirstDigit)
                    {
                        return true;
                    }


                }
            }
            
            return false;
        }
            
            return false;
        }
    }

    public class Range
    {
        public long Minimum { get; set; }
        public long Maximum { get; set; }
    }
}
