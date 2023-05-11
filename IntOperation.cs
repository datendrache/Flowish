//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Collections;

namespace Flowish
{
    public class IntOperation
    {
        private static object Pop(ArrayList stack)
        {
            Object current = null;
            if (stack.Count > 0)
            {
                current = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
            }
            return current;
        }

        private static void Push(ArrayList stack, Object item)
        {
            stack.Add(item);
        }

        public static IntError resolveArray(IntRuntime runtime, ArrayList varStack, ArrayList parameters, IntToken arrayVal, ArrayList opStack)
        {
            IntToken firstValue = (IntToken)Pop(opStack);
            IntError errorCode = null;

            if (firstValue != null)
            {
                if (firstValue.tokentype == IntConstants.INTEGER || firstValue.tokentype == IntConstants.BYTE)
                {
                    Boolean tokenHandled = false;

                    if (!tokenHandled)
                    {
                        for (int i = 0; i < varStack.Count; i++)
                        {
                            IntToken currentVar = (IntToken)varStack[i];
                            if (arrayVal.text == currentVar.text)
                            {
                                tokenHandled = true;
                                i = varStack.Count;
                                arrayVal.array = currentVar.array;
                                arrayVal.arraytype = currentVar.arraytype;
                            }
                        }

                        if (!tokenHandled)  // Check global space
                        {
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                IntToken currentVar = (IntToken)parameters[i];
                                if (arrayVal.text == currentVar.text)
                                {
                                    tokenHandled = true;
                                    i = parameters.Count;
                                    arrayVal.array = currentVar.array;
                                    arrayVal.arraytype = currentVar.arraytype;
                                }
                            }

                            if (!tokenHandled)  // Check global space
                            {
                                for (int i = 0; i < runtime.globalVariables.Count; i++)
                                {
                                    IntToken currentVar = (IntToken)runtime.globalVariables[i];
                                    if (arrayVal.text == currentVar.text)
                                    {
                                        tokenHandled = true;
                                        i = runtime.globalVariables.Count;
                                        arrayVal.array = currentVar.array;
                                        arrayVal.arraytype = currentVar.arraytype;
                                    }
                                }

                                if (!tokenHandled)  // Check global space
                                {
                                    for (int i = 0; i < runtime.globalRegexVariables.Count; i++)
                                    {
                                        IntToken currentVar = (IntToken)runtime.globalRegexVariables[i];
                                        if (arrayVal.text == currentVar.text)
                                        {
                                            tokenHandled = true;
                                            i = runtime.globalRegexVariables.Count;
                                            arrayVal.array = currentVar.array;
                                            arrayVal.arraytype = currentVar.arraytype;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (arrayVal.array != null)
                    {
                        int index;

                        if (firstValue.tokentype == IntConstants.INTEGER)
                        {
                            index = (int)firstValue.variableReference.getValue();
                        }
                        else
                        {
                            index = (int)((byte)firstValue.variableReference.getValue());
                        }

                        if (index >= 0)
                        {
                            if (arrayVal.array.Count >= index + 1)
                            {
                                IntToken arrayToken = (IntToken)arrayVal.array[index];
                                Push(opStack, arrayToken);
                            }
                            else
                            {
                                for (int i = arrayVal.array.Count; i < index+1; i++)
                                {
                                    switch (arrayVal.arraytype)
                                    {
                                        case 1:
                                            {
                                                IntToken arrayToken = new IntToken("", IntConstants.INTEGER);
                                                arrayToken.variableReference = new VarInteger();
                                                arrayVal.array.Add(arrayToken);
                                            }
                                            break;
                                        case 2:
                                            {
                                                IntToken arrayToken = new IntToken("", IntConstants.STRING);
                                                arrayToken.variableReference = new VarString();
                                                arrayVal.array.Add(arrayToken);
                                            }
                                            break;
                                        case 3:
                                            {
                                                IntToken arrayToken = new IntToken("", IntConstants.FLOAT);
                                                arrayToken.variableReference = new VarFloat();
                                                arrayVal.array.Add(arrayToken); 
                                            }
                                            break;
                                        case 4:
                                            {
                                                IntToken arrayToken = new IntToken("", IntConstants.BYTE);
                                                arrayToken.variableReference = new VarByte();
                                                arrayVal.array.Add(arrayToken);
                                            }
                                            break;
                                        case 5:
                                            {
                                                IntToken arrayToken = new IntToken("", IntConstants.CHAR);
                                                arrayToken.variableReference = new VarChar();
                                                arrayVal.array.Add(arrayToken);
                                            }
                                            break;
                                        case 6:
                                            {
                                                IntToken arrayToken = new IntToken("", IntConstants.BOOLEAN);
                                                arrayToken.variableReference = new VarBoolean();
                                                arrayVal.array.Add(arrayToken);
                                            }
                                            break;
                                    }
                                }

                                IntToken arrToken = (IntToken)arrayVal.array[index];
                                Push(opStack, arrToken);
                            }
                        }
                        else
                        {
                            errorCode = new IntError(arrayVal, 9);
                        }
                    }
                    else
                    {
                        errorCode = new IntError(arrayVal, 7);
                    }
                }
                else
                {
                    errorCode = new IntError(arrayVal, 8);
                }
            }
            else
            {
                errorCode = new IntError(null, 6);
            }

            return errorCode;
        }

        public static IntError resolveString(IntRuntime runtime, IntToken strVal, ArrayList opStack)
        {
            IntToken firstValue = (IntToken)Pop(opStack);
            IntError errorCode = null;

            if (firstValue.tokentype == IntConstants.INTEGER || firstValue.tokentype == IntConstants.BYTE)
            {
                int index;
                if (firstValue.tokentype == IntConstants.INTEGER)
                {
                    index = (int)firstValue.variableReference.getValue();
                }
                else
                {
                    index = (byte)firstValue.variableReference.getValue();
                }

                if (index>-1)
                {
                    string strval = (string) strVal.variableReference.getValue();
                    if (index < strval.Length)
                    {
                        IntToken newToken = new IntToken("", IntConstants.CHAR);
                        newToken.variableReference = new VarChar();
                        newToken.variableReference.setValue(strval[index]);
                        Push(opStack, newToken);
                    }
                    else
                    {
                        errorCode = new IntError(null, 9);
                    }
                }
                else
                {
                    errorCode = new IntError(null, 9);
                }
            }
            else
            {
                errorCode = new IntError(null, 6);
            }

            return errorCode;
        }

        public static IntError operationAssignment(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);

            //IntToken firstValue = (IntToken)Pop(opStack);
            //IntToken secondValue = (IntToken)Pop(opStack);

            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                firstValue.variableReference.setValue(secondValue.variableReference.getValue());
                            }
                            break;
                        case 2:
                            {
                                firstValue.variableReference.setValue(((long)secondValue.variableReference.getValue()).ToString());
                            }
                            break;
                        case 3:
                            {
                                firstValue.variableReference.setValue(((float)secondValue.variableReference.getValue()).ToString());
                            }
                            break;
                        case 4:
                            {
                                firstValue.variableReference.setValue(((byte)secondValue.variableReference.getValue()).ToString());
                            }
                            break;
                        case 5:
                            {
                                firstValue.variableReference.setValue(((char)secondValue.variableReference.getValue()).ToString());
                            }
                            break;
                        case 6:
                            {
                                firstValue.variableReference.setValue(((Boolean)secondValue.variableReference.getValue()).ToString());
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                long value;
                                long.TryParse((string)secondValue.variableReference.getValue(), out value);
                                firstValue.variableReference.setValue(value);
                            }
                            break;
                        case 2:
                            {
                                firstValue.variableReference.setValue((long)secondValue.variableReference.getValue());
                            }
                            break;
                        case 3:
                            {
                                firstValue.variableReference.setValue((long)((float)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 4:
                            {
                                firstValue.variableReference.setValue((long)((byte)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 5:
                            {
                                firstValue.variableReference.setValue(Convert.ToInt64((char)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 6:
                            {
                                Boolean value;
                                value = (Boolean)secondValue.variableReference.getValue();
                                if (value)
                                {
                                    firstValue.variableReference.setValue(1);
                                }
                                else
                                {
                                    firstValue.variableReference.setValue(0);
                                }
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:  // string
                            {
                                float value;
                                float.TryParse((string)secondValue.variableReference.getValue(), out value);
                                firstValue.variableReference.setValue(value);
                            }
                            break;
                        case 2: // integer
                            {
                                firstValue.variableReference.setValue((float)((int)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 3: // float
                            {
                                firstValue.variableReference.setValue((float)secondValue.variableReference.getValue());
                            }
                            break;
                        case 4: // byte
                            {
                                firstValue.variableReference.setValue((float)((byte)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 5:  // char
                            {
                                errorCode = new IntError(firstValue, 3);
                            };
                            break;
                        case 6:  // boolean
                            {
                                Boolean value;
                                value = (Boolean)secondValue.variableReference.getValue();
                                if (value)
                                {
                                    firstValue.variableReference.setValue(1.0);
                                }
                                else
                                {
                                    firstValue.variableReference.setValue(0.0);
                                }
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                byte value;
                                byte.TryParse((string)secondValue.variableReference.getValue(), out value);
                                firstValue.variableReference.setValue(value);
                            }
                            break;
                        case 2:
                            {
                                firstValue.variableReference.setValue((byte)((int)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 3:
                            {
                                firstValue.variableReference.setValue((byte)((float)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 4:
                            {
                                firstValue.variableReference.setValue((byte)secondValue.variableReference.getValue());
                            }
                            break;
                        case 5:
                            {
                                firstValue.variableReference.setValue(Convert.ToByte((char)secondValue.variableReference.getValue()));
                            }
                            break;
                        case 6:
                            {
                                Boolean value;
                                value = (Boolean)secondValue.variableReference.getValue();
                                if (value)
                                {
                                    secondValue.variableReference.setValue(1);
                                }
                                else
                                {
                                    firstValue.variableReference.setValue(0);
                                }
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                char second = (char)secondValue.variableReference.getValue();
                                string first = (string)firstValue.variableReference.getValue();

                                firstValue.variableReference.setValue(second.ToString());
                            }
                            break;
                        case 2:
                            {
                                long value = (long)secondValue.variableReference.getValue();
                                firstValue.variableReference.setValue(Convert.ToChar(value));
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte value = (byte)secondValue.variableReference.getValue();
                                firstValue.variableReference.setValue(Convert.ToChar(value));
                            }
                            break;
                        case 5:
                            {
                                char value = (char)secondValue.variableReference.getValue();
                                firstValue.variableReference.setValue(value);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                string value = (string)secondValue.variableReference.getValue();
                                if (value.ToLower() == "true")
                                {
                                    firstValue.variableReference.setValue(true);
                                }
                                else
                                {
                                    firstValue.variableReference.setValue(false);
                                }
                            }
                            break;
                        case 2:
                            {
                                long value = (long)secondValue.variableReference.getValue();
                                if (value > 0)
                                {
                                    firstValue.variableReference.setValue(true);
                                }
                                else
                                {
                                    firstValue.variableReference.setValue(false);
                                }
                            }
                            break;
                        case 3:
                            {
                                float value = (float)secondValue.variableReference.getValue();
                                if (value > 0)
                                {
                                    firstValue.variableReference.setValue(true);
                                }
                                else
                                {
                                    firstValue.variableReference.setValue(false);
                                }
                            }
                            break;
                        case 4:
                            {
                                byte value = (byte)secondValue.variableReference.getValue();
                                if (value > 0)
                                {
                                    firstValue.variableReference.setValue(true);
                                }
                                else
                                {
                                    firstValue.variableReference.setValue(false);
                                }
                            }
                            break;
                        case 5:
                            {
                                char value = (char)secondValue.variableReference.getValue();
                                Boolean result = false;
                                if (value != '\0')
                                {
                                    result = true;
                                }
                                firstValue.variableReference.setValue(result);
                            }
                            break;
                        case 6:
                            {
                                Boolean value;
                                value = (Boolean)secondValue.variableReference.getValue();
                                firstValue.variableReference.setValue(value);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 7) // VarTable
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 7:
                            {
                                firstValue.variableReference.setValue(secondValue.variableReference.getValue());
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 8) // VarRow
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 8:
                            {
                                firstValue.variableReference.setValue(secondValue.variableReference.getValue());
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 9) // VarColumn
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 9:
                            {
                                firstValue.variableReference.setValue(secondValue.variableReference.getValue());
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 10) // VarFile
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 10:
                            {
                                firstValue.variableReference.setValue(secondValue.variableReference.getValue());
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 11) // VarSocket
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 11:
                            {
                                firstValue.variableReference.setValue(secondValue.variableReference.getValue());
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 12) // VarTree
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 12:
                            {
                                firstValue.variableReference.setValue(secondValue.variableReference.getValue());
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }

            return errorCode;
        }


        //  Operation Addition

        public static IntError operationAddition(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                string second = (string)secondValue.variableReference.getValue();

                                string first = (string)firstValue.variableReference.getValue();
                                string result = first + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                string second = ((int)secondValue.variableReference.getValue()).ToString();

                                string first = (string)firstValue.variableReference.getValue();
                                string result = first + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                string second = ((float)secondValue.variableReference.getValue()).ToString();

                                string first = (string)firstValue.variableReference.getValue();
                                string result = first + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                string second = ((byte)secondValue.variableReference.getValue()).ToString();

                                string first = (string)firstValue.variableReference.getValue();
                                string result = first + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                string second = ((char)secondValue.variableReference.getValue()).ToString();

                                string first = (string)firstValue.variableReference.getValue();
                                string result = first + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                string second = ((Boolean)secondValue.variableReference.getValue()).ToString();

                                string first = (string)firstValue.variableReference.getValue();
                                string result = first + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                long second;
                                long.TryParse((string)secondValue.variableReference.getValue(), out second);
                                long first = (long)firstValue.variableReference.getValue();
                                long result = first + second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                long result = first + second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                float result = first + second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (int)firstValue.variableReference.getValue();
                                long result = first + second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                long second = Convert.ToInt64((char)secondValue.variableReference.getValue());
                                long first = (int)firstValue.variableReference.getValue();
                                long result = first + second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                long first = (long)firstValue.variableReference.getValue();
                                long result;

                                Boolean value;
                                value = (Boolean)secondValue.variableReference.getValue();
                                if (value)
                                {
                                    result = first + 1;
                                }
                                else
                                {
                                    result = first + 0;
                                }

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                float second;
                                float.TryParse((string)secondValue.variableReference.getValue(), out second);
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first + second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                float second = (float)((int)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first + second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first + second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                float second = (float)((byte)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first + second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                char second = (char)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                string result = second + first.ToString();

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                Boolean second = (Boolean)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                string result = second.ToString() + first.ToString();

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                byte value;
                                byte.TryParse((string)secondValue.variableReference.getValue(), out value);

                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first + value);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first + second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                float result = first + second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first + second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first + second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result;

                                Boolean value;
                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    result = (byte)(first + 1);
                                }
                                else
                                {
                                    result = first;
                                }

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                string second = (string)secondValue.variableReference.getValue();
                                char first = (char)firstValue.variableReference.getValue();
                                string result = first + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                char first = (char)firstValue.variableReference.getValue();
                                char result = (char)(first + second);

                                resultValue.tokentype = IntConstants.CHAR;
                                resultValue.variableReference = new VarChar();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                char first = (char)firstValue.variableReference.getValue();
                                string result = first.ToString() + second.ToString();

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                char first = (char)firstValue.variableReference.getValue();
                                char result = (char)(first + second);

                                resultValue.tokentype = IntConstants.CHAR;
                                resultValue.variableReference = new VarChar();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                char second = (char)secondValue.variableReference.getValue();
                                char first = (char)firstValue.variableReference.getValue();
                                string result = first.ToString() + second.ToString();

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                char first = (char)firstValue.variableReference.getValue();
                                string result;

                                Boolean value = (Boolean)secondValue.variableReference.getValue();

                                result = first.ToString() + value.ToString();
                                
                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                string second = (string)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                string result = first.ToString() + second;

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();

                                Boolean result = false;

                                if (second > 0)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (first == true) result = true;
                                }
                                
                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();

                                string result = first.ToString() + second.ToString();

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean result = false;

                                if (second > 0)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (first == true) result = true;
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                char second = (char)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                string result = first.ToString() + second.ToString();

                                resultValue.tokentype = IntConstants.STRING;
                                resultValue.variableReference = new VarString();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean second = (Boolean)secondValue.variableReference.getValue();
                                Boolean result = false;

                                if (second)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (first) result = true;
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        //  Operation Subtraction

        public static IntError operationSubtraction(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (int)secondValue.variableReference.getValue();
                                long first = (int)firstValue.variableReference.getValue();
                                long result = first - second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();

                                long first = (long)firstValue.variableReference.getValue();
                                float result = first - second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (int)firstValue.variableReference.getValue();
                                long result = first - second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                float second = (float)((int)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first - second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first - second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                float second = (float)((byte)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first - second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first - second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                float result = first - second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first - second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }
                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    errorCode = new IntError(firstValue, 3);
                }
                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    errorCode = new IntError(firstValue, 3);
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationMultiplication(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                long result = first * second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                long first = (int)firstValue.variableReference.getValue();
                                float result = first * second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (int)firstValue.variableReference.getValue();
                                long result = first * second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                float second = (float)((int)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first * second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first * second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                float second = (float)((byte)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first * second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first * second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                float result = first * second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first * second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }
                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    errorCode = new IntError(firstValue, 3);
                }
                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    errorCode = new IntError(firstValue, 3);
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationDivision(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                long result = first / second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();

                                long first = (long)firstValue.variableReference.getValue();
                                float result = first / second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                long result = first / second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                float second = (float)((int)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first / second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first / second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                float second = (float)((byte)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first / second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((long)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first / second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                float result = first / second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first / second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }
                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    errorCode = new IntError(firstValue, 3);
                }
                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    errorCode = new IntError(firstValue, 3);
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationModulus(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                long result = first % second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                float result = first % second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                long result = first % second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                float second = (float)((long)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first % second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first % second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                float second = (float)((byte)secondValue.variableReference.getValue());
                                float first = (float)firstValue.variableReference.getValue();
                                float result = first % second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((long)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first % second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                float result = first % second;

                                resultValue.tokentype = IntConstants.FLOAT;
                                resultValue.variableReference = new VarFloat();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first % second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }
                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    errorCode = new IntError(firstValue, 3);
                }
                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    errorCode = new IntError(firstValue, 3);
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationShiftLeft(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                long result = (int) first << (int) second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                long result = (int) first << (int) second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((long)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first << second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first << second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }
                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    errorCode = new IntError(firstValue, 3);
                }
                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    errorCode = new IntError(firstValue, 3);
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationShiftRight(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                long result = (int) first >> (int) second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                long result = (int) first >> (int) second;

                                resultValue.tokentype = IntConstants.INTEGER;
                                resultValue.variableReference = new VarInteger();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first >> second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                byte result = (byte)(first >> second);

                                resultValue.tokentype = IntConstants.BYTE;
                                resultValue.variableReference = new VarByte();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }
                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    errorCode = new IntError(firstValue, 3);
                }
                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    errorCode = new IntError(firstValue, 3);
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }


        //  Operation Addition

        public static IntError operationEquals(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                string second = (string)secondValue.variableReference.getValue();
                                string first = (string)firstValue.variableReference.getValue();

                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (int)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                long second = Convert.ToInt64((char)secondValue.variableReference.getValue());
                                long first = (int)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                long first = (int)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }

                            break;
                        case 2:
                            {
                                char second = Convert.ToChar((int)secondValue.variableReference.getValue());
                                char first = (char)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                char second = Convert.ToChar((byte)secondValue.variableReference.getValue());
                                char first = (char)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                char second = (char)secondValue.variableReference.getValue();
                                char first = (char)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)((long)secondValue.variableReference.getValue());
                                Boolean first = (Boolean)firstValue.variableReference.getValue();

                                Boolean result = false;
                                if (second > 0 && first == true)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == false)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();

                                Boolean result = false;
                                if (second > 0 && first == true)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == false)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean result = false;
                                if (second > 0 && first == true)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == false)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean result = false;
                                if (second > 0 && first == true)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == false)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean result = false;
                                Boolean value = (Boolean)secondValue.variableReference.getValue();

                                if (first == value)
                                {
                                    result = true;
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationNotEquals(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                string second = (string)secondValue.variableReference.getValue();
                                string first = (string)firstValue.variableReference.getValue();

                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                long second = Convert.ToInt64((char)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
                if (firstValue.variableReference.getVariableType() == 5) // Char
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }

                            break;
                        case 2:
                            {
                                char second = Convert.ToChar((int)secondValue.variableReference.getValue());
                                char first = (char)firstValue.variableReference.getValue();
                                Boolean result = (first == second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                char second = Convert.ToChar((byte)secondValue.variableReference.getValue());
                                char first = (char)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                char second = (char)secondValue.variableReference.getValue();
                                char first = (char)firstValue.variableReference.getValue();
                                Boolean result = (first != second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)((long)secondValue.variableReference.getValue());
                                Boolean first = (Boolean)firstValue.variableReference.getValue();

                                Boolean result = false;
                                if (second > 0 && first == false)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == true)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();

                                Boolean result = false;
                                if (second > 0 && first == false)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == true)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean result = false;
                                if (second > 0 && first == false)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == true)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean result = false;
                                if (second > 0 && first == false)
                                {
                                    result = true;
                                }
                                else
                                {
                                    if (second == 0 && first == true)
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                Boolean first = (Boolean)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (first != value)
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationLessThanEqualTo(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first <= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first <= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                long second = Convert.ToInt64((char)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first <= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value;

                                value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                Boolean result = (first <= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first <= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first <= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first <= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                Boolean result = true;
                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationGreaterThanEqualTo(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                //if (firstValue.variableReference.getVariableType() == 1) // String
                //{
                //    switch (secondValue.variableReference.getVariableType())
                //    {
                //        // Error:  cannot apply this operation to strings
                //    }
                //}

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                // Error:  cannot compare an integer to a string
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first >= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                // Error: cannot compare an integer to a float
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first >= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                long second = Convert.ToInt64((char)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first >= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result;
                                Boolean value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                Boolean result = (first >= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first >= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first >= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first >= second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                Boolean result = true;
                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationGreaterThan(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first > second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first > second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                long second = Convert.ToInt64((char)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first > second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result;

                                Boolean value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                Boolean result = (first > second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first > second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first > second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first > second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result;

                                Boolean value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationLessThan(IntRuntime runtime, ArrayList opStack)
        {
            IntToken secondValue = (IntToken)Pop(opStack);
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (secondValue != null && firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 1) // String
                {
                    errorCode = new IntError(firstValue, 3);
                }

                if (firstValue.variableReference.getVariableType() == 2) // Integer
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                long second = (long)secondValue.variableReference.getValue();
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first < second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                long second = Convert.ToInt64((byte)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first < second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                long second = Convert.ToInt64((char)secondValue.variableReference.getValue());
                                long first = (long)firstValue.variableReference.getValue();
                                Boolean result = (first < second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                long first = (int)firstValue.variableReference.getValue();
                                Boolean result;

                                Boolean value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 3) // Float
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 3:
                            {
                                float second = (float)secondValue.variableReference.getValue();
                                float first = (float)firstValue.variableReference.getValue();
                                Boolean result = (first < second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 4:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 5:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 6:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                    }
                }

                if (firstValue.variableReference.getVariableType() == 4) // Byte
                {
                    switch (secondValue.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 2:
                            {
                                byte second = (byte)((int)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first < second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 3:
                            {
                                errorCode = new IntError(firstValue, 3);
                            }
                            break;
                        case 4:
                            {
                                byte second = (byte)secondValue.variableReference.getValue();
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first < second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 5:
                            {
                                byte second = Convert.ToByte((char)secondValue.variableReference.getValue());
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result = (first < second);

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                        case 6:
                            {
                                byte first = (byte)firstValue.variableReference.getValue();
                                Boolean result;

                                Boolean value = (Boolean)secondValue.variableReference.getValue();

                                if (value)
                                {
                                    if (first > 0)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    if (first > 0)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }

                                resultValue.tokentype = IntConstants.BOOLEAN;
                                resultValue.variableReference = new VarBoolean();
                                resultValue.variableReference.setValue(result);
                                Push(opStack, resultValue);
                            }
                            break;
                    }
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        public static IntError operationNot(IntRuntime runtime, ArrayList opStack)
        {
            IntToken firstValue = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorCode = null;

            if (firstValue != null)
            {
                if (firstValue.variableReference.getVariableType() == 6) // Boolean
                {
                    Boolean first = (Boolean)firstValue.variableReference.getValue();
                    resultValue.tokentype = IntConstants.BOOLEAN;
                    resultValue.variableReference = new VarBoolean();
                    resultValue.variableReference.setValue(!first);
                    Push(opStack, resultValue);
                }
                else
                {
                    errorCode = new IntError(firstValue, 3);
                }
            }
            else
            {
                errorCode = new IntError(firstValue, 4);
            }
            return errorCode;
        }

        private static int advanceForward(IntRuntime runtime, IntToken operation, IntFunction funct)
        {
            int result = -1;
            int depthcount = 0;

            if (operation.advance == -1)
            {
                for (int i = (int) (runtime.lineNumber + 1); i < funct.compilation.Count; i++)
                {
                    ArrayList line = (ArrayList)funct.compilation[i];
                    if (line != null)
                    {
                        if (line.Count > 2)
                        {
                            IntToken nextToLast = (IntToken)line[line.Count - 2];
                            switch (nextToLast.tokentype)
                            {
                                case IntConstants.RES_FOR:
                                case IntConstants.RES_WHILE:
                                case IntConstants.RES_UNTIL:
                                case IntConstants.RES_IF:
                                    depthcount++;
                                    break;
                            }
                        }
                        else
                        {
                            if (line.Count == 2)
                            {
                                IntToken tmptoken = (IntToken)line[0];
                                switch (tmptoken.tokentype)
                                {
                                    case IntConstants.RES_ENDIF:
                                    case IntConstants.RES_ENDWHILE:
                                    case IntConstants.RES_NEXT:
                                        depthcount--;
                                        break;
                                }
                            }
                            if (depthcount == -1)
                            {
                                operation.advance = i;
                                result = i;
                                i = (int) funct.compilation.Count;
                            }
                        }
                    }
                }
            }
            else
            {
                result = operation.advance;
            }

            return result;
        }

        private static int advanceBackward(IntRuntime runtime, IntToken operation, IntFunction funct)
        {
            int result = -1;
            int depthcount = 0;

            if (operation.advance == -1)
            {
                for (int i = (int) (runtime.lineNumber - 1); i > -1; i--)
                {
                    ArrayList line = (ArrayList)funct.compilation[i];
                    if (line != null)
                    {
                        if (line.Count > 2)
                        {
                            IntToken nextToLast = (IntToken)line[line.Count - 2];
                            switch (nextToLast.tokentype)
                            {
                                case IntConstants.RES_UNTIL:
                                    depthcount++;
                                    break;
                            }
                        }
                        else
                        {
                            if (line.Count == 2)
                            {
                                IntToken tmptoken = (IntToken)line[0];
                                switch (tmptoken.tokentype)
                                {
                                    case IntConstants.RES_REPEAT:
                                        depthcount--;
                                        break;
                                }
                            }
                            if (depthcount == -1)
                            {
                                operation.advance = i;
                                result = i;
                                i = -1;
                            }
                        }
                    }
                }
            }
            else
            {
                result = operation.advance;
            }

            return result;
        }

        public static IntError operationIf(IntRuntime runtime, ArrayList varStack, IntToken operation, IntFunction funct, ArrayList parameters, ArrayList opStack)
        {
            IntError result = null;
            IntToken firstValue = (IntToken)Pop(opStack);

            if (firstValue != null)
            {
                if (firstValue.tokentype == IntConstants.BOOLEAN)
                {
                    Boolean value = (Boolean) firstValue.variableReference.getValue();
                    if (value)
                    {
                        runtime.lineNumber++;
                        Interpreter.processRPN(runtime, varStack, funct, parameters, null, opStack);
                    }
                    else
                    {
                        runtime.lineNumber = advanceForward(runtime,operation,funct);
                    }

                }
                else
                {
                    if (firstValue.tokentype == IntConstants.INTEGER)
                    {
                        long value = (long)firstValue.variableReference.getValue();
                        if (value != 0)
                        {
                            runtime.lineNumber++;
                            Interpreter.processRPN(runtime, varStack, funct, parameters, null, opStack);
                        }
                    }
                    else
                    {
                        if (firstValue.tokentype == IntConstants.BYTE)
                        {
                            byte value = (byte)firstValue.variableReference.getValue();
                            if (value != 0)
                            {
                                runtime.lineNumber++;
                                Interpreter.processRPN(runtime, varStack, funct, parameters, null, opStack);
                            }
                        }
                        else
                        {
                            result = new IntError(null, 15);
                        }
                    }
                }
            }
            else
            {
                result = new IntError(null, 14);
            }
            return result;
        }

        public static IntError operationWhile(IntRuntime runtime, ArrayList varStack, IntToken operation, IntFunction funct, ArrayList parameters, ArrayList opStack)
        {
            IntError result = null;
            IntToken firstValue = (IntToken)Pop(opStack);
            short startline = (short) (runtime.lineNumber - 1);

            if (firstValue != null)
            {
                if (firstValue.tokentype == IntConstants.BOOLEAN)
                {
                    Boolean value = (Boolean)firstValue.variableReference.getValue();
                    if (value)
                    {
                        runtime.lineNumber++;
                        Interpreter.processRPN(runtime, varStack, funct, parameters, null, opStack);
                        runtime.lineNumber = startline;
                    }
                    else
                    {
                        runtime.lineNumber = advanceForward(runtime, operation, funct);
                    }
                }
                else
                {
                    if (firstValue.tokentype == IntConstants.INTEGER)
                    {
                        long value = (long)firstValue.variableReference.getValue();
                        if (value != 0)
                        {
                            runtime.lineNumber++;
                            Interpreter.processRPN(runtime, varStack, funct, parameters, null, opStack);
                            runtime.lineNumber = startline;
                        }
                        else
                        {
                            runtime.lineNumber = advanceForward(runtime, operation, funct);
                        }
                    }
                    else
                    {
                        if (firstValue.tokentype == IntConstants.BYTE)
                        {
                            byte value = (byte)firstValue.variableReference.getValue();
                            if (value != 0)
                            {
                                runtime.lineNumber++;
                                Interpreter.processRPN(runtime, varStack, funct, parameters, null, opStack);
                                runtime.lineNumber = startline;
                            }
                            else
                            {
                                runtime.lineNumber = advanceForward(runtime, operation, funct);
                            }
                        }
                        else
                        {
                            result = new IntError(null, 15);
                        }
                    }
                }
            }
            else
            {
                result = new IntError(null, 14);
            }
            return result;
        }

        public static IntError operationFor(IntRuntime runtime, ArrayList varStack, IntToken operation, IntFunction funct, ArrayList parameters, ArrayList opStack)
        {
            IntError result = null;
            IntToken firstValue = (IntToken)Pop(opStack);
            if (firstValue.tokentype == IntConstants.UNKNOWN || firstValue.tokentype == -1)
            {
                IntToken tmpcurrent = Interpreter.locateVariable(runtime, varStack, parameters, firstValue);
                if (tmpcurrent == null)
                {
                    result = new IntError(firstValue, 11);
                    return result;
                }
                else
                {
                    firstValue = tmpcurrent;
                }
            }

            int startline = runtime.lineNumber;
            ArrayList evaluation = new ArrayList();
            ArrayList assignment = new ArrayList();

            ArrayList currentline = (ArrayList)funct.compilation[runtime.lineNumber];

            Push(opStack, firstValue);
            Push(opStack,currentline[2]);
            operationAssignment(runtime, opStack);

            string indexname = "for-temp";

            IntToken index = new IntToken(indexname, IntConstants.INTEGER);
            index.variableReference = new VarInteger();

            evaluation.Add(index);
            for (int i = 3; i < currentline.Count - 3; i++)
            {
                IntToken current = (IntToken)currentline[i];
                if (current.tokentype == IntConstants.UNKNOWN || current.tokentype == -1)
                {

                    IntToken tmpcurrent = Interpreter.locateVariable(runtime, varStack, parameters, current);

                    if (tmpcurrent==null)
                    {
                        result = new IntError(current,11);
                        return result;
                    }
                    else
                    {
                        current = tmpcurrent;
                    }
                }
                evaluation.Add(current);
            }
            evaluation.Add(new IntToken(null,IntConstants.OP_ASSIGNMENT));

            IntRuntime miniRuntime = new IntRuntime();
            miniRuntime.lineNumber = 0;
            miniRuntime.globalVariables.Add(index);

            ArrayList miniCompilation = new ArrayList();
            miniCompilation.Add(evaluation);
            IntFunction minifunction = new IntFunction(miniCompilation, true);

            Boolean firstLoop = true;

            Boolean loop = true;
            while (loop)
            {
                if (firstLoop)
                {
                    firstLoop = false;
                }
                else
                {
                    long tmp = (long)firstValue.variableReference.getValue();
                    tmp++;
                    firstValue.variableReference.setValue(tmp);
                }

                // Evaluate For Expression and increment value
                Interpreter.processRPN(miniRuntime, varStack, minifunction, null, null, opStack);

                if ((long) firstValue.variableReference.getValue()  < (long) index.variableReference.getValue())
                {
                    runtime.lineNumber++;
                    Interpreter.processRPN(runtime, varStack, funct, parameters, null, opStack);
                    runtime.lineNumber = startline;
                }
                else
                {
                    runtime.lineNumber = advanceForward(runtime, operation, funct);
                    loop = false;
                }
            }

            index.variableReference = null;
            evaluation.Clear();
            assignment.Clear();

            return result;
        }

        public static IntError operationRepeat(IntRuntime runtime, ArrayList varStack, IntToken operation, IntFunction funct, ArrayList parameters, ArrayList opStack)
        {
            IntError result = null;
            IntToken firstValue = (IntToken)Pop(opStack);
            int startline = advanceBackward(runtime, operation, funct);

            if (firstValue != null)
            {
                if (firstValue.tokentype == IntConstants.BOOLEAN)
                {
                    Boolean value = (Boolean)firstValue.variableReference.getValue();
                    if (!value)
                    {
                        runtime.lineNumber = startline;
                    }
                }
                else
                {
                    if (firstValue.tokentype == IntConstants.INTEGER)
                    {
                        long value = (long)firstValue.variableReference.getValue();
                        if (value == 0)
                        {
                            runtime.lineNumber = startline;
                        }
                    }
                    else
                    {
                        if (firstValue.tokentype == IntConstants.BYTE)
                        {
                            byte value = (byte)firstValue.variableReference.getValue();
                            if (value == 0)
                            {
                                runtime.lineNumber = startline;
                            }
                        }
                        else
                        {
                            result = new IntError(null, 15);
                        }
                    }
                }
            }
            else
            {
                result = new IntError(null, 14);
            }
            return result;
        }
    }
}
