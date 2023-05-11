//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;

namespace Flowish
{
    class VarChar : IntVariable
    {
        public Char setChar = '\0';

        public void setValue(Object value)
        {
            setChar = (Char)value;
        }
        public Object getValue()
        {
            return setChar;
        }
        public int getVariableType()
        {
            return 5;
        }
    }
}
