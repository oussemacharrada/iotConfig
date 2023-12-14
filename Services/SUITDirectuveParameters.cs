using System;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class SUITDirectiveParameters
    {
        public int Weight { get; set; }
        public SUITParameters Parameters { get; set; }
        public List<string> DepParams { get; set; } 
        public List<SUITSequence> Sequence { get; set; }
        public SUITDirectiveParameters()
        {
            Weight = 0;
            Parameters = new SUITParameters();
            DepParams = new List<string>(); 
            Sequence = new List<SUITSequence>();
        }

    }
}