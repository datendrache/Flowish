//   Flowish
//   Copyright (C) 2003-2019 Eric Knight


namespace Flowish
{
    public class IntConstants
    {
        // Reserved words

        public const short UNKNOWN = 0;
        public const short STRING = 1;
        public const short FUNCTION = 2;
        public const short OPERATOR = 3;
        public const short REFERENCE = 4;
        public const short ARRAY = 5;
        public const short INTEGER = 6;
        public const short FLOAT = 7;
        public const short OBJECT = 8;
        public const short OPEN_PAREN = 9;
        public const short CLOSE_PAREN = 10;
        public const short OPEN_BRACKET = 11;
        public const short CLOSE_BRACKET = 12;
        public const short OPEN_PARAMETER = 13;
        public const short CHAR = 14;
        public const short BYTE = 15;
        public const short CLOSE_PARAMETER = 16;
        public const short SEPERATOR = 17;
        public const short ENDOFSTATEMENT = 18;
        public const short BOOLEAN = 19;

        public const short REF_STRING = 99;   // Indexed array associated with defined string

        public const short ERROR = -1;

        public const short RES_FOR = 20;
        public const short RES_FOREACH = 21;
        public const short RES_WHILE = 22;
        public const short RES_REPEAT = 23;
        public const short RES_ENDIF = 24;
        public const short RES_ENDWHILE = 25;
        public const short RES_END = 26;
        public const short RES_IF = 27;
        public const short RES_THEN = 28;
        public const short RES_ELSE = 29;
        public const short RES_ELSEIF = 30;
        public const short RES_DIM = 31;
        public const short RES_REM = 32;
        public const short RES_GOTO = 33;
        public const short RES_LABEL = 34;
        public const short RES_RETURN = 35;
        public const short RES_FUNCTION = 36;
        public const short RES_INTEGER = 37;
        public const short RES_STRING = 38;
        public const short RES_FLOAT = 39;
        public const short RES_BOOLEAN = 40;
        public const short RES_AS = 41;
        public const short RES_SWITCH = 42;
        public const short RES_CASE = 43;
        public const short RES_CONST = 44;
        public const short RES_IN = 45;
        public const short RES_TRUE = 46;
        public const short RES_FALSE = 47;
        public const short RES_BYTE = 48;
        public const short RES_CHAR = 49;
        public const short RES_AND = 50;
        public const short RES_OR = 51;
        public const short RES_NOT = 52;
        public const short RES_ARRAY = 53;
        public const short RES_LET = 54;
        public const short RES_NEXT = 55;
        public const short RES_OF = 56;
        public const short RES_UNTIL = 57;
        public const short RES_TO = 58;
        // The following are operation declarations

        public const short OP_ADDITION = 500;
        public const short OP_SUBTRACTION = 501;
        public const short OP_MULTIPLICATION = 502;
        public const short OP_DIVISION = 503;
        public const short OP_MODULUS = 504;
        public const short OP_SHIFTLEFT = 505;
        public const short OP_SHIFTRIGHT = 506;

        public const short OP_EQUALS = 507;
        public const short OP_NOTEQUALS = 508;
        public const short OP_LESSTHAN = 509;
        public const short OP_GREATERTHAN = 510;
        public const short OP_LESSTHANEQUALTO = 511;
        public const short OP_GREATERTHANEQUALTO = 512;

        public const short OP_OR = 520;
        public const short OP_AND = 521;
        public const short OP_NOT = 522;
        public const short OP_BITWISE_OR = 523;
        public const short OP_BITWISE_AND = 524;

        public const short OP_ASSIGNMENT = 530;

        // The following are enhancement declarations

        public const short TABLE = 600;
        public const short DATABASE = 601;
        public const short Tree = 602;
        public const short NULL = 603;
    }
}
