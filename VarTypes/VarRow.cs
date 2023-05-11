//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Data;

namespace Flowish
{
    class VarRow : IntVariable
    {
        public DataRow setRow = null;

        public void setValue(Object value)
        {
            setRow = (DataRow)value;
        }
        public Object getValue()
        {
            return setRow;
        }
        public int getVariableType()
        {
            return 8;
        }
    }
}
