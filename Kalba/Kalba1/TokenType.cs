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
        BracketR = 15,

        CurlyL = 16,
        CurlyR = 17,

        BoxL = 16,
        BoxR = 17,

        Number = 18,

        Value = 21,
        Unknown = 22,

        Comma = 23,
        Dot = 24,
        SemiComma = 25,

        Error = 26,
        Comment = 27,

        LessOrEqual = 28,
        MoreOrEqual = 29,

        Not = 30,
        Negative = 31,
<<<<<<< Updated upstream
        Method = 32
=======
        Method = 32,
        AddMethod = 33,

        If = 34,
        For = 35,
        While = 36,
        Else = 37,
        Elsif = 38,
        Abs = 39,
        Sqrt = 40,
        Print = 41,
        PrintLine = 42
>>>>>>> Stashed changes
    }
}
