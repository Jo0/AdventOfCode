using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.IntCode
{
    public static class IntCodeOperations
    {
        public static void RestoreIntCode(this int[] intCode, Dictionary<int, int> restorePositions)
        {
            foreach (var position in restorePositions.Keys)
            {
                intCode[position] = restorePositions[position];
            }
        }

        public static void ProcessIntCode(this int[] intCode)
        {
            int position = 0;
            while (position < intCode.Length)
            {
                var operation = intCode.GetOperation(position, out var positionIncrement);
                position += positionIncrement;

                if (operation.OpCode == 99)
                    break;

                intCode.ProcessOpCode(operation);
            }
        }

        public static void ProcessIntCode(this int[] intCode, int? overrideNoun, int? overrideVerb)
        {
            if(overrideNoun.HasValue && overrideVerb.HasValue)
            {
                intCode[1] = overrideNoun.Value;
                intCode[2] = overrideVerb.Value;
            }

            int position = 0;
            while (position < intCode.Length)
            {
                var operation = intCode.GetOperation(position, out var positionIncrement);
                position += positionIncrement;

                if (operation.OpCode == 99)
                    break;

                intCode.ProcessOpCode(operation);
            }
        }

        
        public static Operation GetOperation(this int[] intCode, int position, out int positionIncrement)
        {
            var opCode = intCode[position];
            var parameters = new List<Parameter>();
            switch (opCode)
            {
                case (1):
                case (2):
                    positionIncrement = 4;

                    parameters = new List<Parameter>()
                    { 
                        new Parameter(ParameterType.Position, intCode[position + 1]),
                        new Parameter(ParameterType.Position, intCode[position + 2]),
                        new Parameter(ParameterType.Position, intCode[position + 3]),
                    };

                    return new Operation(opCode,parameters);
                case (3):
                    positionIncrement = 1;
                    
                    parameters = new List<Parameter>()
                    {
                        new Parameter(ParameterType.Position, intCode[position + 1]),
                    };

                    return new Operation(opCode, parameters);
                case (4):
                    positionIncrement = 1;

                    parameters = new List<Parameter>()
                    {
                        new Parameter(ParameterType.Position, intCode[position + 1]),
                    };

                    return new Operation(opCode, parameters);
                case (99):
                    positionIncrement = 0;
                    return new Operation(opCode);
                default:
                    throw new ArgumentException("Invalid OpCode");
            }
        }

        public static Position GetPosition(this int[] intCode, int location)
        {
            return new Position
            {
                Location = intCode[location],
                Value = intCode[intCode[location]]
            };
        }

        public static void ProcessOpCode(this int[] intCode, Operation operation)
        {
            switch (operation.OpCode)
            {
                case (1):
                    intCode[operation.Parameters[2].Value] = intCode[operation.Parameters[0].Value] + intCode[operation.Parameters[1].Value];
                    break;
                case (2):
                    intCode[operation.Parameters[2].Value] = intCode[operation.Parameters[0].Value] * intCode[operation.Parameters[1].Value];
                    break;
                default:
                    break;
            }
        }

        public static ValuePair FindNounVerb(this int[] intCode, int expected)
        {
            int[] intCodeCopy = new int[intCode.Length];

            for(int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    intCode.CopyTo(intCodeCopy, 0);

                    intCodeCopy.ProcessIntCode(x, y);

                    if (intCodeCopy[0] == expected)
                    {
                        return new ValuePair
                        {
                            Noun = x,
                            Verb = y
                        };
                    }
                }
            }

            return default;
        }

    }

}
