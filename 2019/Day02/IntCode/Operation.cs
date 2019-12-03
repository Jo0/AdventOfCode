using System;
using System.Collections.Generic;
using System.Text;

namespace Day02.IntCode
{
    public class Operation
    {
        public Operation()
        {
            OpCode = 0;
            Noun = default;
            Verb = default;
            Override = default;
        }

        public Operation(int opCode, Position noun, Position verb, Position @override)
        {
            OpCode = opCode;
            Noun = noun;
            Verb = verb;
            Override = @override;
        }

        public Operation(int opCode)
        {
            OpCode = opCode;
            Noun = default;
            Verb = default;
            Override = default;
        }

        public int OpCode { get; set; }
        public Position Noun { get; set; }
        public Position Verb { get; set; }
        public Position Override { get; set; }

        public int Output
        {
            get
            {
                return Override.Value;
            }
        }
    }
}
