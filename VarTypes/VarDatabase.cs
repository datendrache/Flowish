//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;

namespace Flowish
{
    class VarDatabase : IntVariable
    {
        public SQLiteDatabase setDatabase = null;

        public void setValue(Object value)
        {
            setDatabase = (SQLiteDatabase)value;
        }
        public Object getValue()
        {
            return setDatabase;
        }
        public int getVariableType()
        {
            return 8;
        }
    }
}
