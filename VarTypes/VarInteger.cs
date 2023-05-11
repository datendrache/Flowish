//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;

namespace Flowish
{
    public class VarInteger : IntVariable
    {
        public long setInteger = 0;

        public void setValue(Object value)
        {
            setInteger = (long)value;
        }
        public Object getValue()
        {
            return setInteger;
        }
        public int getVariableType()
        {
            return 2;
        }
    }
}
