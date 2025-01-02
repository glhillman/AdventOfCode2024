using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Day24
{
    internal class Gate
    {
        public Gate(string in1, string in2, string outName)
        {
            In1 = in1;
            Alias1 = in1; 
            In2 = in2;
            Alias2 = in2;
            OutName = outName;
            AliasOut = outName;
            In1Value = null;
            In2Value = null;
            _outValue = null;
        }
        public string In1 { get; set; }
        public string Alias1 { get; set; }
        public int? In1Value { get; set; }
        public string In2 { get; set; }
        public string Alias2 { get; set; }
        public int? In2Value { get; set; }
        public string OutName { get; set; }
        public string AliasOut { get; set; }
        protected int? _outValue;
        public virtual int? OutValue
        {
            get { return _outValue; }
        }
        public override string ToString()
        {
            return string.Format("in1 {0}:{1}, in2 {2}:{3}, value {4}:{5}", Alias1, In1Value.HasValue ? In1Value.Value : "null",
                                                                            Alias2, In2Value.HasValue ? In2Value.Value : "null",
                                                                            AliasOut, OutValue.HasValue ? OutValue.Value : "null");
        }
    }


    internal class AND : Gate
    {
        public AND(string in1, string in2, string value)
            : base(in1, in2, value)
        { }
        
        public override int? OutValue
        {
            get
            {
                if (_outValue == null)
                {
                    if (base.In1Value.HasValue && base.In2Value.HasValue)
                    {
                        _outValue = base.In1Value & base.In2Value;
                    }
                }
                return _outValue;
            }
        }
        public override string ToString()
        {
            return string.Format("AND {0}", base.ToString());
        }
    }

    internal class OR : Gate
    {
        public OR(string in1, string in2, string value)
            : base(in1, in2, value)
        { }
        public override int? OutValue
        {
            get
            {
                if (_outValue == null)
                {
                    if (base.In1Value.HasValue && base.In2Value.HasValue)
                    {
                        _outValue = base.In1Value | base.In2Value;
                    }
                }
                return _outValue;
            }
        }
        public override string ToString()
        {
            return string.Format("OR {0}", base.ToString());
        }
    }

    internal class XOR : Gate
    {
        public XOR(string in1, string in2, string value)
            : base(in1, in2, value)
        { }
        public override int? OutValue
        {
            get
            {
                if (_outValue == null)
                {
                    if (base.In1Value.HasValue && base.In2Value.HasValue)
                    {
                        _outValue = base.In1Value ^ base.In2Value;
                    }
                }
                return _outValue;
            }
        }
        public override string ToString()
        {
            return string.Format("XOR {0}", base.ToString());
        }
    }
}


