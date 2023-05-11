//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Collections;

namespace Flowish
{
    public class IntRuntime
    {
        public IntAssembly programAssembly = null;
        public ArrayList Output = null;
        public ArrayList loopStack = new ArrayList();               // Stack of IntLoop
        public ArrayList globalVariables = new ArrayList();         // List of IntTokens
        public ArrayList globalRegexVariables = new ArrayList();    // List of IntTokens
        public int lineNumber = -1;
        public string startFunction = "";
        public int overflowProtection = 0;

        // Database Components

        public ArrayList openDatabases = new ArrayList();           // All open databases

        // Additional Extensible Events

        public CallbackEventHandler OnDocument = null;

        //public static IntRuntime Clone(IntRuntime original)
        //{
        //    IntRuntime newRuntime = new IntRuntime();
        //    newRuntime.programAssembly = original.programAssembly;
        //    newRuntime.loopStack = original.loopStack;
        //    newRuntime.globalVariables = original.globalVariables;
        //    newRuntime.globalRegexVariables = new ArrayList();
        //    newRuntime.lineNumber = -1;
        //    newRuntime.startFunction = "";
        //    newRuntime.overflowProtection = 0;
        //    newRuntime.openDatabases = original.openDatabases;
        //    newRuntime.Output = new ArrayList();
            
        //    return newRuntime;
        //}
    }
}
