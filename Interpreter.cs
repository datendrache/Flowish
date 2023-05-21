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
    public class Interpreter
    {
        public static ArrayList Load(string[] code)
        {
            ArrayList codelist = new ArrayList();

            for (int i = 0; i < code.Length; i++)
            {
                codelist.Add(code[i]);
            }

            return codelist;
        }

        public static ArrayList Compile(string code)
        {
            ArrayList Code;
            ArrayList result = new ArrayList();

            char[] sep = { '\n' };
            string[] lines = code.Split(sep);
            Code = Load(lines);

            short linenumber = 1;

            foreach (string current in Code)
            {
                ArrayList tokens = parseInstruction(current, linenumber);
                result.Add(tokens);
                linenumber++;
            }

            return result;
        }

        private static ArrayList parseInstruction(string line, short linenumber)
        {
            ArrayList newList = new ArrayList();

            String temp;
            int start = -1;
            Boolean stringflag = false;
            Boolean commented = false;
            int startofcomment = -1;

            int lineLength = line.Length;

            for (int i = 0; i < lineLength; i++)
            {
                char eval = line[i];

                if (eval == '#' && stringflag == false)
                {
                    commented = true;
                    startofcomment = i;
                    i = lineLength;
                }
                else
                {
                    if (start == -1)
                    {
                        if (!Char.IsWhiteSpace(eval))
                        {
                            start = i;
                            if (eval == '\"')
                            {
                                stringflag = true;
                            }
                            switch (eval)
                            {
                                case '[':
                                    {
                                        IntToken newToken;
                                        temp = "" + eval;
                                        newToken = new IntToken(temp, IntConstants.OPEN_BRACKET);
                                        newToken.linenumber = linenumber;
                                        newList.Add(newToken);
                                        start = -1;
                                    }
                                    break;
                                case ']':
                                    {
                                        IntToken newToken;
                                        temp = "" + eval;
                                        newToken = new IntToken(temp, IntConstants.CLOSE_BRACKET);
                                        newToken.linenumber = linenumber;
                                        newList.Add(newToken);
                                        start = -1;
                                    }
                                    break;
                                case '.':
                                    {
                                        IntToken newToken;
                                        temp = "" + eval;
                                        newToken = new IntToken(temp, IntConstants.REFERENCE);
                                        newToken.linenumber = linenumber;
                                        newList.Add(newToken);
                                        start = -1;
                                    }
                                    break;
                                case '(':
                                    {
                                        IntToken newToken;
                                        temp = "" + eval;
                                        newToken = new IntToken(temp, IntConstants.OPEN_PAREN);
                                        newToken.linenumber = linenumber;
                                        newList.Add(newToken);
                                        start = -1;
                                    }
                                    break;
                                case ')':
                                    {
                                        IntToken newToken;
                                        temp = "" + eval;
                                        newToken = new IntToken(temp, IntConstants.CLOSE_PAREN);
                                        newToken.linenumber = linenumber;
                                        newList.Add(newToken);
                                        start = -1;
                                    }
                                    break;

                                case '+':
                                case '-':
                                case '*':
                                case '/':
                                case '=':
                                case '>':
                                case '<':
                                case '|':
                                case '&':
                                case '%':
                                case '^':
                                case '\'':
                                case '!':
                                    {
                                        IntToken newToken;
                                        temp = "" + eval;
                                        newToken = new IntToken(temp, IntConstants.OPERATOR);
                                        newToken.linenumber = linenumber;
                                        newList.Add(newToken);
                                        start = -1;
                                    }
                                    break;
                                case ',':
                                    {
                                        IntToken newToken;
                                        temp = "" + eval;
                                        newToken = new IntToken(temp, IntConstants.SEPERATOR);
                                        newToken.linenumber = linenumber;
                                        newList.Add(newToken);
                                        start = -1;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if ((Char.IsWhiteSpace(eval)) && !stringflag)
                        {
                            temp = line.Substring(start, i - start);
                            IntToken newToken = new IntToken(temp, IntConstants.UNKNOWN);
                            newToken.linenumber = linenumber;
                            newList.Add(newToken);
                            start = -1;
                            stringflag = false;
                        }
                        else
                        {
                            if ((eval == '\"') && stringflag)
                            {
                                temp = line.Substring(start + 1, i - start - 1);
                                IntToken newToken = new IntToken(temp, IntConstants.STRING);
                                newToken.linenumber = linenumber;
                                newList.Add(newToken);
                                stringflag = false;
                                start = -1;
                            }
                            else
                            {
                                if (!stringflag)
                                {
                                    switch (eval)
                                    {
                                        case '[':
                                            {
                                                IntToken newToken;
                                                if (start != -1)
                                                {
                                                    temp = line.Substring(start, i - start);
                                                    newToken = new IntToken(temp, IntConstants.ARRAY);
                                                    newToken.linenumber = linenumber;
                                                    newList.Add(newToken);
                                                }

                                                temp = "" + eval;
                                                newToken = new IntToken(temp, IntConstants.OPEN_BRACKET);
                                                newToken.linenumber = linenumber;
                                                newList.Add(newToken);
                                                start = -1;
                                            }
                                            break;
                                        case ']':
                                            {
                                                IntToken newToken;
                                                if (start != -1)
                                                {
                                                    temp = line.Substring(start, i - start);
                                                    newToken = new IntToken(temp, IntConstants.UNKNOWN);
                                                    newToken.linenumber = linenumber;
                                                    newList.Add(newToken);
                                                }
                                                temp = "" + eval;
                                                newToken = new IntToken(temp, IntConstants.CLOSE_BRACKET);
                                                newToken.linenumber = linenumber;
                                                newList.Add(newToken);
                                                start = -1;
                                            }
                                            break;
                                        case '.':
                                            {
                                                IntToken newToken;
                                                if (start != -1)
                                                {
                                                    temp = line.Substring(start, i - start);
                                                    Boolean numbertest = true;
                                                    foreach (char digit in temp)
                                                    {
                                                        if (!Char.IsDigit(digit))
                                                        {
                                                            numbertest = false;
                                                        }
                                                    }
                                                    if (numbertest)
                                                    {
                                                        newToken = new IntToken(temp, IntConstants.INTEGER);
                                                    }
                                                    else
                                                    {
                                                        newToken = new IntToken(temp, IntConstants.OBJECT);
                                                    }
                                                    newToken.linenumber = linenumber;
                                                    newList.Add(newToken);
                                                }

                                                temp = "" + eval;
                                                newToken = new IntToken(temp, IntConstants.REFERENCE);
                                                newList.Add(newToken);
                                                start = -1;
                                            }
                                            break;
                                        case '(':
                                            {
                                                IntToken newToken;
                                                if (start != -1)
                                                {
                                                    temp = line.Substring(start, i - start);
                                                    newToken = new IntToken(temp, IntConstants.UNKNOWN);
                                                    newToken.linenumber = linenumber;
                                                    newList.Add(newToken);
                                                }

                                                temp = "" + eval;
                                                newToken = new IntToken(temp, IntConstants.OPEN_PAREN);
                                                newToken.linenumber = linenumber;
                                                newList.Add(newToken);
                                                start = -1;
                                            }
                                            break;
                                        case ')':
                                            {
                                                IntToken newToken;
                                                if (start != -1)
                                                {
                                                    temp = line.Substring(start, i - start);
                                                    newToken = new IntToken(temp, IntConstants.UNKNOWN);
                                                    newToken.linenumber = linenumber;
                                                    newList.Add(newToken);
                                                }

                                                temp = "" + eval;
                                                newToken = new IntToken(temp, IntConstants.CLOSE_PAREN);
                                                newToken.linenumber = linenumber;
                                                newList.Add(newToken);
                                                start = -1;
                                            }
                                            break;

                                        case '+':
                                        case '-':
                                        case '*':
                                        case '/':
                                        case '=':
                                        case '>':
                                        case '<':
                                        case '|':
                                        case '&':
                                        case '%':
                                        case '^':
                                        case '\'':
                                        case '!':
                                            {
                                                IntToken newToken;
                                                if (start != -1)
                                                {
                                                    temp = line.Substring(start, i - start);
                                                    newToken = new IntToken(temp, IntConstants.UNKNOWN);
                                                    newToken.linenumber = linenumber;
                                                    newList.Add(newToken);
                                                }

                                                temp = "" + eval;
                                                newToken = new IntToken(temp, IntConstants.OPERATOR);
                                                newToken.linenumber = linenumber;
                                                newList.Add(newToken);
                                                start = -1;
                                            }
                                            break;
                                        case ',':
                                            {
                                                IntToken newToken;
                                                if (start != -1)
                                                {
                                                    temp = line.Substring(start, i - start);
                                                    newToken = new IntToken(temp, IntConstants.UNKNOWN);
                                                    newToken.linenumber = linenumber;
                                                    newList.Add(newToken);
                                                }

                                                temp = "" + eval;
                                                newToken = new IntToken(temp, IntConstants.SEPERATOR);
                                                newToken.linenumber = linenumber;
                                                newList.Add(newToken);
                                                start = -1;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (commented)
            {
                if (start != -1)
                {
                    temp = line.Substring(start, startofcomment - start);
                }
            }
            else
            {
                if (start != -1)
                {
                    temp = line.Substring(start, lineLength - start);
                    IntToken newToken;
                    if (temp.Length == 1)
                    {
                        switch (temp[0])
                        {
                            case ']':
                                {
                                    temp = "" + temp[0];
                                    newToken = new IntToken(temp, IntConstants.CLOSE_BRACKET);
                                }
                                break;
                            case ')':
                                {
                                    temp = "" + temp[0];
                                    newToken = new IntToken(temp, IntConstants.CLOSE_PAREN);
                                }
                                break;
                            case '[':
                            case '.':
                            case '(':
                            case '+':
                            case '-':
                            case '*':
                            case '/':
                            case '=':
                            case '>':
                            case '<':
                            case '!':
                            case '|':
                            case '&':
                            case '^':
                            case '%':
                            case ',':
                                {
                                    temp = "" + temp[0];
                                    newToken = new IntToken(temp, IntConstants.ERROR);
                                }
                                break;
                            default:
                                newToken = new IntToken(temp, IntConstants.UNKNOWN);
                                break;
                        }
                    }
                    else
                    {
                        newToken = new IntToken(temp, IntConstants.UNKNOWN);
                    }
                    newToken.linenumber = linenumber;
                    newList.Add(newToken);
                }
            }

            if (newList.Count > 1)
            {
                IntToken tmp = (IntToken)newList[0];
                tmp.text = tmp.text.ToLower();
            }

            numberCheck(newList);
            combineFloats(newList);
            assembleCharConstants(newList);
            combineOperators(newList);
            assignOperatorConstants(newList);
            assignReservedWords(newList);
            removeReminders(newList);
            assignDim(newList);
            identifyFunctions(newList);
            assignLibraryFunctions(newList);
            assignVariablestoConstants(newList);
            adjustSpecialCharacters(newList);

            return newList;
        }

        private static void adjustSpecialCharacters(ArrayList newList)
        {
            foreach (IntToken current in newList)
            {
                if (current.text != null)
                {
                    if (current.tokentype == IntConstants.STRING || current.tokentype == IntConstants.CHAR)
                    {
                        current.text = current.text.Replace("\\n", "\n");
                        current.text = current.text.Replace("\\t", "\t");
                        current.text = current.text.Replace("\\r", "\r");
                        current.text = current.text.Replace("\\\"", "\"");
                        current.text = current.text.Replace("\\\'", "\'");
                        current.text = current.text.Replace("\\0", "\0");
                        current.text = current.text.Replace("\\b", "\b");
                        current.text = current.text.Replace("\\f", "\f");
                        current.text = current.text.Replace("\\a", "\a");
                        current.text = current.text.Replace("\\v", "\v");
                        current.variableReference.setValue(current.text);
                    }
                }
            }
        }

        private static void assignVariablestoConstants(ArrayList newList)
        {
            foreach (IntToken current in newList)
            {
                if (current.text != null)
                {
                    switch (current.tokentype)
                    {
                        case IntConstants.INTEGER:
                            {
                                current.variableReference = new VarInteger();
                                long value;
                                long.TryParse(current.text, out value);
                                current.variableReference.setValue(value);
                            }
                            break;
                        case IntConstants.BYTE:
                            {
                                current.variableReference = new VarByte();
                                byte value;
                                byte.TryParse(current.text, out value);
                                current.variableReference.setValue(value);
                            }
                            break;
                        case IntConstants.STRING:
                            {
                                current.variableReference = new VarString();
                                current.variableReference.setValue(current.text);
                            }
                            break;
                        case IntConstants.CHAR:
                            {
                                current.variableReference = new VarChar();
                                current.variableReference.setValue(current.text[0]);
                            }
                            break;
                        case IntConstants.FLOAT:
                            {
                                current.variableReference = new VarFloat();
                                float value;
                                float.TryParse(current.text, out value);
                                current.variableReference.setValue(value);
                            }
                            break;
                        case IntConstants.BOOLEAN:
                            {
                                current.variableReference = new VarBoolean();
                                if (current.text.ToLower() == "true")
                                {
                                    current.variableReference.setValue(true);
                                }
                                else
                                {
                                    current.variableReference.setValue(false);
                                }
                            }
                            break;
                        case IntConstants.NULL:
                            {
                                current.variableReference = new VarNull();
                            }
                            break;
                    }
                }
            }
        }
        
        private static void numberCheck(ArrayList list)
        {
            foreach (IntToken current in list)
            {
                if (current.tokentype == IntConstants.UNKNOWN)
                {
                    Boolean numbertest = true;
                    if (current.text != null)
                    {
                        foreach (char digit in current.text)
                        {
                            if (!Char.IsDigit(digit))
                            {
                                numbertest = false;
                            }
                        }
                        if (numbertest)
                        {
                            current.tokentype = IntConstants.INTEGER;
                        }
                    }
                }
            }
        }

        private static void combineFloats(ArrayList list)
        {
            IntToken previous;
            IntToken next = null;
            IntToken current = null;
            Boolean ignorePrevious = false;

            ArrayList newList = new ArrayList();
            if (list.Count > 2)
            {
                for (int i = 1; i < list.Count - 1; i++)
                {
                    previous = (IntToken)list[i - 1];
                    next = (IntToken)list[i + 1];
                    current = (IntToken)list[i];
                    if (current.tokentype == IntConstants.REFERENCE)
                    {
                        if (previous.tokentype == IntConstants.INTEGER && next.tokentype == IntConstants.INTEGER)
                        {
                            if (previous.text != null && next.text != null)
                            {
                                IntToken newToken = new IntToken(previous.text + "." + next.text, IntConstants.FLOAT);
                                newList.Add(newToken);
                                current = null;
                                ignorePrevious = true;
                                next = null;
                                i = i + 1;
                            }
                        }
                        else
                        {
                            if (ignorePrevious)
                            {
                                ignorePrevious = false;
                            }
                            else
                            {
                                newList.Add(previous);    
                            }
                        }
                    }
                    else
                    {
                        if (ignorePrevious)
                        {
                            ignorePrevious = false;
                        }
                        else
                        {
                            newList.Add(previous);
                        }
                    }
                }

                if (current != null) newList.Add(current);
                if (next != null) newList.Add(next);

                list.Clear();
                foreach (IntToken token in newList)
                {
                    list.Add(token);
                }
            }
        }

        private static void assembleCharConstants(ArrayList list)
        {
            IntToken previous;
            IntToken next = null;
            IntToken current = null;

            ArrayList newList = new ArrayList();

            if (list.Count > 2)
            {
                for (int i = 1; i < list.Count - 1; i++)
                {
                    previous = (IntToken)list[i - 1];
                    next = (IntToken)list[i + 1];
                    current = (IntToken)list[i];

                    if (previous.tokentype == IntConstants.OPERATOR && next.tokentype == IntConstants.OPERATOR)
                    {
                        if ((previous.text == "'") && (next.text == "'"))
                        {
                            IntToken newToken = new IntToken(current.text, IntConstants.CHAR);
                            newList.Add(newToken);
                            current = null;
                            next = null;
                            i += 1;
                        }
                        else
                        {
                            newList.Add(previous);
                        }
                    }
                    else
                    {
                        newList.Add(previous);
                    }
                }

                if (current != null) newList.Add(current);
                if (next != null) newList.Add(next);

                list.Clear();
                foreach (IntToken token in newList)
                {
                    list.Add(token);
                }
            }
        }

        private static void combineOperators(ArrayList list)
        {
            IntToken previous = null;
            IntToken current = null;

            if (list.Count > 1)
            {
                ArrayList newList = new ArrayList();
                previous = (IntToken)list[0];
                
                for (int i = 1; i < list.Count; i++)
                {
                    Boolean adjusted = false;

                    current = (IntToken)list[i];
                    if (previous != null)
                    {
                        if (current.text != null)
                        {
                            if (previous.tokentype == IntConstants.OPERATOR && current.tokentype == IntConstants.OPERATOR)
                            {
                                IntToken tmpPrevious = previous;
                                IntToken tmpCurrent = current;

                                if ((tmpPrevious.text == "=") && (tmpCurrent.text == "="))
                                {
                                    IntToken newToken = new IntToken("==", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "|") && (tmpCurrent.text == "|"))
                                {
                                    IntToken newToken = new IntToken("||", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "&") && (tmpCurrent.text == "&"))
                                {
                                    IntToken newToken = new IntToken("&&", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == ">") && (tmpCurrent.text == ">"))
                                {
                                    IntToken newToken = new IntToken(">>", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "<") && (tmpCurrent.text == "<"))
                                {
                                    IntToken newToken = new IntToken("<<", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "!") && (tmpCurrent.text == "="))
                                {
                                    IntToken newToken = new IntToken("!=", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == ">") && (tmpCurrent.text == "="))
                                {
                                    IntToken newToken = new IntToken(">=", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "<") && (tmpCurrent.text == "="))
                                {
                                    IntToken newToken = new IntToken("<=", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "+") && (tmpCurrent.text == "+"))
                                {
                                    IntToken newToken = new IntToken("++", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "-") && (tmpCurrent.text == "-"))
                                {
                                    IntToken newToken = new IntToken("--", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if ((tmpPrevious.text == "+") && (tmpCurrent.text == "="))
                                {
                                    IntToken newToken = new IntToken("+=", IntConstants.OPERATOR);
                                    newList.Add(newToken);
                                    current = null;
                                    previous = null;
                                    adjusted = true;
                                }

                                if (!adjusted)
                                {
                                    newList.Add(previous);
                                }
                            }
                            else
                            {
                                newList.Add(previous);
                            }
                        }
                    }

                    if (!adjusted)
                    {
                        previous = current;
                    }
                }

                if (current != null) newList.Add(current);

                list.Clear();
                foreach (IntToken token in newList)
                {
                    list.Add(token);
                }
            }
        }

        private static void assignReservedWords(ArrayList newList)
        {
            foreach (IntToken current in newList)
            {
                if (current.tokentype == IntConstants.UNKNOWN)
                {
                    current.tokentype = IntToken.reservedWordCheck(current);
                }
            }
        }

        private static void assignDim(ArrayList newList)
        {
            if (newList.Count>0)
            {
                IntToken current = (IntToken)newList[0];
                if (current.tokentype == IntConstants.RES_DIM)
                {
                    if (newList.Count == 4 || newList.Count == 6)
                    {
                        IntToken variable = (IntToken)newList[1];
                        IntToken as_ref = (IntToken)newList[2];
                        IntToken value = (IntToken)newList[3];

                        if (variable.tokentype == -1)
                        {
                            switch (value.tokentype)
                            {
                                case IntConstants.RES_BYTE:
                                    {
                                        variable.tokentype = IntConstants.BYTE;
                                        variable.variableReference = new VarByte();
                                    }
                                    break;
                                case IntConstants.RES_CHAR:
                                    {
                                        variable.tokentype = IntConstants.CHAR;
                                        variable.variableReference = new VarChar();
                                    }
                                    break;
                                case IntConstants.RES_INTEGER:
                                    {
                                        variable.tokentype = IntConstants.INTEGER;
                                        variable.variableReference = new VarInteger();
                                    }
                                    break;
                                case IntConstants.RES_FLOAT:
                                    {
                                        variable.tokentype = IntConstants.FLOAT;
                                        variable.variableReference = new VarFloat();
                                    }
                                    break;
                                case IntConstants.RES_STRING:
                                    {
                                        variable.tokentype = IntConstants.STRING;
                                        variable.variableReference = new VarString();
                                    }
                                    break;
                                case IntConstants.RES_BOOLEAN:
                                    {
                                        variable.tokentype = IntConstants.BOOLEAN;
                                        variable.variableReference = new VarBoolean();
                                    }
                                    break;
                                case IntConstants.TABLE:
                                    {
                                        variable.tokentype = IntConstants.TABLE;
                                        variable.variableReference = new VarTable();
                                    }
                                    break;
                                case IntConstants.Tree:
                                    {
                                        variable.tokentype = IntConstants.Tree;
                                        variable.variableReference = new VarTree();
                                    }
                                    break;
                                case IntConstants.NULL:
                                    {
                                        variable.tokentype = IntConstants.NULL;
                                        variable.variableReference = new VarNull();
                                    }
                                    break;
                                case IntConstants.RES_ARRAY:
                                    {
                                        if (newList.Count == 6)
                                        {
                                            variable.tokentype = IntConstants.ARRAY;
                                            variable.array = new ArrayList();
                                            IntToken tokentmp = (IntToken)newList[5];

                                            if (tokentmp.text != null)
                                            {
                                                switch (tokentmp.text)
                                                {
                                                    case "string":
                                                        {
                                                            variable.arraytype = 1;
                                                        }
                                                        break;
                                                    case "integer":
                                                        {
                                                            variable.arraytype = 2;
                                                        }
                                                        break;
                                                    case "float":
                                                        {
                                                            variable.arraytype = 3;
                                                        }
                                                        break;
                                                    case "char":
                                                        {
                                                            variable.arraytype = 5;
                                                        }
                                                        break;
                                                    case "byte":
                                                        {
                                                            variable.arraytype = 4;
                                                        }
                                                        break;
                                                    case "boolean":
                                                        {
                                                            variable.arraytype = 6;
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Error:  Improper assignment of ARRAY type
                                        }
                                    }
                                    break;
                            }

                            if (newList.Count == 4)
                            {
                                newList.RemoveRange(2, 2);  // Eliminate tailing "as <type>" because they are no longer needed
                            }
                            else
                            {
                                if (newList.Count == 6)
                                {
                                    newList.RemoveRange(2, 4);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void removeReminders(ArrayList newList)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                IntToken current = (IntToken)newList[i];

                if (current.tokentype == IntConstants.RES_REM)
                {
                    newList.RemoveRange(i, newList.Count - i);
                    i = newList.Count;
                }
            }
        }

        private static void identifyFunctions(ArrayList newList)
        {
            for (int i=0;i<newList.Count;i++)
            {
                IntToken current = (IntToken)newList[i];
                if (current.tokentype==IntConstants.UNKNOWN)
                {
                    if (newList.Count > i + 1)
                    {
                        IntToken parameterCheck = (IntToken)newList[i + 1];
                        if (parameterCheck.tokentype == IntConstants.OPEN_PAREN)
                        {
                            current.tokentype = IntConstants.FUNCTION;
                            parameterCheck.tokentype = IntConstants.OPEN_PARAMETER;
                            for (int x=i+2;x<newList.Count;x++)
                            {
                                IntToken endParameter = (IntToken)newList[x];
                                if (endParameter.tokentype == IntConstants.CLOSE_PAREN)
                                {
                                    endParameter.tokentype = IntConstants.CLOSE_PARAMETER;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (current.tokentype == IntConstants.RES_FUNCTION)
                    {
                        if (newList.Count > i+1)
                        {
                            IntToken functionName = (IntToken)newList[i + 1];
                            functionName.tokentype = IntConstants.FUNCTION;
                        }
                    }
                }
            }
        }

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

        public static ArrayList divideSections(ArrayList list)
        {
            ArrayList compilation = new ArrayList();
            ArrayList sectionStack = new ArrayList();
            ArrayList currentSection = new ArrayList();
            ArrayList currentLine = new ArrayList();

            foreach (ArrayList line in list)
            {
                Boolean applyEOS = true;
                Boolean dropEnd = false;

                if (line.Count == 0) applyEOS = false;

                foreach (IntToken current in line)
                {
                    switch (current.tokentype)
                    {
                        case IntConstants.RES_FUNCTION:
                            {
                                Push(sectionStack, currentSection);
                                currentSection = new ArrayList();
                                applyEOS = false;
                            }
                            break;

                        case IntConstants.RES_END:
                            {
                                Push(currentLine, current);
                                Push(currentSection, currentLine);
                                Push(compilation, currentSection);
                                currentSection = (ArrayList)Pop(sectionStack);
                                applyEOS = false;
                                dropEnd = true;
                            }
                            break;

                        default:
                            {
                                Push(currentLine, current);
                            }
                            break;
                    }
                }

                if (applyEOS)
                {
                    IntToken seperator = new IntToken(";", IntConstants.ENDOFSTATEMENT);
                    Push(currentLine, seperator);
                }

                if (currentLine.Count > 0)
                {
                    if (!dropEnd)
                    {
                        Push(currentSection, currentLine);
                        currentLine = new ArrayList();
                    }
                    else
                    {
                        currentLine = new ArrayList();
                        dropEnd = false;
                    }

                }
            }

            if (currentSection.Count > 0)
            {
                Push(sectionStack, currentSection);
            }

            while (sectionStack.Count > 0)
            {
                ArrayList tmp = (ArrayList)Pop(sectionStack);
                Push(compilation, tmp);
            }

            return compilation;
        }

        public static void assignLibraryFunctions(ArrayList newList)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                IntToken current = (IntToken)newList[i];

                if ((current.tokentype == IntConstants.UNKNOWN) || (current.tokentype == -1))
                {
                    Flowish.locateLibraryFunction(current);
                }
                else
                {
                    
                    if (newList.Count > (i+1))
                    {
                        IntToken lookahead = (IntToken)newList[i + 1];

                        if (lookahead.tokentype == IntConstants.OPEN_PAREN || lookahead.tokentype == IntConstants.OPEN_PARAMETER)
                        {
                            switch (current.tokentype)
                            {
                                case IntConstants.RES_INTEGER:
                                case IntConstants.RES_STRING:
                                case IntConstants.RES_FLOAT:
                                case IntConstants.RES_CHAR:
                                case IntConstants.RES_BYTE:
                                case IntConstants.RES_BOOLEAN:
                                case IntConstants.DATABASE:
                                case IntConstants.TABLE:
                                case IntConstants.Tree:
                                    Flowish.locateLibraryFunction(current);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void assignOperatorConstants(ArrayList newList)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                IntToken current = (IntToken)newList[i];

                if (current.tokentype == IntConstants.OPERATOR)
                {
                    if (current.text != null)
                    {
                        switch (current.text)
                        {
                            case "*": current.tokentype = IntConstants.OP_MULTIPLICATION; break;
                            case "/": current.tokentype = IntConstants.OP_DIVISION; break;
                            case "%": current.tokentype = IntConstants.OP_MODULUS; break;
                            case "+": current.tokentype = IntConstants.OP_ADDITION; break;
                            case "-": current.tokentype = IntConstants.OP_SUBTRACTION; break;
                            case "<<": current.tokentype = IntConstants.OP_SHIFTLEFT; break;
                            case ">>": current.tokentype = IntConstants.OP_SHIFTRIGHT; break;

                            case "==": current.tokentype = IntConstants.OP_EQUALS; break;
                            case "!=": current.tokentype = IntConstants.OP_NOTEQUALS; break;
                            case "<": current.tokentype = IntConstants.OP_LESSTHAN; break;
                            case ">": current.tokentype = IntConstants.OP_GREATERTHAN; break;
                            case "<=": current.tokentype = IntConstants.OP_LESSTHANEQUALTO; break;
                            case ">=": current.tokentype = IntConstants.OP_GREATERTHANEQUALTO; break;

                            case "&&": current.tokentype = IntConstants.OP_AND; break;
                            case "||": current.tokentype = IntConstants.OP_OR; break;
                            case "!": current.tokentype = IntConstants.OP_NOT; break;
                            case "&": current.tokentype = IntConstants.OP_BITWISE_AND; break;
                            case "|": current.tokentype = IntConstants.OP_BITWISE_OR; break;

                            case "=": current.tokentype = IntConstants.OP_ASSIGNMENT; break;
                        }
                    }
                }
            }
        }

        public static IntError Compile(IntAssembly assembly)
        {
            ArrayList compiled = new ArrayList();
            ArrayList outputStack = new ArrayList();
            ArrayList operatorStack = new ArrayList();
            IntError result = null;

            foreach (IntFunction function in assembly.Functions)
            {
                for (int lineNumber = 0; lineNumber < function.compilation.Count; lineNumber++)
                {
                    ArrayList line = (ArrayList)function.compilation[lineNumber];

                    foreach (IntToken current in line)
                    {
                        if (current.tokentype == IntConstants.ENDOFSTATEMENT)
                        {
                            Boolean stackloop = true;

                            while (stackloop)
                            {
                                IntToken remainder = (IntToken)Pop(operatorStack);
                                if (remainder != null)
                                {
                                    outputStack.Add(remainder);
                                }
                                else
                                {
                                    stackloop = false;
                                }
                            }
                            Push(outputStack, current);
                            break;
                        }
                        else
                        {
                            Boolean isValue = false;
                            switch (current.tokentype)
                            {
                                case IntConstants.BYTE:
                                case IntConstants.INTEGER:
                                case IntConstants.BOOLEAN:
                                case IntConstants.STRING:
                                case IntConstants.CHAR:
                                case IntConstants.FLOAT:
                                case IntConstants.DATABASE:
                                case IntConstants.TABLE:
                                case IntConstants.Tree:
                                case IntConstants.NULL:
                                case -1:
                                    isValue = true;
                                    break;
                            }

                            if (isValue)
                            {
                                Push(outputStack, current);
                            }
                            else
                            {
                                Boolean isfunction = false;
                                switch (current.tokentype)
                                {
                                    case IntConstants.FUNCTION:
                                        isfunction = true;
                                        break;
                                    case IntConstants.ARRAY:
                                        isfunction = true;
                                        break;
                                    case IntConstants.REF_STRING:
                                        isfunction = true;
                                        break;
                                }

                                if (((current.tokentype > 19) && (current.tokentype < 100)) || (current.tokentype > 999))
                                {
                                    switch (current.tokentype)
                                    {
                                        case IntConstants.RES_FALSE:
                                        case IntConstants.RES_TRUE:
                                            isfunction = false;
                                            break;
                                        default: isfunction = true;
                                            break;
                                    }
                                }

                                if (isfunction)
                                {
                                    Push(operatorStack, current);
                                }
                                else
                                {
                                    if (current.tokentype == IntConstants.RES_FALSE || current.tokentype == IntConstants.RES_TRUE)
                                    {
                                        current.variableReference = new VarBoolean();
                                        if (current.tokentype == IntConstants.RES_TRUE)
                                        {
                                            current.variableReference.setValue(true);
                                        }
                                        else
                                        {
                                            current.variableReference.setValue(false);
                                        }
                                        current.tokentype = IntConstants.BOOLEAN;
                                        Push(outputStack, current);
                                    }
                                    else
                                    {

                                        if (((current.tokentype > 499) && (current.tokentype < 599)) || (current.tokentype == 0))
                                        {
                                            Boolean repeat = true;
                                            while (repeat)
                                            {
                                                IntToken op2 = (IntToken)Pop(operatorStack);
                                                if (op2 != null)
                                                {
                                                    int precedence1 = getprecedence(current);
                                                    int precedence2 = getprecedence(op2);

                                                    if (isLeftAssociation(current))
                                                    {
                                                        if (precedence1 <= precedence2)
                                                        {
                                                            Push(outputStack, op2);
                                                        }
                                                        else
                                                        {
                                                            Push(operatorStack, op2);
                                                            repeat = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (precedence1 < precedence2)
                                                        {
                                                            Push(outputStack, op2);
                                                        }
                                                        else
                                                        {
                                                            Push(operatorStack, op2);
                                                            repeat = false;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    repeat = false;
                                                }
                                            }
                                            Push(operatorStack, current);
                                        }
                                        else
                                        {
                                            if (current.tokentype == IntConstants.OPEN_PAREN || current.tokentype == IntConstants.OPEN_BRACKET || current.tokentype == IntConstants.OPEN_PARAMETER)
                                            {
                                                Push(operatorStack, current);
                                            }
                                            else
                                            {
                                                if ((current.tokentype == IntConstants.CLOSE_PAREN) || (current.tokentype == IntConstants.CLOSE_BRACKET))
                                                {
                                                    Boolean repeat = true;
                                                    while (repeat)
                                                    {
                                                        IntToken op2 = (IntToken)Pop(operatorStack);
                                                        if (op2 != null)
                                                        {
                                                            if ((op2.tokentype == IntConstants.OPEN_PAREN) || (op2.tokentype == IntConstants.OPEN_BRACKET) || (op2.tokentype == IntConstants.OPEN_PARAMETER))
                                                            {
                                                                IntToken funcTest = (IntToken)Pop(operatorStack);
                                                                Boolean isFunc = false;

                                                                if (funcTest != null)
                                                                {
                                                                    if ((funcTest.tokentype == IntConstants.FUNCTION) || ((funcTest.tokentype > 999) && (funcTest.tokentype < 9000)))
                                                                    {
                                                                        isFunc = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        switch (funcTest.tokentype)
                                                                        {
                                                                            case IntConstants.RES_IF:
                                                                            case IntConstants.RES_WHILE:
                                                                            case IntConstants.RES_ELSEIF:
                                                                            case IntConstants.RES_UNTIL:
                                                                            case IntConstants.RES_FOR:
                                                                            case IntConstants.RES_FOREACH:
                                                                            case IntConstants.RES_SWITCH:
                                                                                isFunc = true;
                                                                                break;

                                                                        }
                                                                    }

                                                                    if (isFunc) { Push(outputStack, funcTest); }
                                                                    else
                                                                    {
                                                                        if (funcTest.array == null)
                                                                        {
                                                                            // It's a string not an array, goodie.
                                                                            //funcTest.tokentype = IntConstants.STRING;
                                                                            Push(outputStack, funcTest);
                                                                        }
                                                                    }
                                                                }
                                                                repeat = false;
                                                            }
                                                            else
                                                            {
                                                                Push(outputStack, op2);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            repeat = false;
                                                            //result = new IntError(current, 4);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Boolean loop = true;

                    while (loop)
                    {
                        IntToken remainder = (IntToken)Pop(operatorStack);
                        if (remainder != null)
                        {
                            outputStack.Add(remainder);
                        }
                        else
                        {
                            loop = false;
                        }
                    }

                    compiled.Add(outputStack);
                    outputStack = new ArrayList();
                    operatorStack.Clear();
                }

                foreach (ArrayList current in function.compilation)
                {
                    current.Clear();
                }

                function.compilation.Clear();
                function.compilation = compiled;
                compiled = new ArrayList();
            }
            return result;
        }

        
        private static Boolean isLeftAssociation(IntToken token)
        {
            Boolean result = false;

            switch (token.tokentype)
            {
                default: break;
            }

            return result;
        }

        private static int getprecedence(IntToken token)
        {
            int result;

            switch (token.tokentype)
            {
                case IntConstants.ENDOFSTATEMENT: result = 0; break;
                case IntConstants.OP_MULTIPLICATION: result = 12; break;
                case IntConstants.OP_DIVISION: result = 12; break;
                case IntConstants.OP_MODULUS: result = 11; break;
                case IntConstants.OP_ADDITION: result = 10; break;
                case IntConstants.OP_SUBTRACTION: result = 10; break;
                case IntConstants.OP_SHIFTLEFT: result = 13; break;
                case IntConstants.OP_SHIFTRIGHT: result = 13; break;

                case IntConstants.OP_EQUALS: result = 5; break;
                case IntConstants.OP_NOTEQUALS: result = 5; break;
                case IntConstants.OP_LESSTHAN: result = 5; break;
                case IntConstants.OP_GREATERTHAN: result = 5; break;
                case IntConstants.OP_LESSTHANEQUALTO: result = 5; break;
                case IntConstants.OP_GREATERTHANEQUALTO: result = 5; break;

                case IntConstants.OP_AND: result = 30; break;
                case IntConstants.OP_OR: result = 30; break;
                case IntConstants.OP_NOT: result = 30; break;
                case IntConstants.OP_BITWISE_AND: result = 31; break;
                case IntConstants.OP_BITWISE_OR: result = 31; break;

                case IntConstants.OP_ASSIGNMENT: result = 2; break;
                case IntConstants.ARRAY:
                case IntConstants.RES_ARRAY:
                case IntConstants.REF_STRING:
                    result = 6;
                    break;
                case IntConstants.OPEN_PARAMETER:
                    result =  3; break;
                case IntConstants.RES_TO:
                    result =  1; break;
                case IntConstants.OPEN_PAREN:
                case IntConstants.OPEN_BRACKET: 
                    result = 4; break;
                case IntConstants.RES_UNTIL:
                    result = 50; break;

                default: result = 9999; break;
            }

            return result;
        }

        public static IntToken locateVariable(IntRuntime runtime, ArrayList varStack, ArrayList parameters, IntToken current)
        {
            for (int i = 0; i < runtime.globalRegexVariables.Count; i++)
            {
                IntToken currentVar = (IntToken)runtime.globalRegexVariables[i];
                if (currentVar.text != null)
                {
                    if (current.text == currentVar.text)
                    {
                        return currentVar;
                    }
                }
            }

            for (int i = 0; i < varStack.Count; i++)
            {
                IntToken currentVar = (IntToken)varStack[i];
                if (current.text != null)
                {
                    if (current.text == currentVar.text)
                    {
                        return currentVar;
                    }
                }
            }

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    IntToken currentVar = (IntToken)parameters[i];
                    if (current.text != null)
                    {
                        if (current.text == currentVar.text)
                        {
                            return currentVar;
                        }
                    }
                }
            }

            for (int i = 0; i < runtime.globalVariables.Count; i++)
            {
                IntToken currentVar = (IntToken)runtime.globalVariables[i];
                if (current.text != null)
                {
                    if (current.text == currentVar.text)
                    {
                        return currentVar;
                    }
                }
            }
            return null;
        }

        public static IntError createParameters(IntRuntime runtime, ArrayList passedParameters, ArrayList parameters, ArrayList parameterDefinition)
        {
            IntError result = null;
            IntToken current = (IntToken)Pop(passedParameters);
            int parmDefCount = 0;

            while (current != null)
            {
                IntToken tmpParm = (IntToken)parameterDefinition[parmDefCount];

                if (tmpParm.tokentype == IntConstants.ARRAY || tmpParm.tokentype == IntConstants.RES_ARRAY)
                {
                    // We'll just pass whole arrays verbatim - no protection from modification
                    parameters.Add(tmpParm);
                }
                else
                {
                    // For tokens we need to create a renamed copy

                    IntToken newToken = new IntToken(tmpParm.text, current.tokentype);
                    if (tmpParm != null)
                    {
                        switch (current.variableReference.getVariableType())
                        {
                            case 1:
                                {
                                    VarString newString = new VarString();
                                    newString.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newString;
                                }
                                break; //string
                            case 2:
                                {
                                    VarInteger newInteger = new VarInteger();
                                    newInteger.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newInteger;
                                }
                                break; //integer
                            case 3:
                                {
                                    VarFloat newFloat = new VarFloat();
                                    newFloat.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newFloat;
                                }
                                break; //float
                            case 4:
                                {
                                    VarChar newChar = new VarChar();
                                    newChar.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newChar;
                                }
                                break; //char
                            case 5:
                                {
                                    VarByte newByte = new VarByte();
                                    newByte.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newByte;
                                }
                                break; //byte
                            case 6:
                                {
                                    VarBoolean newBoolean = new VarBoolean();
                                    newBoolean.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newBoolean;
                                }
                                break; //boolean
                            case 7:
                                {
                                    VarTable newTable = new VarTable();
                                    newTable.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newTable;
                                }
                                break; //table
                            case 12:
                                {
                                    VarTree newTree = new VarTree();
                                    newTree.setValue(current.variableReference.getValue());
                                    newToken.variableReference = newTree;
                                }
                                break; // Tree
                            case 13:
                                {
                                    VarNull newNull = new VarNull();
                                    newToken.variableReference = newNull;
                                }
                                break; // Tree
                        }
                    }
                    else
                    {
                        result = new IntError(current, 18);
                    }
                    parameters.Add(newToken);
                }

                parmDefCount++;
                current = (IntToken)Pop(passedParameters);
            }

            return result;
        }

        public static IntError processRPN(IntRuntime runtime, ArrayList variables, IntFunction funct, ArrayList Parameters, ArrayList importedParameters, ArrayList parentOpStack)
        {
            IntError result = null;

            try
            {
                ArrayList operandStack = new ArrayList();
                ArrayList varStack = null;
                
                Boolean Error = false;

                if (importedParameters != null)
                {
                    if (importedParameters.Count > 0)
                    {
                        result = createParameters(runtime, importedParameters, Parameters, funct.parameters);
                        importedParameters.Clear();
                        if (result != null) Error = true;
                    }
                }

                if (variables == null)
                {
                    varStack = new ArrayList();
                }
                else
                {
                    varStack = variables;
                }

                while ((runtime.lineNumber < funct.compilation.Count) && (result == null))
                {
                    ArrayList currentline = (ArrayList)funct.compilation[runtime.lineNumber];
                    for (int i = 0; i < currentline.Count; i++)
                    {
                        IntToken current = (IntToken)currentline[i];
                        runtime.overflowProtection++;

                        if (!Error)
                        {
                            Boolean value = false;
                            switch (current.tokentype)
                            {
                                case IntConstants.ARRAY:
                                case IntConstants.BYTE:
                                case IntConstants.CHAR:
                                case IntConstants.STRING:
                                case IntConstants.INTEGER:
                                case IntConstants.FLOAT:
                                case IntConstants.BOOLEAN:
                                case IntConstants.TABLE:
                                case IntConstants.DATABASE:
                                case IntConstants.Tree:
                                case IntConstants.UNKNOWN:
                                case IntConstants.NULL:
                                    value = true;
                                    break;
                            }

                            if (value == true)
                            {

                                IntToken resolved = locateVariable(runtime, varStack, Parameters, current);
                                if (resolved != null)
                                {
                                    if (resolved.tokentype != IntConstants.ARRAY)
                                    {
                                        if (current.tokentype == IntConstants.ARRAY)
                                        {
                                            switch (resolved.tokentype)
                                            {
                                                case IntConstants.STRING:
                                                    {
                                                        result = IntOperation.resolveString(runtime, resolved, operandStack);
                                                        if (result != null)
                                                        {
                                                            result.lastToken = current;
                                                            Error = true;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    Push(operandStack, resolved);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Push(operandStack, resolved);
                                        }
                                    }
                                    else
                                    {
                                        result = IntOperation.resolveArray(runtime, varStack, Parameters, resolved, operandStack);
                                        if (result != null)
                                        {
                                            result.lastToken = current;
                                            Error = true;
                                        }
                                    }
                                }
                                else
                                {
                                    Push(operandStack, current);
                                }
                            }
                            else
                            {
                                if (current.tokentype == IntConstants.RES_DIM)
                                {
                                    IntToken varname = (IntToken)Pop(operandStack);

                                    if (varname != null)
                                    {
                                        IntToken resolved = locateVariable(runtime, varStack, Parameters, varname);
                                        if (resolved == null)
                                        {
                                            switch (varname.tokentype)
                                            {
                                                case IntConstants.BYTE:
                                                    {
                                                        VarByte newByte = new VarByte();
                                                        varname.variableReference = newByte;
                                                    }
                                                    break;
                                                case IntConstants.STRING:
                                                    {
                                                        VarString newString = new VarString();
                                                        varname.variableReference = newString;
                                                    }
                                                    break;
                                                case IntConstants.CHAR:
                                                    {
                                                        VarChar newChar = new VarChar();
                                                        varname.variableReference = newChar;
                                                    }
                                                    break;
                                                case IntConstants.FLOAT:
                                                    {
                                                        VarFloat newFloat = new VarFloat();
                                                        varname.variableReference = newFloat;
                                                    }
                                                    break;
                                                case IntConstants.INTEGER:
                                                    {
                                                        VarInteger newInteger = new VarInteger();
                                                        varname.variableReference = newInteger;
                                                    }
                                                    break;
                                                case IntConstants.BOOLEAN:
                                                    {
                                                        VarBoolean newBoolean = new VarBoolean();
                                                        varname.variableReference = newBoolean;
                                                    }
                                                    break;
                                                case IntConstants.Tree:
                                                    {
                                                        VarTree newTree = new VarTree();
                                                        varname.variableReference = newTree;
                                                    }
                                                    break;
                                                case IntConstants.ARRAY:
                                                    {
                                                        varname.variableReference = null;
                                                        varname.array = new ArrayList();
                                                    }
                                                    break;
                                                case IntConstants.NULL:
                                                    {
                                                        varname.variableReference = new VarNull();
                                                    }
                                                    break;
                                            }
                                            if (funct.functionName == "main")
                                            {
                                                runtime.globalVariables.Add(varname);
                                            }
                                            else
                                            {
                                                varStack.Add(varname);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Error = true;
                                        result = new IntError(current, 2);
                                    }
                                }
                                else
                                {
                                    Boolean tokenHandled = false;

                                    switch (current.tokentype)
                                    {
                                        case IntConstants.OP_ASSIGNMENT: result = IntOperation.operationAssignment(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_ADDITION: result = IntOperation.operationAddition(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_SUBTRACTION: result = IntOperation.operationSubtraction(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_MULTIPLICATION: result = IntOperation.operationMultiplication(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_DIVISION: result = IntOperation.operationDivision(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_MODULUS: result = IntOperation.operationModulus(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_SHIFTLEFT: result = IntOperation.operationShiftLeft(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_SHIFTRIGHT: result = IntOperation.operationShiftRight(runtime, operandStack); tokenHandled = true; break;

                                        case IntConstants.OP_EQUALS: result = IntOperation.operationEquals(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_NOTEQUALS: result = IntOperation.operationNotEquals(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_LESSTHANEQUALTO: result = IntOperation.operationLessThanEqualTo(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_GREATERTHANEQUALTO: result = IntOperation.operationGreaterThanEqualTo(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_GREATERTHAN: result = IntOperation.operationGreaterThan(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_LESSTHAN: result = IntOperation.operationLessThan(runtime, operandStack); tokenHandled = true; break;
                                        case IntConstants.OP_NOT: result = IntOperation.operationNot(runtime, operandStack); tokenHandled = true; break;

                                        case IntConstants.RES_IF:
                                            {
                                                result = IntOperation.operationIf(runtime, varStack, current, funct, Parameters, operandStack); tokenHandled = true;
                                            }
                                            break;

                                        case IntConstants.RES_WHILE:
                                            {
                                                result = IntOperation.operationWhile(runtime, varStack, current, funct, Parameters, operandStack); tokenHandled = true;
                                            }
                                            break;

                                        case IntConstants.RES_FOR:
                                            {
                                                result = IntOperation.operationFor(runtime, varStack, current, funct, Parameters, operandStack);
                                                i = currentline.Count;
                                                tokenHandled = true;
                                            }
                                            break;

                                        case IntConstants.RES_UNTIL:
                                            {
                                                result = IntOperation.operationRepeat(runtime, varStack, current, funct, Parameters, operandStack);
                                                i = currentline.Count;
                                                tokenHandled = true;
                                            }
                                            break;

                                        case IntConstants.RES_ENDIF:
                                        case IntConstants.RES_ENDWHILE:
                                        case IntConstants.RES_REPEAT:
                                            tokenHandled = true;
                                            break;

                                        case IntConstants.RES_NEXT:
                                            runtime.lineNumber = (short)funct.compilation.Count;
                                            tokenHandled = true;
                                            break;

                                        case IntConstants.RES_RETURN:
                                            {
                                                IntToken tmpResult = (IntToken)Pop(operandStack);
                                                parentOpStack.Add(tmpResult);
                                                runtime.lineNumber = (short)funct.compilation.Count;
                                            }
                                            tokenHandled = true;
                                            break;

                                        case IntConstants.RES_END:
                                            runtime.lineNumber = (short)funct.compilation.Count;
                                            tokenHandled = true;
                                            break;
                                    }

                                    if (result != null)
                                    {
                                        result.lastToken = current;
                                        Error = true;
                                    }

                                    if (!tokenHandled)
                                    {
                                        //  Check to see if this is part of the basic library

                                        if ((current.tokentype > 999) && (current.tokentype < 2000))
                                        {
                                            Flowish.callLibraryFunction(runtime, operandStack, current);
                                        }
                                        else
                                        {
                                            if (!tokenHandled)
                                            {
                                                switch (current.tokentype)
                                                {
                                                    case IntConstants.ENDOFSTATEMENT:
                                                        break;
                                                    case IntConstants.RES_THEN:
                                                        break;
                                                    case IntConstants.FUNCTION:
                                                        IntFunction callFunction = IntFunction.locateFunction(current, runtime.programAssembly.Functions);
                                                        if (callFunction != null)
                                                        {
                                                            ArrayList newVarStack = new ArrayList();
                                                            ArrayList parameterList = new ArrayList();
                                                            ArrayList newParmList = new ArrayList();
                                                            int pauseLine = runtime.lineNumber;
                                                            runtime.lineNumber = 0;
                                                            int parmcount = 0;
                                                            if (callFunction.parameters != null)
                                                            {
                                                                parmcount = callFunction.parameters.Count;
                                                            }
                                                            else
                                                            {
                                                                parmcount = 0;
                                                            }

                                                            for (int parms = 0; parms < parmcount; parms++)
                                                            {
                                                                IntToken tmpToken = (IntToken)Pop(operandStack);
                                                                if (tmpToken != null)
                                                                {
                                                                    Push(parameterList, tmpToken);
                                                                }
                                                                else
                                                                {
                                                                    result = new IntError(current, 17);
                                                                }
                                                            }

                                                            result = Interpreter.processRPN(runtime, newVarStack, callFunction, newParmList, parameterList, operandStack);
                                                            runtime.lineNumber = pauseLine;
                                                        }
                                                        else
                                                        {
                                                            result = new IntError(current, 16);
                                                        }
                                                        break;
                                                    default:
                                                        IntToken tmpvar = locateVariable(runtime, varStack, Parameters, current);
                                                        if (tmpvar == null)
                                                        {
                                                            Error = true;
                                                            result = new IntError(current, 11);
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Push(operandStack, tmpvar);
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    runtime.lineNumber++;
                }
            }
            catch (Exception xyz)
            {

            }
            return result;
        }
    }
}
