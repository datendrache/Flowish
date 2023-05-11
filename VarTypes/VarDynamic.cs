//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using FatumCore;

namespace Flowish
{
    public class VarTree : IntVariable
    {
        public Tree setTree = new Tree();

        public void setValue(Object value)
        {
            setTree = (Tree)value;
        }
        public Object getValue()
        {
            return setTree;
        }
        public int getVariableType()
        {
            return 12;
        }
    }
}
