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

using Proliferation.Fatum;

namespace Proliferation.Flowish
{
    public class VarTree : IntVariable
    {
        public Tree setTree = new Tree();

        public void setValue(Object value)
        {
            setTree = (Tree)value;
        }
        public Object getValue()
        {
            return setTree;
        }
        public int getVariableType()
        {
            return 12;
        }
    }
}
