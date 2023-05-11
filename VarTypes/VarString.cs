//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;

namespace Flowish
{
    public class VarString : IntVariable
    {
        public String setString = "";

        public void setValue(Object value)
        {
            setString = (String)value;
        }
        public Object getValue()
        {
            return setString;
        }
        public int getVariableType()
        {
            return 1;
        }
    }
}
