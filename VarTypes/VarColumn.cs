//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Data;

namespace Flowish
{
    class VarColumn : IntVariable
    {
        public DataColumn setColumn = null;

        public void setValue(Object value)
        {
            setColumn = (DataColumn)value;
        }
        public Object getValue()
        {
            return setColumn;
        }
        public int getVariableType()
        {
            return 9;
        }
    }
}
