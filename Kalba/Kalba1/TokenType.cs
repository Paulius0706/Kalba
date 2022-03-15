using System;
using System.Collections.Generic;
using System.Text;

namespace Kalba1
{
    enum TokenType {
        Bool = 0,
        Int = 1,
        Double = 2,
        String = 3,
        Class = 4,

        Add = 5,
        Sub = 6,
        Div = 7,
        Mul = 8,

        Equal = 9,
        NotEqual = 10,
        More = 11,
        Less = 12,

        Ass = 13,

        BracketL = 14,
        BraketR = 15,

        CurlyL = 16,
        CurlyR = 17,

        BoxL = 16,
        BoxR = 17,

        Number = 18,

        Value = 21,
        Unknow = 22,

        Comma = 23,
        Dot = 24,
        SemiComma = 25,

        Error = 26,
        Comment = 27,

        LessOrEqual = 28,
        MoreOrEqual = 29,

        Not = 30,
        Negative = 31,
        method = 32
    }
}
