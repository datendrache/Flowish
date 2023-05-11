//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Data;

namespace Flowish
{
    class VarTable : IntVariable
    {
        public DataTable setDataTable = new DataTable();

        public void setValue(Object value)
        {
            setDataTable = (DataTable)value;
        }
        public Object getValue()
        {
            return setDataTable;
        }
        public int getVariableType()
        {
            return 7;
        }
    }
}
