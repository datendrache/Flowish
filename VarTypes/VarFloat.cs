//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;

namespace Flowish
{
    public class VarFloat : IntVariable
    {
        public float setFloat = 0;

        public void setValue(Object value)
        {
            setFloat = (float)value;
        }
        public Object getValue()
        {
            return setFloat;
        }
        public int getVariableType()
        {
            return 3;
        }
    }
}
