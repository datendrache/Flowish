//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.Data;
using System.IO;
using FatumCore;
using Microsoft.Win32;
using AbsolutionLib.CanOpener;
using System.Data.SqlClient;
using Fatum.FatumCore;

namespace Flowish
{
    public class Flowish
    {
        static public void locateLibraryFunction(IntToken token)
        {
            switch (token.text)
            {
                // Input/Output Library
                
                case "print": token.tokentype = 1000; break; 

                // Type conversion library

                case "integer": token.tokentype = 1001; break; 
                case "string": token.tokentype = 1002; break; 
                case "char": token.tokentype = 1003; break; 
                case "byte": token.tokentype = 1004; break; 
                case "float": token.tokentype = 1005; break; 
                case "boolean": token.tokentype = 1006; break; 

                // String manipulation library

                case "strcat": token.tokentype = 1007; break;
                case "strlen": token.tokentype = 1008; break;
                case "strreplace": token.tokentype = 1011; break;
                case "strindexof": token.tokentype = 1012; break;
                case "substring": token.tokentype = 1013; break;
                case "regex": token.tokentype = 1024; break;

                // Math library

                case "cos": token.tokentype = 1014; break;
                case "sin": token.tokentype = 1015; break;
                case "tan": token.tokentype = 1016; break;
                case "abs": token.tokentype = 1017; break;
                case "asin": token.tokentype = 1018; break;
                case "acos": token.tokentype = 1019; break;
                case "atan": token.tokentype = 1020; break;
                case "log": token.tokentype = 1021; break;
                case "sqrt": token.tokentype = 1022; break;
                case "exp": token.tokentype = 1023; break;

                // I/O Library

                case "open": token.tokentype = 1040; break;
                case "close": token.tokentype = 1041; break;
                case "read": token.tokentype = 1080; break;
                case "write": token.tokentype = 1081; break;
                case "append": token.tokentype = 1082; break;
                case "flush": token.tokentype = 1083; break;
                case "readline": token.tokentype = 1084; break;
                case "writeline": token.tokentype = 1085; break;
                case "eof": token.tokentype = 1086; break;

                // Overloaded Functions

                case "count": token.tokentype = 1045; break;

                // Database Library

                case "databaseexecutesql": token.tokentype = 1042; break;
                case "databasequery": token.tokentype = 1043; break;
                case "databasecreate": token.tokentype = 1044; break;                
                case "getrow": token.tokentype = 1046; break;
                case "setrowfield": token.tokentype = 1047; break;
                   
                // Tree Library

                case "findnode": token.tokentype = 1050; break;
                case "getnode": token.tokentype = 1051; break;
                case "setattribute": token.tokentype = 1052; break;
                case "getattribute": token.tokentype = 1053; break;
                case "removeattribute": token.tokentype = 1054; break;
                case "setelement": token.tokentype = 1055; break;
                case "getelement": token.tokentype = 1056; break;
                case "removeelement": token.tokentype = 1057; break;
                case "newTree": token.tokentype = 1058; break;
                case "disposeTree": token.tokentype = 1059; break;
                case "readxml": token.tokentype = 1060; break;
                case "writexml": token.tokentype = 1061; break;
                case "getleafnames": token.tokentype = 1062; break;
                case "getattributes": token.tokentype = 1063; break;
                case "readjson": token.tokentype = 1064; break;
                case "writejson": token.tokentype = 1065; break;
                case "addnode": token.tokentype = 1066; break;

                // Registry Key Library

                case "loadregistrykeyvalue": token.tokentype = 1070; break;
                case "loadregistrykey": token.tokentype = 1071; break;
                case "loadregistrysubkeys": token.tokentype = 1072; break;

                // SQL ADO

                case "sqlexecutequery": token.tokentype = 1080; break;
                case "sqlexecutenonquery": token.tokentype = 1081; break;

                // Custom Items follow

                case "forwardmessage": token.tokentype = 1010; break;
                case "evidencetag": token.tokentype = 1201; break;
                case "vadersentiment": token.tokentype = 1200; break;
                case "nop": token.tokentype = 1202; break;
                case "breakpoint": token.tokentype = 1203; break;
            }
        }

        static public IntError callLibraryFunction(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;

            switch (token.tokentype)
            {
                case 1000: result = callPrint(runtime, opStack, token); break;
                case 1001: result = callInteger(runtime, opStack, token); break;
                case 1002: result = callString(runtime, opStack, token); break;
                case 1003: result = callFloat(runtime, opStack, token); break;
                case 1004: result = callByte(runtime, opStack, token); break;
                case 1005: result = callChar(runtime, opStack, token); break;
                case 1006: result = callBoolean(runtime, opStack, token); break;

                // Math libraries

                case 1014: result = callCosine(runtime, opStack, token); break;
                case 1015: result = callSine(runtime, opStack, token); break;
                case 1016: result = callTan(runtime, opStack, token); break;
                case 1017: result = callAbs(runtime, opStack, token); break;
                case 1018: result = callArcSin(runtime, opStack, token); break;
                case 1019: result = callArcCos(runtime, opStack, token); break;
                case 1020: result = callArcTan(runtime, opStack, token); break;
                case 1021: result = callLog(runtime, opStack, token); break;
                case 1022: result = callSqrt(runtime, opStack, token); break;
                case 1023: result = callExp(runtime, opStack, token); break;

                // String libraries

                case 1007: result = callStrcat(runtime, opStack, token); break;
                case 1008: result = callStrlen(runtime, opStack, token); break;
                case 1011: result = callStrReplace(runtime, opStack, token); break;
                case 1012: result = callStrIndexOf(runtime, opStack, token); break;
                case 1013: result = callSubstring(runtime, opStack, token); break;
                case 1024: result = callRegex(runtime, opStack, token); break;

                // Database libraries

                case 1040: result = Open(runtime, opStack, token); break;
                case 1041: result = Close(runtime, opStack, token); break;

                // Database libraries

                case 1042: result = DatabaseExecuteSql(runtime, opStack, token); break;
                case 1043: result = DatabaseQuery(runtime, opStack, token); break;
                case 1044: result = DatabaseCreate(runtime, opStack, token); break;
                case 1045: result = Count(runtime, opStack, token); break;
                case 1046: result = GetRowField(runtime, opStack, token); break;
                case 1047: result = SetRowField(runtime, opStack, token); break;

                // Tree libraries

                case 1050: result = TreeFindNode(runtime, opStack, token); break;
                case 1051: result = TreeGetNode(runtime, opStack, token); break;
                case 1052: result = TreeSetAttribute(runtime, opStack, token); break;
                case 1053: result = TreeGetAttribute(runtime, opStack, token); break;
                case 1054: result = TreeRemoveAttribute(runtime, opStack, token); break;
                case 1055: result = TreeSetElement(runtime, opStack, token); break;
                case 1056: result = TreeGetElement(runtime, opStack, token); break;
                case 1057: result = TreeRemoveElement(runtime, opStack, token); break;
                case 1058: result = TreeNew(runtime, opStack, token); break;
                case 1059: result = TreeDispose(runtime, opStack, token); break;
                case 1060: result = TreeReadXML(runtime, opStack, token); break;
                case 1061: result = TreeWriteXML(runtime, opStack, token); break;
                case 1062: result = TreeGetLeafnames(runtime, opStack, token); break;
                case 1063: result = TreeGetAttributeNames(runtime, opStack, token); break;
                case 1064: result = TreeReadJson(runtime, opStack, token); break;
                case 1065: result = TreeWriteJson(runtime, opStack, token); break;
                case 1066: result = TreeAddNode(runtime, opStack, token); break;

                // Registry libraries

                case 1070: result = loadRegistryKeyValue(runtime, opStack, token); break;
                case 1071: result = loadRegistryKey(runtime, opStack, token); break;
                case 1072: result = loadSubkeys(runtime, opStack, token); break;

                // SQL ADO

                case 1080: result = SQLExecuteQuery(runtime, opStack, token); break;
                case 1081: result = SQLExecuteNonQuery(runtime, opStack, token); break;

                // File Functions

                case 1082: result = appendToFile(runtime, opStack, token); break;

                // Custom items follow

                case 1010: result = callForwardMessage(runtime, opStack, token); break;
                //case 1200: result = sentiment(runtime, opStack, token); break;
                case 1202: result = nop(runtime, opStack, token); break;
                case 1203: result = breakpoint(runtime, opStack, token); break;
            }
            return result;
        }

        static private object Pop(ArrayList stack)
        {
            Object current = null;
            if (stack.Count > 0)
            {
                current = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
            }
            return current;
        }

        static private void Push(ArrayList stack, Object item)
        {
            stack.Add(item);
        }

        static private IntError callPrint(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken message = (IntToken)Pop(opStack);
            IntError result = null;

            if (message != null)
            {
                if (message.tokentype != IntConstants.ENDOFSTATEMENT)
                {
                    switch (message.variableReference.getVariableType())
                    {
                        case 1:
                            {
                                runtime.Output.Add((string)message.variableReference.getValue());
                            }
                            break;
                        case 2:
                            {
                                runtime.Output.Add((string)((long)message.variableReference.getValue()).ToString());
                            }
                            break;
                        case 3:
                            {
                                runtime.Output.Add((string)((float)message.variableReference.getValue()).ToString());
                            }
                            break;
                        case 4:
                            {
                                runtime.Output.Add((string)((byte)message.variableReference.getValue()).ToString());
                            }
                            break;
                        case 5:
                            {
                                runtime.Output.Add((string)((char)message.variableReference.getValue()).ToString());
                            }
                            break;
                        case 6:
                            {
                                runtime.Output.Add((string)((Boolean)message.variableReference.getValue()).ToString());
                            }
                            break;
                    }
                }
                else
                {
                    runtime.Output.Add("");
                }
            }
            else
            {
                runtime.Output.Add("");
            }
            return result;
        }

        static private IntError callString(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken convert = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError errorresult = null;

            switch (convert.variableReference.getVariableType())
            {
                case 1:
                    {
                        string result = (string)convert.variableReference.getValue();

                        resultValue.tokentype = IntConstants.STRING;
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(result);
                        Push(opStack, resultValue);
                    }
                    break;
                case 2:
                    {
                        string result = ((int)convert.variableReference.getValue()).ToString();

                        resultValue.tokentype = IntConstants.STRING;
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(result);
                        Push(opStack, resultValue);
                    }
                    break;
                case 3:
                    {
                        string result = ((float)convert.variableReference.getValue()).ToString();

                        resultValue.tokentype = IntConstants.STRING;
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(result);
                        Push(opStack, resultValue);
                    }
                    break;
                case 4:
                    {
                        string result = ((byte)convert.variableReference.getValue()).ToString();

                        resultValue.tokentype = IntConstants.STRING;
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(result);
                        Push(opStack, resultValue);
                    }
                    break;
                case 5:
                    {
                        string result = ((char)convert.variableReference.getValue()).ToString();

                        resultValue.tokentype = IntConstants.STRING;
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(result);
                        Push(opStack, resultValue);
                    }
                    break;
                case 6:
                    {
                        string result = ((Boolean)convert.variableReference.getValue()).ToString();

                        resultValue.tokentype = IntConstants.STRING;
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(result);
                        Push(opStack, resultValue);
                    }
                    break;
                default:
                    errorresult = new IntError(token, 3);
                    break;
            }
            return errorresult;
        }

        static private IntError callInteger(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken convert = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            switch (convert.variableReference.getVariableType())
                {
                    case 1:
                        {
                            long second = 0;
                            long.TryParse((string)convert.variableReference.getValue(), out second);
                            
                            resultValue.tokentype = IntConstants.INTEGER;
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(second);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 2:
                        {
                            long second = (long)convert.variableReference.getValue();
                            
                            resultValue.tokentype = IntConstants.INTEGER;
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(second);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 3:
                        {
                            int second = Convert.ToInt32((float)convert.variableReference.getValue());

                            resultValue.tokentype = IntConstants.INTEGER;
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(second);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 4:
                        {
                            int second = Convert.ToInt32((byte)convert.variableReference.getValue());

                            resultValue.tokentype = IntConstants.INTEGER;
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(second);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 5:
                        {
                            int second = Convert.ToInt32((char)convert.variableReference.getValue());

                            resultValue.tokentype = IntConstants.INTEGER;
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(second);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 6:
                        {
                            int tmp = 0;

                            Boolean value = false;
                            value = (Boolean)convert.variableReference.getValue();
                            if (value)
                            {
                                tmp = 1;
                            }
                            else
                            {
                                tmp = 0;
                            }

                            resultValue.tokentype = IntConstants.INTEGER;
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(tmp);
                            Push(opStack, resultValue);
                        }
                        break;
                    default:
                        result = new IntError(token, 3);
                        break;
                
            }
            return result;
        }

        static private IntError callFloat(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken convert = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            switch (convert.variableReference.getVariableType())
            {
                case 1:
                    {
                        float tmp = 0;
                        float.TryParse((string)convert.variableReference.getValue(), out tmp);

                        resultValue.tokentype = IntConstants.FLOAT;
                        resultValue.variableReference = new VarFloat();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 2:
                    {
                        float tmp = (float)((int)convert.variableReference.getValue());

                        resultValue.tokentype = IntConstants.FLOAT;
                        resultValue.variableReference = new VarFloat();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 3:
                    {
                        float tmp = (float)convert.variableReference.getValue();

                        resultValue.tokentype = IntConstants.FLOAT;
                        resultValue.variableReference = new VarFloat();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 4:
                    {
                        float tmp = (float)((byte)convert.variableReference.getValue());

                        resultValue.tokentype = IntConstants.FLOAT;
                        resultValue.variableReference = new VarFloat();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                default:
                    result = new IntError(token, 3);
                    break;
            }
            return result;
        }

        static private IntError callChar(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken convert = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            switch (convert.variableReference.getVariableType())
            {
                case 2:
                    {
                        char tmp = Convert.ToChar(((int)convert.variableReference.getValue()));

                        resultValue.tokentype = IntConstants.CHAR;
                        resultValue.variableReference = new VarChar();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 4:
                    {
                        char tmp = Convert.ToChar((byte)convert.variableReference.getValue());

                        resultValue.tokentype = IntConstants.CHAR;
                        resultValue.variableReference = new VarChar();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 5:
                    {
                        resultValue.tokentype = IntConstants.CHAR;
                        resultValue.variableReference = new VarChar();
                        resultValue.variableReference.setValue((char)convert.variableReference.getValue());
                        Push(opStack, resultValue);
                    }
                    break;
                default:
                    result = new IntError(token, 3);
                    break;
            }

            return result;
        }

        static private IntError callByte(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken convert = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            switch (convert.variableReference.getVariableType())
                {
                    case 1:
                        {
                            byte value = 0;
                            byte.TryParse((string)convert.variableReference.getValue(), out value);

                            resultValue.tokentype = IntConstants.BYTE;
                            resultValue.variableReference = new VarByte();
                            resultValue.variableReference.setValue(value);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 2:
                        {
                            byte tmp = (byte)((int)convert.variableReference.getValue());
                            
                            resultValue.tokentype = IntConstants.BYTE;
                            resultValue.variableReference = new VarByte();
                            resultValue.variableReference.setValue(tmp);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 3:
                        {
                            byte tmp = (byte)((float)convert.variableReference.getValue());
                            
                            resultValue.tokentype = IntConstants.BYTE;
                            resultValue.variableReference = new VarByte();
                            resultValue.variableReference.setValue(tmp);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 4:
                        {
                            byte tmp = (byte)convert.variableReference.getValue();
                            
                            resultValue.tokentype = IntConstants.BYTE;
                            resultValue.variableReference = new VarByte();
                            resultValue.variableReference.setValue(tmp);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 5:
                        {
                            byte tmp = Convert.ToByte((char)convert.variableReference.getValue());
                            
                            resultValue.tokentype = IntConstants.BYTE;
                            resultValue.variableReference = new VarByte();
                            resultValue.variableReference.setValue(tmp);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 6:
                        {
                            byte tmp = 0;

                            Boolean value = false;
                            value = (Boolean)convert.variableReference.getValue();

                            if (value)
                            {
                                tmp = (byte)(1);
                            }
                            else
                            {
                                tmp = 0;
                            }

                            resultValue.tokentype = IntConstants.BYTE;
                            resultValue.variableReference = new VarByte();
                            resultValue.variableReference.setValue(tmp);
                            Push(opStack, resultValue);
                        }
                        break;
                }
            return result;
        }

        static private IntError callBoolean(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken convert = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            switch (convert.variableReference.getVariableType())
            {
                case 1:
                    {
                        
                        string value = (string)convert.variableReference.getValue();
                        value = value.ToLower();
                        
                        Boolean tmp = false;

                        if (value.ToLower() == "true")
                        {
                            tmp = true;
                        }

                        resultValue.tokentype = IntConstants.BOOLEAN;
                        resultValue.variableReference = new VarBoolean();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 2:
                    {
                        long value = (int)convert.variableReference.getValue();
                        Boolean tmp = false;

                        if (value!=0)
                        {
                            tmp = true;
                        }

                        resultValue.tokentype = IntConstants.BOOLEAN;
                        resultValue.variableReference = new VarBoolean();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 3:
                    {
                        float value = (float)convert.variableReference.getValue();
                        Boolean tmp = false;

                        if (value != 0)
                        {
                            tmp = true;
                        }

                        resultValue.tokentype = IntConstants.BOOLEAN;
                        resultValue.variableReference = new VarBoolean();
                        resultValue.variableReference.setValue(tmp);
                        Push(opStack, resultValue);
                    }
                    break;
                case 4:
                    {
                        byte tmp = (byte)convert.variableReference.getValue();
                        Boolean value = false;
                        if (tmp!=0)
                        {
                            value = true;
                        }
                        resultValue.tokentype = IntConstants.BOOLEAN;
                        resultValue.variableReference = new VarBoolean();
                        resultValue.variableReference.setValue(value);
                        Push(opStack, resultValue);
                    }
                    break;
                case 5:
                    {
                        char tmp = (char)convert.variableReference.getValue();
                        Boolean value = false;

                        if (tmp!='\0')
                        {
                            value = true;
                        }
                        resultValue.tokentype = IntConstants.BOOLEAN;
                        resultValue.variableReference = new VarBoolean();
                        resultValue.variableReference.setValue(value);
                        Push(opStack, resultValue);
                    }
                    break;
                case 6:
                    {
                        resultValue.tokentype = IntConstants.BOOLEAN;
                        resultValue.variableReference = new VarBoolean();
                        resultValue.variableReference.setValue((Boolean)convert.variableReference.getValue());
                        Push(opStack, resultValue);
                    }
                    break;
            }

            return result;
        }

        static private IntError callStrcat(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken combineOne = (IntToken)Pop(opStack);
            IntToken combineTwo = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (combineOne == null || combineTwo == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if ((combineOne.variableReference.getVariableType() == 1) && (combineTwo.variableReference.getVariableType() == 1))
                {
                    string valueOne = (string)combineOne.variableReference.getValue();
                    string valueTwo = (string)combineTwo.variableReference.getValue();
                    string value = valueOne + valueTwo;

                    resultValue.tokentype = IntConstants.STRING;
                    resultValue.variableReference = new VarString();
                    resultValue.variableReference.setValue(value);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callStrlen(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken tmpString = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.INTEGER);
            IntError result = null;

            if (tmpString == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (tmpString.variableReference.getVariableType() == 1)
                {
                    string valueOne = (string)tmpString.variableReference.getValue();
                    long value = valueOne.Length;

                    resultValue.tokentype = IntConstants.INTEGER;
                    resultValue.variableReference = new VarInteger();
                    resultValue.variableReference.setValue(value);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callStrIndexOf(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken matchString = (IntToken)Pop(opStack);
            IntToken bigString = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.INTEGER);
            IntError result = null;

            if (bigString == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (matchString.variableReference.getVariableType() == 1)
                {
                    string valueOne = (string)bigString.variableReference.getValue();
                    string valueTwo = (string)matchString.variableReference.getValue();
                    long value = valueOne.IndexOf(valueTwo);

                    resultValue.tokentype = IntConstants.INTEGER;
                    resultValue.variableReference = new VarInteger();
                    resultValue.variableReference.setValue(value);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callStrReplace(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken combineOne = (IntToken)Pop(opStack);
            IntToken combineTwo = (IntToken)Pop(opStack);
            IntToken target = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (combineOne == null || combineTwo == null || target == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if ((combineOne.variableReference.getVariableType() == 1) && (combineTwo.variableReference.getVariableType() == 1))
                {
                    string valueOne = (string)combineOne.variableReference.getValue();
                    string valueTwo = (string)combineTwo.variableReference.getValue();
                    string targetOne = (string)target.variableReference.getValue();

                    string value = targetOne.Replace(valueTwo, valueOne);

                    resultValue.tokentype = IntConstants.STRING;
                    resultValue.variableReference = new VarString();
                    resultValue.variableReference.setValue(value);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callSubstring(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken length = (IntToken)Pop(opStack);
            IntToken start = (IntToken)Pop(opStack);
            IntToken bigstring = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (length == null || start == null || bigstring == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if ((length.variableReference.getVariableType() == 2) && (start.variableReference.getVariableType() == 2) && (bigstring.variableReference.getVariableType() == 1))
                {
                    int lengthValue = (int)length.variableReference.getValue();
                    int startValue = (int)start.variableReference.getValue();
                    string bigstringValue = (string)bigstring.variableReference.getValue();

                    string value = bigstringValue.Substring(startValue,lengthValue);

                    resultValue.tokentype = IntConstants.STRING;
                    resultValue.variableReference = new VarString();
                    resultValue.variableReference.setValue(value);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callCosine(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);


            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float) Math.Cos((double) mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callSine(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);


            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Sin((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callTan(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Tan((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callAbs(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Abs((double)mathValue);
                    
                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    if (value.variableReference.getVariableType() == 2)
                    {
                        int mathValue = (int)value.variableReference.getValue();
                        int calc = Math.Abs(mathValue);

                        resultValue.tokentype = IntConstants.INTEGER;
                        resultValue.variableReference = new VarInteger();
                        resultValue.variableReference.setValue(calc);
                        Push(opStack, resultValue);
                    }
                    else
                    {
                        result = new IntError(token, 3);
                    }
                }
            }
            return result;
        }

        static private IntError callArcTan(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Atan((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callArcCos(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Acos((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callArcSin(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Asin((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callLog(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Log((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callSqrt(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Sqrt((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callExp(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken value = (IntToken)Pop(opStack);

            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (value == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if (value.variableReference.getVariableType() == 3)
                {
                    float mathValue = (float)value.variableReference.getValue();
                    float calc = (float)Math.Exp((double)mathValue);

                    resultValue.tokentype = IntConstants.FLOAT;
                    resultValue.variableReference = new VarFloat();
                    resultValue.variableReference.setValue(calc);
                    Push(opStack, resultValue);
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }

        static private IntError callRegex(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntToken combineOne = (IntToken)Pop(opStack);
            IntToken combineTwo = (IntToken)Pop(opStack);
            IntToken resultValue = new IntToken("", IntConstants.UNKNOWN);
            IntError result = null;

            if (combineOne == null || combineTwo == null)
            {
                result = new IntError(token, 3);
            }
            else
            {
                if ((combineOne.variableReference.getVariableType() == 1) && (combineTwo.variableReference.getVariableType() == 1))
                {
                    try
                    {
                        string stringVar = (string)combineOne.variableReference.getValue();
                        string expressionVar = (string)combineTwo.variableReference.getValue();

                        Regex newRegex = new Regex(expressionVar, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        Match match = newRegex.Match(stringVar);

                        if (match.Success)
                        {
                            resultValue.tokentype = IntConstants.STRING;
                            resultValue.variableReference = new VarString();
                            resultValue.variableReference.setValue(match.Value);
                        }
                        else
                        {
                            resultValue.tokentype = IntConstants.STRING;
                            resultValue.variableReference = new VarString();
                            resultValue.variableReference.setValue("");
                        }
                        Push(opStack, resultValue);
                    }
                    catch (Exception)
                    {
                        result = new IntError(token, 31);
                    }
                }
                else
                {
                    result = new IntError(token, 3);
                }
            }
            return result;
        }


        // THIS SECTION IS FOR FUNCTIONS THAT ARE EXPECTED TO BE OVERLOADED FOR VARIOUS COMMUNICATIONS TYPES
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //

        static private IntError Count(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            if (token.tokentype == IntConstants.ARRAY || token.tokentype == IntConstants.RES_ARRAY)
            {
                IntToken resultValue = new IntToken("<tmp>", IntConstants.INTEGER);
                resultValue.variableReference = new VarInteger();
                resultValue.variableReference.setValue(token.array.Count);
                Push(opStack, resultValue);
            }
            else
            {
                switch (target.variableReference.getVariableType())
                {
                    case 7:
                        {
                            DataTable tmpTable = (DataTable)target.variableReference.getValue();
                            IntToken resultValue = new IntToken("<tmp>", IntConstants.INTEGER);
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(tmpTable.Rows.Count);
                            Push(opStack, resultValue);
                        }
                        break;
                    case 8:
                        {
                            DataRow tmpRow = (DataRow) target.variableReference.getValue();
                            IntToken resultValue = new IntToken("<tmp>", IntConstants.INTEGER);
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(tmpRow.ItemArray.Count());
                            Push(opStack, resultValue);
                        }
                        break;
                    case 12:  // Tree Tree Nodes
                        {
                            Tree tmpTree = (Tree)target.variableReference.getValue();
                            IntToken resultValue = new IntToken("<tmp>", IntConstants.INTEGER);
                            resultValue.variableReference = new VarInteger();
                            resultValue.variableReference.setValue(tmpTree.tree.Count);
                            Push(opStack, resultValue);
                        }
                        break;
                    default:
                        result = new IntError(token, 17);
                        break;
                }
            }
            return result;
        }

        // I/O Libraries

        static private IntError Open(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            switch (target.variableReference.getVariableType())
            {
                case 10:  // File
                    {
                        IntToken filename = (IntToken)Pop(opStack);
                        if (filename != null)
                        {
                            string value = (string)filename.variableReference.getValue();

                            try
                            {
                                UniversalFile uf = new UniversalFile(value);
                                if (uf.ArchiveFilename != "")
                                {
                                    UniversalFileReader ufr = new UniversalFileReader(uf.FullFilename);
                                    if (ufr.findEntry(uf.ArchiveFilename))
                                    {
                                        try
                                        {
                                            Stream instream = ufr.CurrentStream.OpenCurrentEntry();
                                            IntToken resultValue = new IntToken("<tmp>", IntConstants.INTEGER);
                                            resultValue.variableReference = new VarFile();
                                            resultValue.variableReference.setValue(instream);
                                            Push(opStack, resultValue);
                                        }
                                        catch (Exception)
                                        {
                                            result = new IntError(token, 33);
                                        }
                                    }
                                }
                                else
                                {
                                    FileStream stream = File.Open(value, FileMode.OpenOrCreate);
                                    target.variableReference.setValue(stream);
                                }
                            }
                            catch (Exception)
                            {
                                result = new IntError(token, 25);
                                result.Additional = "Filename" + value;
                            }
                        }
                        else
                        {
                            result = new IntError(token, 17);
                        }
                    }
                    break;
            }
            return result;
        }

        static private IntError Close(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            switch (target.variableReference.getVariableType())
            {
                case 10:
                    {
                        UniversalFileReader tmp = (UniversalFileReader)target.variableReference;
                        try
                        {
                            tmp.Close();
                        }
                        catch (Exception)
                        {
                            result = new IntError(token, 24);
                        }
                    }
                    break;
            }
            return result;
        }

        static private IntError Read(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken count = (IntToken)Pop(opStack);
            IntToken buffer = (IntToken)Pop(opStack);
            IntToken target = (IntToken)Pop(opStack);

            switch (target.variableReference.getVariableType())
            {
                case 10:
                    {
                        UniversalFileReader tmp = (UniversalFileReader)target.variableReference;
                        try
                        {
                            tmp.Close();
                        }
                        catch (Exception)
                        {
                            result = new IntError(token, 24);
                        }
                    }
                    break;
            }
            return result;
        }

        //
        //
        //
        //
        //
        //
        //   AFTER THIS LINE THE FOLLOWING FUNCTIONS ARE ALL APPLICATION SPECIFIC
        //   I.E. CUSTOM ENHANCEMENTS TO THE STANDARD "BASIC" LANGUAGE
        //
        //
        //
        //
        //
        //
        //

        static private IntError TreeGetElement(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree) dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        string value = tmpdyn.getElement(element);
                        
                        IntToken resultValue = new IntToken("<tmp>", IntConstants.STRING);
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(value);
                        Push(opStack, resultValue);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeGetAttribute(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        string value = tmpdyn.getAttribute(element);

                        IntToken resultValue = new IntToken("<tmp>", IntConstants.STRING);
                        resultValue.variableReference = new VarString();
                        resultValue.variableReference.setValue(value);
                        Push(opStack, resultValue);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeRemoveElement(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        tmpdyn.deleteNode(element);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeRemoveAttribute(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        tmpdyn.removeAttribute(element);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeSetElement(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken value = (IntToken)Pop(opStack);
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        string val = (string)value.variableReference.getValue();
                        tmpdyn.setElement(element,val);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeSetAttribute(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken value = (IntToken)Pop(opStack);
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        string val = (string)value.variableReference.getValue();
                        tmpdyn.setAttribute(element, val);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeGetNode(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        int element = (int)target.variableReference.getValue();
                        Tree tmpnode = (Tree)tmpdyn.tree[element];

                        IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                        resultValue.variableReference = new VarTree();
                        resultValue.variableReference.setValue(tmpnode);
                        Push(opStack, resultValue);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeReadXML(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);

            string element = (string)target.variableReference.getValue();
            Tree tmpnode = XMLTree.readXML(element);

            IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
            resultValue.variableReference = new VarTree();
            resultValue.variableReference.setValue(tmpnode);
            Push(opStack, resultValue);

            return result;
        }

        static private IntError TreeWriteXML(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        TreeDataAccess.writeXML(element, tmpdyn, "Data");
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeReadJson(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);

            string element = (string)target.variableReference.getValue();
            Tree tmpnode = TreeDataAccess.readJson(element);

            IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
            resultValue.variableReference = new VarTree();
            resultValue.variableReference.setValue(tmpnode);
            Push(opStack, resultValue);

            return result;
        }

        static private IntError TreeWriteJson(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        TreeDataAccess.writeJson(element, tmpdyn, "Data");
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeAddNode(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            
            IntToken target = (IntToken)Pop(opStack);
            IntToken node = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        Tree tmpnode = (Tree)node.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        tmpdyn.addNode(tmpnode, element);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeFindNode(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken target = (IntToken)Pop(opStack);
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        string element = (string)target.variableReference.getValue();
                        Tree tmpnode = (Tree)tmpdyn.findNode(element);

                        IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                        resultValue.variableReference = new VarTree();
                        resultValue.variableReference.setValue(tmpnode);
                        Push(opStack, resultValue);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeNew(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        dyn.variableReference.setValue(new Tree());
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeDuplicate(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree) dyn.variableReference.getValue();
                        
                        IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                        resultValue.variableReference = new VarTree();
                        resultValue.variableReference.setValue(tmpdyn.Duplicate());
                        Push(opStack, resultValue);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeDispose(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();
                        tmpdyn.dispose();
                        dyn.variableReference.setValue(null);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeGetLeafnames(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();

                        IntToken resultValue = new IntToken("<tmp>", IntConstants.ARRAY);
                        resultValue.variableReference = null;
                        resultValue.array = new ArrayList();

                        foreach (string current in tmpdyn.leafnames)
                        {
                            resultValue.array.Add(current);
                        }

                        Push(opStack, resultValue);
                    }
                    break;
            }
            return result;
        }

        static private IntError TreeGetAttributeNames(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken dyn = (IntToken)Pop(opStack);

            switch (dyn.variableReference.getVariableType())
            {
                case 12:
                    {
                        Tree tmpdyn = (Tree)dyn.variableReference.getValue();

                        IntToken resultValue = new IntToken("<tmp>", IntConstants.ARRAY);
                        resultValue.variableReference = null;
                        resultValue.array = new ArrayList();

                        foreach (string current in tmpdyn.attributes)
                        {
                            resultValue.array.Add(current);
                        }

                        Push(opStack, resultValue);
                    }
                    break;
            }
            return result;
        }

        // Database Functions

        static private IntError DatabaseCreate(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken database = (IntToken)Pop(opStack);
            string file = (string)database.variableReference.getValue();
            
            try
            {
                SQLiteConnection.CreateFile(file);
            }
            catch (Exception)
            {
                result = new IntError(token, 22);
            }

            return result;
        }

        static private IntError DatabaseExecuteSql(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken sqltoken = (IntToken)Pop(opStack);
            IntToken database = (IntToken)Pop(opStack);
            string SQL = (string)sqltoken.variableReference.getValue();
            SQLiteDatabase db = (SQLiteDatabase)database.variableReference.getValue();

            try
            {
                SQLiteTransaction newTrans;
                newTrans = db.dbCursor.BeginTransaction();

                db.ExecuteNonQuery(SQL);

                newTrans.Commit();
            }
            catch (Exception)
            {
                result = new IntError(token, 23);
                result.Additional = "SQL Statement: \"" + SQL + "\"";
            }

            return result;
        }

        static private IntError DatabaseQuery(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken sqltoken = (IntToken)Pop(opStack);
            IntToken database = (IntToken)Pop(opStack);
            string SQL = (string)sqltoken.variableReference.getValue();
            SQLiteDatabase db = (SQLiteDatabase)database.variableReference.getValue();

            try
            {
                DataTable newTable = db.GetDataTable(SQL);

                IntToken table = new IntToken("<tmp-table>", IntConstants.TABLE);
                table.variableReference = new VarTable();
                table.variableReference.setValue(newTable);
                Push(opStack, table);
            }
            catch (Exception)
            {
                result = new IntError(token, 23);
                result.Additional = "SQL Statement: \"" + SQL + "\"";
            }
            return result;
        }

        static private IntError GetRowField(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken location = (IntToken)Pop(opStack);
            IntToken row = (IntToken)Pop(opStack);

            if (location != null)
            {
                if (row != null)
                {
                    int rowindex = (int)location.variableReference.getValue();
                    if (rowindex > -1)
                    {
                        DataRow rowyurboat = (DataRow)row.variableReference.getValue();
                        if (rowyurboat != null)
                        {
                            if (rowindex > rowyurboat.ItemArray.Count())
                            {
                                result = new IntError(token, 29);
                            }
                            else
                            {
                                string tmp = rowyurboat.ItemArray[rowindex].ToString();
                                IntToken value = new IntToken("<tmp>", IntConstants.STRING);
                                value.variableReference.setValue(tmp);
                                Push(opStack, value);
                            }
                        }
                        else
                        {
                            result = new IntError(token, 28);
                        }
                    }
                    else
                    {
                        result = new IntError(token, 9);
                    }
                }
                else
                {
                    result = new IntError(token, 19);
                }
            }
            else
            {
                result = new IntError(token, 19);
            }
            return result;
        }

        static private IntError SetRowField(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken value = (IntToken)Pop(opStack);
            IntToken field = (IntToken)Pop(opStack);
            IntToken row = (IntToken)Pop(opStack);

            if (value != null)
            {
                if (field != null)
                {
                    if (row != null)
                    {
                        int rowfield = (int)field.variableReference.getValue();
                        string rowdata = (string)value.variableReference.getValue();
                        DataRow rowyurboat = (DataRow)row.variableReference.getValue();
                        if (rowyurboat != null)
                        {
                            rowyurboat.ItemArray[rowfield] = rowdata;
                        }
                        else
                        {
                            result = new IntError(token, 19);
                        }
                    }
                    else
                    {
                        result = new IntError(token, 19);
                    }
                }
                else
                {
                    result = new IntError(token, 19);
                }
            }
            else
            {
                result = new IntError(token, 19);
            }
            return result;
        }

        // Registry Functions

        static public IntError loadRegistryKeyValue(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken value = (IntToken)Pop(opStack);

            if (value != null)
            {
                string fullkeyname = (string) value.variableReference.getValue();
                Tree dynresult = null;
                RegistryKey tmpKey = null;

                string hive = "";
                string key = "";
                int locationindex = fullkeyname.IndexOf("\\");
                if (locationindex < 3)
                {
                    IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                    resultValue.variableReference = new VarTree();
                    resultValue.variableReference.setValue(new Tree());
                    Push(opStack, resultValue);
                }
                else
                {
                    hive = fullkeyname.Substring(0, locationindex);
                    key = fullkeyname.Substring(locationindex + 1);

                    if (hive.CompareTo("HKEY_LOCAL_MACHINE") == 0)
                    {
                        tmpKey = Registry.LocalMachine;
                    }

                    if (hive.CompareTo("HKEY_CURRENT_USERS") == 0)
                    {
                        tmpKey = Registry.CurrentUser;
                    }

                    if (hive.CompareTo("HKEY_CLASSES_ROOT") == 0)
                    {
                        tmpKey = Registry.ClassesRoot;
                    }

                    if (hive.CompareTo("HKEY_CURRENT_CONFIG") == 0)
                    {
                        tmpKey = Registry.CurrentConfig;
                    }

                    if (hive.CompareTo("HKEY_USERS") == 0)
                    {
                        tmpKey = Registry.Users;
                    }

                    // SHORTENED

                    if (hive.CompareTo("HKLM") == 0)
                    {
                        tmpKey = Registry.LocalMachine;
                    }

                    if (hive.CompareTo("HKCU") == 0)
                    {
                        tmpKey = Registry.CurrentUser;
                    }

                    if (hive.CompareTo("HKCR") == 0)
                    {
                        tmpKey = Registry.ClassesRoot;
                    }

                    if (hive.CompareTo("HKCC") == 0)
                    {
                        tmpKey = Registry.CurrentConfig;
                    }

                    if (hive.CompareTo("HKU") == 0)
                    {
                        tmpKey = Registry.Users;
                    }

                    if (tmpKey != null)
                    {
                        char[] sep = new char[1];
                        sep[0] = '\\';
                        string[] tmp = key.Split(sep);
                        Boolean okiedokie = true;
                        Boolean ErrorOccured = false;
                        int tmpLength = tmp.Length - 1;

                        for (int i = 0; i < tmpLength; i++)
                        {
                            if (okiedokie)
                            {
                                try
                                {
                                    if (tmpKey == null)
                                    {
                                        okiedokie = false;
                                    }
                                    else
                                    {
                                        RegistryKey nav = tmpKey.OpenSubKey(tmp[i]);
                                        tmpKey.Close();
                                        tmpKey = nav;
                                    }
                                }
                                catch (IOException)
                                {
                                    okiedokie = false;
                                }
                            }
                        }

                        if (okiedokie && (tmpKey != null))
                        {
                            // Process Key

                            Tree newPrivacyValue = new Tree();
                            string updatedKeyname = key;

                            try
                            {
                                Object tmpValue = (Object)tmpKey.GetValue(tmp[tmp.Length - 1]);
                                string invalue = null;
                                string simplification = null;

                                if (tmpValue != null)
                                {
                                    string inType = tmpValue.GetType().ToString();
                                    if (inType.CompareTo("System.String") == 0)
                                    {
                                        invalue = (string)tmpKey.GetValue(tmp[tmp.Length - 1]);
                                    }
                                    else
                                    {
                                        if (inType.CompareTo("System.Byte[]") == 0)
                                        {
                                            invalue = FatumLib.convertBytesTostring((byte[])tmpKey.GetValue(tmp[tmp.Length - 1]));
                                            simplification = simplifier((byte[])tmpKey.GetValue(tmp[tmp.Length - 1]));
                                        }
                                        else
                                        {
                                            if (inType.CompareTo("System.Int32") == 0)
                                            {
                                                int tmpint = (int)tmpKey.GetValue(tmp[tmp.Length - 1]);
                                                invalue = tmpint.ToString();
                                            }
                                        }
                                    }
                                }

                                if (invalue == null)
                                {
                                    RegistryKey anothertmp = tmpKey.OpenSubKey(tmp[tmp.Length - 1]);

                                    if (anothertmp != null)
                                    {
                                        invalue = (string)anothertmp.GetValue("");
                                        updatedKeyname = key + "\\" + tmp[tmp.Length - 1];
                                        anothertmp.Close();
                                    }
                                    else
                                    {
                                        ErrorOccured = true;
                                    }
                                }

                                if (!ErrorOccured)
                                {
                                    newPrivacyValue.setElement("Hive", hive);
                                    newPrivacyValue.setElement("Key", updatedKeyname);
                                    if (invalue == null)
                                    {
                                        newPrivacyValue.setElement("Value", "(null)");
                                    }
                                    else
                                    {
                                        newPrivacyValue.setElement("Value", invalue);
                                    }
                                    if (simplification != null)
                                    {
                                        if (simplification.CompareTo("") != 0) newPrivacyValue.setElement("Simplification", simplification);
                                    }
                                    //newPrivacyValue.setElement("Action", "Investigate");

                                    IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                                    resultValue.variableReference = new VarTree();
                                    resultValue.variableReference.setValue(newPrivacyValue);

                                    dynresult = newPrivacyValue;
                                }
                            }
                            catch (InvalidCastException)
                            {
                                result = new IntError(token, 31);
                                newPrivacyValue.dispose();
                                return result;
                            }
                        }
                    }
                    if (tmpKey != null) tmpKey.Close();
                }
            }
            return result;
        }

        static public IntError loadRegistryKey(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken value = (IntToken)Pop(opStack);

            if (value != null)
            {
                RegistryKey tmpKey = null;
                string fullkeyname = (string)value.variableReference.getValue();

                string hive = "";
                string key = "";
                int locationindex = fullkeyname.IndexOf("\\");
                if (locationindex < 3)
                {
                    IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                    resultValue.variableReference = new VarTree();
                    resultValue.variableReference.setValue(new Tree());
                    Push(opStack, resultValue);
                }
                else
                {
                    hive = fullkeyname.Substring(0, locationindex);
                    key = fullkeyname.Substring(locationindex + 1);

                    if (hive.CompareTo("HKEY_LOCAL_MACHINE") == 0)
                    {
                        tmpKey = Registry.LocalMachine;
                    }

                    if (hive.CompareTo("HKEY_CURRENT_USERS") == 0)
                    {
                        tmpKey = Registry.CurrentUser;
                    }

                    if (hive.CompareTo("HKEY_CLASSES_ROOT") == 0)
                    {
                        tmpKey = Registry.ClassesRoot;
                    }

                    if (hive.CompareTo("HKEY_CURRENT_CONFIG") == 0)
                    {
                        tmpKey = Registry.CurrentConfig;
                    }

                    if (hive.CompareTo("HKEY_USERS") == 0)
                    {
                        tmpKey = Registry.Users;
                    }

                    // SHORTENED

                    if (hive.CompareTo("HKLM") == 0)
                    {
                        tmpKey = Registry.LocalMachine;
                    }

                    if (hive.CompareTo("HKCU") == 0)
                    {
                        tmpKey = Registry.CurrentUser;
                    }

                    if (hive.CompareTo("HKCR") == 0)
                    {
                        tmpKey = Registry.ClassesRoot;
                    }

                    if (hive.CompareTo("HKCC") == 0)
                    {
                        tmpKey = Registry.CurrentConfig;
                    }

                    if (hive.CompareTo("HKU") == 0)
                    {
                        tmpKey = Registry.Users;
                    }

                    if (tmpKey != null)
                    {
                        char[] sep = new char[1];
                        sep[0] = '\\';
                        string[] tmp = key.Split(sep);
                        Boolean okiedokie = true;
                        int tmpLength = tmp.Length;

                        for (int i = 0; i < tmpLength; i++)
                        {
                            if (okiedokie)
                            {
                                try
                                {
                                    if (tmpKey == null)
                                    {
                                        okiedokie = false;
                                    }
                                    else
                                    {
                                        RegistryKey nav = tmpKey.OpenSubKey(tmp[i]);
                                        tmpKey.Close();
                                        tmpKey = nav;
                                    }
                                }
                                catch (IOException)
                                {
                                    okiedokie = false;
                                }
                            }
                        }

                        if (okiedokie && (tmpKey != null))
                        {
                            // Process Key

                            string[] allvalues = tmpKey.GetValueNames();
                            Tree Registry_Privacy = new Tree();

                            for (int i = 0; i < allvalues.Length; i++)
                            {
                                Tree newPrivacyValue = new Tree();

                                try
                                {
                                    Object tmpValue = (Object)tmpKey.GetValue(allvalues[i]);
                                    string invalue = null;
                                    string simplification = null;

                                    if (tmpValue != null)
                                    {
                                        string inType = tmpValue.GetType().ToString();
                                        if (inType.CompareTo("System.String") == 0)
                                        {
                                            invalue = (string)tmpKey.GetValue(allvalues[i]);
                                        }
                                        else
                                        {
                                            if (inType.CompareTo("System.Byte[]") == 0)
                                            {
                                                invalue = FatumLib.convertBytesTostring((byte[])tmpKey.GetValue(allvalues[i]));
                                                simplification = simplifier((byte[])tmpKey.GetValue(allvalues[i]));
                                            }
                                            else
                                            {
                                                if (inType.CompareTo("System.Int32") == 0)
                                                {
                                                    int tmpint = (int)tmpKey.GetValue(allvalues[i]);
                                                    invalue = tmpint.ToString();
                                                }
                                            }
                                        }
                                    }

                                    if (invalue != null)
                                    {
                                        newPrivacyValue.setElement("Hive", hive);
                                        newPrivacyValue.setElement("Key", key + "\\" + allvalues[i]);
                                        newPrivacyValue.setElement("Value", invalue);
                                        if (simplification != null)
                                        {
                                            if (simplification.CompareTo("") != 0) newPrivacyValue.setElement("Simplification", simplification);
                                        }
                                        //newPrivacyValue.setElement("Action", "Investigate");
                                        Registry_Privacy.addNode(newPrivacyValue, "Data");
                                    }
                                }
                                catch (InvalidCastException)
                                {
                                    result = new IntError(token, 31);
                                    newPrivacyValue.dispose();
                                    return result;
                                }
                            }

                            IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                            resultValue.variableReference = new VarTree();
                            resultValue.variableReference.setValue(Registry_Privacy);
                            Push(opStack, resultValue);
                        }
                    }
                    if (tmpKey != null) tmpKey.Close();
                }
            }
            return result;
        }

        static public IntError loadSubkeys(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken value = (IntToken)Pop(opStack);

            if (value != null)
            {
                RegistryKey tmpKey = null;
                string fullkeyname = (string)value.variableReference.getValue();

                string hive = "";
                string key = "";
                int locationindex = fullkeyname.IndexOf("\\");
                if (locationindex < 3)
                {
                    IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                    resultValue.variableReference = new VarTree();
                    resultValue.variableReference.setValue(new Tree());
                    Push(opStack, resultValue);
                }
                else
                {
                    hive = fullkeyname.Substring(0, locationindex);
                    key = fullkeyname.Substring(locationindex + 1);

                    if (hive.CompareTo("HKEY_LOCAL_MACHINE") == 0)
                    {
                        tmpKey = Registry.LocalMachine;
                    }

                    if (hive.CompareTo("HKEY_CURRENT_USERS") == 0)
                    {
                        tmpKey = Registry.CurrentUser;
                    }

                    if (hive.CompareTo("HKEY_CLASSES_ROOT") == 0)
                    {
                        tmpKey = Registry.ClassesRoot;
                    }

                    if (hive.CompareTo("HKEY_CURRENT_CONFIG") == 0)
                    {
                        tmpKey = Registry.CurrentConfig;
                    }

                    if (hive.CompareTo("HKEY_USERS") == 0)
                    {
                        tmpKey = Registry.Users;
                    }

                    // SHORTENED

                    if (hive.CompareTo("HKLM") == 0)
                    {
                        tmpKey = Registry.LocalMachine;
                    }

                    if (hive.CompareTo("HKCU") == 0)
                    {
                        tmpKey = Registry.CurrentUser;
                    }

                    if (hive.CompareTo("HKCR") == 0)
                    {
                        tmpKey = Registry.ClassesRoot;
                    }

                    if (hive.CompareTo("HKCC") == 0)
                    {
                        tmpKey = Registry.CurrentConfig;
                    }

                    if (hive.CompareTo("HKU") == 0)
                    {
                        tmpKey = Registry.Users;
                    }

                    if (tmpKey != null)
                    {
                        char[] sep = new char[1];
                        sep[0] = '\\';
                        string[] tmp = key.Split(sep);
                        Boolean okiedokie = true;
                        int tmpLength = tmp.Length;

                        for (int i = 0; i < tmpLength; i++)
                        {
                            if (okiedokie)
                            {
                                try
                                {
                                    if (tmpKey == null)
                                    {
                                        okiedokie = false;
                                    }
                                    else
                                    {
                                        RegistryKey nav = tmpKey.OpenSubKey(tmp[i]);
                                        tmpKey.Close();
                                        tmpKey = nav;
                                    }
                                }
                                catch (IOException)
                                {
                                    okiedokie = false;
                                }
                            }
                        }

                        if (okiedokie && (tmpKey != null))
                        {
                            // Process Key

                            Tree Registry_Privacy = new Tree();

                            try
                            {
                                string[] subkeynames = tmpKey.GetSubKeyNames();
                                for (int x = 0; x < subkeynames.Length; x++)
                                {
                                    Tree newsubkey = new Tree();
                                    newsubkey.setElement("Name", subkeynames[x]);
                                    Registry_Privacy.addNode(newsubkey, "Subkey");
                                }
                            }
                            catch (Exception)
                            {
                                result = new IntError(token, 31);
                                Registry_Privacy.dispose();
                                return result;
                            }

                            IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
                            resultValue.variableReference = new VarTree();
                            resultValue.variableReference.setValue(Registry_Privacy);
                            Push(opStack, resultValue);
                        }
                    }
                    if (tmpKey != null) tmpKey.Close();
                }
            }
            return result;
        }

        static private string simplifier(byte[] inbytes)
        {
            string tmpstring = "";

            if (inbytes != null)
            {
                for (int i = 0; i < inbytes.Length; i++)
                {
                    byte tmpByte = inbytes[i];
                    if (tmpByte > 128)
                    {
                        tmpByte = (byte)(tmpByte - 128);
                    }

                    int tmpInt = (int)tmpByte;

                    if (tmpInt > 31)
                    {
                        if (tmpInt != 127)
                        {
                            char convertedChar = (char)tmpInt;
                            tmpstring = tmpstring + convertedChar;
                        }
                    }
                    else
                    { //  We're going to strip out all non-printables
                        if (tmpInt == 10)
                        {
                            char convertedChar = (char)10;
                            tmpstring = tmpstring + convertedChar;
                        }
                        else
                        {
                            if (tmpInt == 13)
                            {
                                char convertedChar = (char)13;
                                tmpstring = tmpstring + convertedChar;
                            }
                        }
                    }
                }
            }
            return tmpstring;
        }

        static private IntError callForwardMessage(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;

            IntToken msgMetadata = (IntToken)Pop(opStack);
            IntToken msgMessage = (IntToken)Pop(opStack);
            IntToken msgLabel = (IntToken)Pop(opStack);
            IntToken msgCategory = (IntToken)Pop(opStack);
            IntToken msgFlowID = (IntToken)Pop(opStack);
            

            if (msgFlowID == null)
            {
                result = new IntError(token, 19);
            }
            else
            {
                if (runtime.OnDocument != null)
                {
                    ArrayList message = new ArrayList();
                    message.Add(msgMessage.variableReference.getValue());
                    message.Add(msgFlowID.variableReference.getValue());
                    message.Add(msgLabel.variableReference.getValue());
                    message.Add(msgCategory.variableReference.getValue());
                    message.Add(msgMetadata.variableReference.getValue());
                    CallbackEventArgs alarmArgs = new CallbackEventArgs();
                    alarmArgs.Message = message;
                    runtime.OnDocument.Invoke(null, alarmArgs);
                }
            }
            return result;
        }

        //static private IntError sentiment(IntRuntime runtime, ArrayList opStack, IntToken token)
        //{
        //    IntError result = null;
        //    IntToken positive = (IntToken)Pop(opStack);
        //    IntToken negative = (IntToken)Pop(opStack);
        //    IntToken neutral = (IntToken)Pop(opStack);
        //    IntToken compound = (IntToken)Pop(opStack);
        //    IntToken message = (IntToken)Pop(opStack);

        //    if (positive != null)
        //    {
        //        if (negative != null)
        //        {
        //            if (neutral != null)
        //            {
        //                if (compound != null)
        //                {
        //                    if (message != null)
        //                    {
        //                        SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();

        //                        string messagetext = (string)message.variableReference.getValue();
        //                        var results = analyzer.PolarityScores(messagetext);
        //                        positive.variableReference.setValue((float)results.Positive);
        //                        negative.variableReference.setValue((float)results.Negative);
        //                        neutral.variableReference.setValue((float)results.Neutral);
        //                        compound.variableReference.setValue((float)results.Compound);
        //                    }
        //                    else
        //                    {
        //                        result = new IntError(runtime, token, 19);
        //                    }
        //                }
        //                else
        //                {
        //                    result = new IntError(runtime, token, 19);
        //                }
        //            }
        //            else
        //            {
        //                result = new IntError(runtime, token, 19);
        //            }
        //        }
        //        else
        //        {
        //            result = new IntError(runtime, token, 19);
        //        }
        //    }
        //    else
        //    {
        //        result = new IntError(runtime, token, 19);
        //    }
        //    return result;
        //}

        static private IntError nop(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;

            return result;
        }

        static private IntError breakpoint(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;

            return result;
        }

        static private IntError appendToFile(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken text = (IntToken)Pop(opStack);
            IntToken filename = (IntToken)Pop(opStack);


            if (filename != null)
            {
                if (text != null)
                {
                    try
                    {
                        StreamWriter outfile = File.AppendText((string)filename.variableReference.getValue());
                        outfile.Write(text.variableReference.getValue());
                    }
                    catch (Exception)
                    {
                        result = new IntError(token, 19);
                    }
                }
                else
                {
                    result = new IntError(token, 19);
                }
            }
            else
            {
                result = new IntError(token, 19);
            }
            return result;
        }

        static private IntError SQLExecuteQuery(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken fields = (IntToken)Pop(opStack);
            IntToken sql = (IntToken)Pop(opStack);
            IntToken connectionstring = (IntToken)Pop(opStack);
            SqlConnection conn = new SqlConnection(connectionstring.variableReference.ToString());
            SqlCommand comm = new SqlCommand();
            Tree response = new Tree();

            comm.CommandType = CommandType.Text;
            comm.Connection = conn;
            comm.CommandText = sql.variableReference.ToString();

            Tree fielddyn = (Tree)fields.variableReference;
            foreach (string tmpStr in fielddyn.leafnames)
            {
                comm.Parameters.AddWithValue(tmpStr, fielddyn.getElement(tmpStr));
            }
            
            conn.Open();
            SqlDataReader dr = comm.ExecuteReader();

            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Tree row = new Tree();
                        for (int i=0;i<dr.FieldCount;i++)
                        {
                            row.addElement(dr.GetName(i), dr.GetString(i));
                        }
                    }
                }
                else
                {
                    response = new Tree();
                }
                dr.Close();
            }
            catch (Exception)
            {
                response = new Tree();
            }

            IntToken resultValue = new IntToken("<tmp>", IntConstants.Tree);
            resultValue.variableReference = new VarTree();
            resultValue.variableReference.setValue(response);
            Push(opStack, resultValue);

            conn.Close();
            comm.Dispose();

            return result;
        }

        static private IntError SQLExecuteNonQuery(IntRuntime runtime, ArrayList opStack, IntToken token)
        {
            IntError result = null;
            IntToken fields = (IntToken)Pop(opStack);
            IntToken sql = (IntToken)Pop(opStack);
            IntToken connectionstring = (IntToken)Pop(opStack);
            string constring = connectionstring.variableReference.getValue().ToString();
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand comm = new SqlCommand();
            comm.CommandType = CommandType.Text;
            comm.Connection = conn;
            comm.CommandText = sql.variableReference.getValue().ToString();

            Tree fielddyn = (Tree)fields.variableReference.getValue();
            foreach (string tmpStr in fielddyn.leafnames)
            {
                comm.Parameters.AddWithValue(tmpStr, fielddyn.getElement(tmpStr));
            }

            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            comm.Dispose();

            return result;
        }
    }
}
