//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;


namespace Flowish
{
    public class VarBoolean : IntVariable
    {
        public Boolean setBoolean = false;

        public void setValue(Object value)
        {
            setBoolean = (Boolean)value;
        }
        public Object getValue()
        {
            return setBoolean;
        }
        public int getVariableType()
        {
            return 6;
        }
    }
}
