//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;

namespace Flowish
{
    public interface IntVariable 
    {
        void setValue(Object value);
        Object getValue();
        int getVariableType();
    }
}
