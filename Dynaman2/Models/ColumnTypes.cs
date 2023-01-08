using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman.Models {
    public enum ColumnTypes {
        [EnumText("バイナリ")]
        Binary,
        [EnumText("バイナリリスト")]
        BinaryList,
        [EnumText("bool")]
        Bool,
        [EnumText("マップ")]
        Map,
        [EnumText("数値")]
        Number,
        [EnumText("数値リスト")]
        NumberList,
        [EnumText("NULL")]
        Null,
        [EnumText("文字列")]
        String,
        [EnumText("文字列リスト")]
        StringList,
    }
}
