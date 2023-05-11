//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.IO;


namespace Flowish
{
    class VarFile : IntVariable
    {
        Stream infile = null;

        public void setValue(Object value)
        {
            infile = (Stream)value;
        }
        public Object getValue()
        {
            return infile;
        }
        public int getVariableType()
        {
            return 10;
        }
    }
}
