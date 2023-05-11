//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System.Collections;

namespace Flowish
{
    public class IntToken
    {
        public string text = null;
        public short tokentype = IntConstants.UNKNOWN;
        public int linenumber = -1;
        public long arraytype = -1;
        public IntVariable variableReference = null;
        public ArrayList array = null;
        public int advance = -1;
        public IntFunction functionReference = null;

        public IntToken(string tokenText, short tokenType)
        {
            if (tokenText != null)
            {
                switch (tokenType)
                {
                    case IntConstants.STRING:
                        text = tokenText;
                        break;
                    default:
                        text = tokenText.ToLower();
                        break;
                }
            }
            tokentype = tokenType;
        }

        public static string info(IntToken token)
        {
            string result;
            switch (token.tokentype)
            {
                case IntConstants.ARRAY: result = "(Array) " + token.text; break;
                case IntConstants.BYTE: result = "(Byte) " + token.text; break;
                case IntConstants.CHAR: result = "(Char) " + token.text; break;
                case IntConstants.INTEGER: result = "(Integer) " + token.text; break;
                case IntConstants.FLOAT: result = "(Float) " + token.text; break;
                case IntConstants.BOOLEAN: result = "(Boolean) " + token.text; break;
                case IntConstants.STRING: result = "(String) " + token.text; break;
                case IntConstants.DATABASE: result = "(Database) " + token.text; break;
                case IntConstants.TABLE: result = "(Table) " + token.text; break;
                case IntConstants.Tree: result = "(Tree) " + token.text; break;
                case IntConstants.NULL: result = "(Null)"; break;

                default: result = token.text + "[" + token.tokentype.ToString() + "] " ; break;
            }
            return result;
        }

        static public short reservedWordCheck(IntToken token)
        {
            short result = -1;

            if (token.text != null)
            {
                switch (token.text)
                {
                    case "for": result = IntConstants.RES_FOR; break;
                    case "foreach": result = IntConstants.RES_FOREACH; break;
                    case "while": result = IntConstants.RES_WHILE; break;
                    case "repeat": result = IntConstants.RES_REPEAT; break;
                    case "endif": result = IntConstants.RES_ENDIF; break;
                    case "endwhile": result = IntConstants.RES_ENDWHILE; break;
                    case "end": result = IntConstants.RES_END; break;
                    case "if": result = IntConstants.RES_IF; break;
                    case "then": result = IntConstants.RES_THEN; break;
                    case "else": result = IntConstants.RES_ELSE; break;
                    case "elseif": result = IntConstants.RES_ELSEIF; break;
                    case "dim": result = IntConstants.RES_DIM; break;
                    case "rem": result = IntConstants.RES_REM; break;
                    case "goto": result = IntConstants.RES_GOTO; break;
                    case "label": result = IntConstants.RES_LABEL; break;
                    case "return": result = IntConstants.RES_RETURN; break;
                    case "function": result = IntConstants.RES_FUNCTION; break;
                    case "integer": result = IntConstants.RES_INTEGER; break;
                    case "string": result = IntConstants.RES_STRING; break;
                    case "float": result = IntConstants.RES_FLOAT; break;
                    case "boolean": result = IntConstants.RES_BOOLEAN; break;
                    case "as": result = IntConstants.RES_AS; break;
                    case "switch": result = IntConstants.RES_SWITCH; break;
                    case "case": result = IntConstants.RES_CASE; break;
                    case "const": result = IntConstants.RES_CONST; break;
                    case "in": result = IntConstants.RES_IN; break;
                    case "true": result = IntConstants.RES_TRUE; break;
                    case "false": result = IntConstants.RES_FALSE; break;
                    case "byte": result = IntConstants.RES_BYTE; break;
                    case "char": result = IntConstants.RES_CHAR; break;
                    case "and": result = IntConstants.RES_AND; break;
                    case "or": result = IntConstants.RES_OR; break;
                    case "not": result = IntConstants.RES_NOT; break;
                    case "let": result = IntConstants.RES_LET; break;
                    case "next": result = IntConstants.RES_NEXT; break;
                    case "array": result = IntConstants.RES_ARRAY; break;
                    case "of": result = IntConstants.RES_OF; break;
                    case "to": result = IntConstants.RES_TO; break;
                    case "until": result = IntConstants.RES_UNTIL; break;
                    case "database": result = IntConstants.DATABASE; break;
                    case "table": result = IntConstants.TABLE; break;
                    case "Tree": result = IntConstants.Tree; break;
                    case "null": result = IntConstants.NULL; break;
                }
            }
            return result;
        }

        public static void disposeTokens(ArrayList tokenlist)
        {
            foreach (IntToken current in tokenlist)
            {
                current.text = "";
                current.variableReference = null;
                current.linenumber = -1;
                current.tokentype = -1;
            }
        }
    }
}
