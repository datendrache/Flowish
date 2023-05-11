//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System.Collections;

namespace Flowish
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
