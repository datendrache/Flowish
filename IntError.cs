//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

namespace Flowish
{
    public class IntError
    {
        public short ErrorType = -1;
        public IntToken lastToken = null;
        public string Additional = "";

        public IntError(IntToken errorAtToken, short errorType)
        {
            ErrorType = errorType;
            lastToken = errorAtToken;
        }

        public static string getErrorMessage(IntError error)
        {
            string ErrorMsg = "Error at line " + error.lastToken.linenumber + " near '" + error.lastToken.text + "': ";

            switch (error.ErrorType)
            {
                case 1: ErrorMsg += "Stack underrun (possibly incomplete statement or too few parameters in function)"; break;
                case 2: ErrorMsg += "DIM without variable name"; break;
                case 3: ErrorMsg += "Cannot perform operation because of value type mismatch"; break;
                case 4: ErrorMsg += "Stack Underflow (possible incomplete expression or internal error)"; break;
                case 5: ErrorMsg += "Missing '(' in expression."; break;
                case 6: ErrorMsg += "Array index provided is neither an INTEGER or BYTE"; break;
                case 7: ErrorMsg += "Incorrectly formatted array reference"; break;
                case 8: ErrorMsg += "Array index out of bounds"; break;
                case 9: ErrorMsg += "Array index cannot be a negative value"; break;
                case 10: ErrorMsg += "Undefined variable referenced"; break;
                case 11: ErrorMsg += "Undefined token in stack"; break;
                case 12: ErrorMsg += "STRING index not of type INTEGER"; break;
                case 13: ErrorMsg += "STRING index out of bounds"; break;
                case 14: ErrorMsg += "If missing condition statement"; break;
                case 15: ErrorMsg += "If condition requires BOOLEAN, INTEGER, or BYTE"; break;
                case 16: ErrorMsg += "Call to undefined function"; break;
                case 17: ErrorMsg += "Not enough parameters for called function"; break;
                case 18: ErrorMsg += "Invalid parameter passed to function"; break;
                case 19: ErrorMsg += "Alarm message does not have proper parameters"; break;
                case 20: ErrorMsg += "Cannot open Database"; break;
                case 21: ErrorMsg += "Error closing Database"; break;
                case 22: ErrorMsg += "Cannot create new Database"; break;
                case 23: ErrorMsg += "Database SQL Query error"; break;
                case 24: ErrorMsg += "Cannot close File"; break;
                case 25: ErrorMsg += "Cannot open File"; break;
                case 26: ErrorMsg += "Cannot open connection because protocol must be either TCP or UDP"; break;
                case 27: ErrorMsg += "Attempt to count a variable that is not a collection"; break;
                case 28: ErrorMsg += "NULL value for Row value"; break;
                case 29: ErrorMsg += "Row index out of bounds"; break;
                case 30: ErrorMsg += "Stack overflow error"; break;
                case 31: ErrorMsg += "Regex parsing error"; break;
                case 32: ErrorMsg += "Invalid Cast Exception loading Registry Key"; break;
                case 33: ErrorMsg += "Input stream could not be opened"; break;
                case 34: ErrorMsg += "Flowish Collection State <null> when trying to forward message"; break;
                case 35: ErrorMsg += "I/O Error (Invalid Filename or No Access to Path)"; break;
            }
            
            if (error.Additional != "")
            {
                ErrorMsg += " Additional Details: " + error.Additional;
            }
            return ErrorMsg;
        }
    }
}
