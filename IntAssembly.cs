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

using System.Collections;

namespace Proliferation.Flowish
{
    public class IntAssembly
    {
        public ArrayList Libraries = new ArrayList();
        public ArrayList Functions = new ArrayList();

        public void dumpAssembly(ArrayList lines)
        {
            lines.Add("");
            lines.Add("------------------------------");
            lines.Add("Assembly Dump:");
            lines.Add("------------------------------");

            for (int i = 0; i < Functions.Count;i++)
            {
                IntFunction current = (IntFunction) Functions[i];
                current.dumpFunction(lines);
            }
        }

        public static void checkFunction(IntAssembly assembly, IntToken check)
        {
            for (int i = 0; i < assembly.Functions.Count; i++)
            {
                IntFunction current = (IntFunction)assembly.Functions[i];
                if (current.functionName == check.text)
                {
                    check.tokentype = IntConstants.FUNCTION;
                    check.functionReference = current;
                    i = assembly.Functions.Count;
                }
            }
        }

        public static void assignFunctions(IntAssembly assembly)
        {
            foreach (IntFunction current in assembly.Functions)
            {
                foreach (ArrayList line in current.compilation)
                {
                    foreach (IntToken token in line)
                    {
                        if (token.text != null)
                        {
                            checkFunction(assembly, token);
                        }
                    }
                }
            }
        }
    }
}
