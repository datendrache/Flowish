//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;

namespace Flowish
{
    class VarByte : IntVariable
    {
        public byte setBoolean = 0;

        public void setValue(Object value)
        {
            setBoolean = (byte)value;
        }
        public Object getValue()
        {
            return setBoolean;
        }
        public int getVariableType()
        {
            return 4;
        }
    }
}
