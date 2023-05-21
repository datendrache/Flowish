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
using Proliferation.Fatum;

namespace Proliferation.Flowish
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
