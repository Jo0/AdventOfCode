using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.IntCode
{
    public class Parameter
    {
        public Parameter(ParameterType type, int value)
        {
            Type = type;
            Value = value;
        }
        public ParameterType Type { get; set; }
        public int Value { get; set; }
    }

    public enum ParameterType
    {
        Position = 0,
        Immediate = 1
    }
}
