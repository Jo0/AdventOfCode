using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Day01
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var path = Path.Combine(AppContext.BaseDirectory, "input.txt");
            var inputFile = File.OpenRead(path);

            var moduleCount = 0;
            var totalFuelRequirement = 0;

            using (var streamReader = new StreamReader(inputFile))
            {
                do
                {
                    var line = await streamReader.ReadLineAsync();

                    if (Int32.TryParse(line, out var mass))
                    {
                        var module = new Module(mass, false);
                        moduleCount++;
                        totalFuelRequirement = module.FuelRequired;
                    }
                }
                while (!streamReader.EndOfStream);
            }

            Console.WriteLine();
            Console.WriteLine($"Number of Modules = {moduleCount}");
            Console.WriteLine($"Total fuel required = {totalFuelRequirement}");


            stopwatch.Stop();
            Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }

    public class Module
    {
        private bool _calculateFuelForFuel;

        public Module(int mass, bool calculateFuelForFuel = false)
        {
            Mass = mass;
            _calculateFuelForFuel = calculateFuelForFuel;
        }

        public int Mass { get; set; }

        public virtual int FuelRequired
        {
            get
            {
                var fuel = CalculateFuel(Mass);

                if(_calculateFuelForFuel)
                {
                    var fuelForFuel = new Fuel(fuel);
                    return fuel + fuelForFuel.FuelRequired;
                }
                else
                {
                    return fuel;
                }
            }
        }

        private int CalculateFuel(int mass)
        {
            return (int)(Math.Floor((double)(mass / 3)) - 2);
        }
    }

    public class Fuel : Module
    {
        public Fuel(int mass) : base(mass)
        {
        }

        public override int FuelRequired
        {
            get
            {
                return CalculateFuel(Mass);
            }
        }

        private int CalculateFuel(int mass)
        {
            var fuel = (int)(Math.Floor((double)(mass / 3)) - 2);

            if (fuel <= 0)
            {
                return 0;
            }
            else
            {
                return fuel + CalculateFuel(fuel);
            }
        }
    }
}
