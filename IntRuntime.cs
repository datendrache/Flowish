//   Flowish - An interpreted language used for Data-In-Motion
//
//   Copyright (C) 2003-2023 Eric Knight
//   This software is distributed under the GNU Public v3 License
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.

//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.

//   You should have received a copy of the GNU General Public License

using System;
using System.Collections;

namespace Proliferation.Flowish
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
