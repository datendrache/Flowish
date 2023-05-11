//   Flowish
//   Copyright (C) 2003-2019 Eric Knight

using System;
using System.Collections;
using FatumCore;

namespace Flowish
{
    public class Interpreter_State
    {
        public String filename;
        public int linenumber;
        public Boolean comparator;
        public String targettype;
        public String target;
        public String command;
        public Tree Query1 = new Tree();
        public Tree Query2 = new Tree();
        public Tree Query3 = new Tree();
        public ArrayList List1 = new ArrayList();
        public ArrayList List2 = new ArrayList();
        public ArrayList List3 = new ArrayList();
        public ArrayList stack = new ArrayList();
        public Tree Dyn1 = new Tree();
        public Tree Dyn2 = new Tree();
        public Tree Dyn3 = new Tree();
        public String Expression;
        public String value;
        public String ErrorMessage;
        public int ErrorNumber;
        public String A, B, C, D;
        public int W, X, Y, Z;
        public String result;
        public String tag1, tag2, tag3, tag4, tag5;

        public ArrayList GosubStack = new ArrayList();

        public String Message;
    }
}
