using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.IntCode
{
    public class Operation
    {
        public Operation()
        {
            OpCode = 0;
            Parameters = new List<Parameter>();
        }

        public Operation(int opCode)
        {
            OpCode = opCode;
            Parameters = new List<Parameter>();
        }

        public Operation(int opCode, List<Parameter> parameters)
        {
            OpCode = opCode;
            Parameters = parameters;
        }

        public int OpCode { get; set; }

        public List<Parameter> Parameters {get; set;}
    }
}
