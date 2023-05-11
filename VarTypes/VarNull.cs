using System;

namespace Flowish
{
    class VarNull : IntVariable
    {
        public void setValue(Object value)
        {
            
        }
        public Object getValue()
        {
            return null;
        }
        public int getVariableType()
        {
            return 13;
        }
    }
}
