//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Collections;

namespace Flowish
{
    public class IntFunction
    {
        public string functionName = "";
        public ArrayList parameters = new ArrayList();
        public ArrayList compilation = null;

        public IntFunction(string FunctionName, ArrayList Parameters, ArrayList Compilation)
        {
            functionName = FunctionName;
            parameters = Parameters;
            compilation = Compilation;
        }

        public IntFunction(ArrayList Compilation, Boolean isLoop)
        {
            if (Compilation.Count>0)
            {
                ArrayList firstline = (ArrayList)Compilation[0];

                if (firstline != null)
                {
                    IntToken title = (IntToken)firstline[0];
                    functionName = title.text;
                    IntToken start = (IntToken)firstline[1];
                    int endofparms = -1;

                    if (start.tokentype == IntConstants.OPEN_PAREN)
                    {
                        // Incorrectly labeled but we know what it has to be now, so LET token = PARAMETER

                        start.tokentype = IntConstants.OPEN_PARAMETER;
                    }

                    for (int i = 2; i < firstline.Count; i++)
                    {
                        IntToken current = (IntToken)firstline[i];
                        if (current.tokentype == IntConstants.CLOSE_PARAMETER)
                        {
                            i = firstline.Count;
                        }
                        else
                        {
                            if (current.tokentype == IntConstants.CLOSE_PAREN)
                            {
                                current.tokentype = IntConstants.CLOSE_PARAMETER;
                                i = firstline.Count;
                            }
                        }
                    }

                    if (start.tokentype == IntConstants.OPEN_PARAMETER)
                    {
                        for (int i = 2; i < firstline.Count; i++)
                        {
                            IntToken current = (IntToken)firstline[i];
                            if (current.tokentype != IntConstants.CLOSE_PARAMETER)
                            {
                                parameters.Add(current);
                            }
                            else
                            {
                                endofparms = i;
                                i = firstline.Count;
                            }
                        }
                        if (endofparms > -1)
                        {
                            firstline.RemoveRange(0, endofparms + 1);
                        }
                        else
                        {
                            // ERROR:  MISSING CLOSE_PARAMETER
                        }
                    }
                    else
                    {
                        // ERROR:  function missing parameter
                    }
                    if (!isLoop)
                    {
                        Compilation.RemoveAt(0);  // Strip off empty reference line
                    }
                }
                else
                {
                    // ERROR:  Function missing header
                }
            }
            else
            {
                // ERROR: improper function declaration -- too short
            }

            compilation = Compilation;
        }

        public void updateParameters()
        {
            if (parameters != null)
            {
                ArrayList newList = parameters;
                parameters = new ArrayList();

                IntToken current;
                int index = 0;

                if (newList.Count > 0)
                {
                    current = (IntToken)newList[0];
                    while (current != null)
                    {
                        if ((current.tokentype == IntConstants.UNKNOWN) || (current.tokentype == -1))
                        {
                            if (index + 2 < newList.Count)
                            {
                                IntToken variable = (IntToken)newList[index];
                                IntToken as_ref = (IntToken)newList[index + 1];
                                IntToken value = (IntToken)newList[index + 2];

                                if (variable.tokentype == -1)
                                {
                                    switch (value.tokentype)
                                    {
                                        case IntConstants.RES_BYTE: variable.tokentype = IntConstants.BYTE; break;
                                        case IntConstants.RES_CHAR: variable.tokentype = IntConstants.CHAR; break;
                                        case IntConstants.RES_INTEGER: variable.tokentype = IntConstants.INTEGER; break;
                                        case IntConstants.RES_FLOAT: variable.tokentype = IntConstants.FLOAT; break;
                                        case IntConstants.RES_STRING: variable.tokentype = IntConstants.STRING; break;
                                        case IntConstants.RES_ARRAY: variable.tokentype = IntConstants.ARRAY; break;
                                        case IntConstants.RES_BOOLEAN: variable.tokentype = IntConstants.BOOLEAN; break;
                                    }
                                }
                                parameters.Add(variable);
                                index += 2;
                            }
                            else
                            {
                                functionName = current.text;
                            }
                        }

                        index++;
                        if (index < newList.Count)
                        {
                            current = (IntToken)newList[index];
                        }
                        else
                        {
                            current = null;
                        }
                    }

                    newList.Clear();
                }
            }
        }

        public void dumpFunction(ArrayList output)
        {
            string tmp;
            output.Add("");
            output.Add("Function: " + functionName);
            
            tmp = "Parameters: ";
            if (parameters!=null)
            {
                foreach (IntToken current in parameters)
                {
                    tmp += IntToken.info(current) + " : ";
                }
            }
            else
            {
                tmp += "(none)";
            }
            output.Add(tmp);
            output.Add("");

            tmp = "Code:";
            output.Add(tmp);

            tmp = "";

            if (compilation != null)
            {
                if (compilation.Count > 0)
                {
                    IntToken temp = new IntToken("",-1);

                    if (compilation[0].GetType() == temp.GetType())
                    {
                        foreach (IntToken current in compilation)
                        {
                            if (current.tokentype == IntConstants.ENDOFSTATEMENT)
                            {
                                output.Add(tmp);
                                tmp = "";
                            }
                            else
                            {
                                tmp += " " + current.text;
                            }
                        }
                    }
                    else
                    {
                        foreach (ArrayList line in compilation)
                        {
                            foreach (IntToken current in line)
                            {
                                if (current.tokentype == IntConstants.ENDOFSTATEMENT)
                                {
                                    output.Add(tmp);
                                    tmp = "";
                                }
                                else
                                {
                                    tmp += " " + current.text;
                                }
                            }
                        }
                    }
                }
            }
            if (tmp.Length > 0) output.Add(tmp);
            output.Add("");
        }

        public static IntFunction locateFunction(IntToken fun, ArrayList compilation)
        {
            if (fun.functionReference == null)
            {
                for (int i = 0; i < compilation.Count; i++)
                {
                    IntFunction current = (IntFunction)compilation[i];
                    if (fun.text == current.functionName)
                    {
                        fun.functionReference = current;
                        return current;
                    }
                }
            }
            else
            {
                return fun.functionReference;
            }
                
            return null;
        }

        public static IntFunction locateFunction(string fun, ArrayList compilation)
        {
            for (int i = 0; i < compilation.Count; i++)
            {
                IntFunction current = (IntFunction)compilation[i];
                if (fun == current.functionName)
                {
                    return current;
                }
            }
            return null;
        }
    }
}
