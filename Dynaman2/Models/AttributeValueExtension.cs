using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman.Models {
    public static class AttributeValueExtension {
        public static AttributeValue CreateNew(string t)
        {
            switch (t.ToColumnTypes())
            {
                case ColumnTypes.String:
                    return new AttributeValue() { S = "*" };
                case ColumnTypes.Binary:
                    return new AttributeValue() { B = new System.IO.MemoryStream() };
                case ColumnTypes.BinaryList:
                    return new AttributeValue() { BS = new List<System.IO.MemoryStream>() };
                case ColumnTypes.Bool:
                    return new AttributeValue() { BOOL = true };
                case ColumnTypes.Number:
                    return new AttributeValue() { N = "0" };
                case ColumnTypes.NumberList:
                    return new AttributeValue() { NS = new List<string>() };
                case ColumnTypes.StringList:
                    return new AttributeValue() { SS = new List<string>() };
                default:
                    return null;
            }
        }

        public static ColumnTypes ToColumnTypes(this string t)
        {
            switch (t)
            {
                case "S":
                    return ColumnTypes.String;
                case "N":
                    return ColumnTypes.Number;
                case "NS":
                    return ColumnTypes.NumberList;
                case "SS":
                    return ColumnTypes.StringList;
                case "B":
                    return ColumnTypes.Binary;
                case "BS":
                    return ColumnTypes.BinaryList;
                case "BOOL":
                    return ColumnTypes.Bool;
                case "M":
                    return ColumnTypes.Map;
                default:
                    return ColumnTypes.String;
            }
        }

        public static ColumnTypes GetColumnTypes(this AttributeValue value) {
            if (value.IsBOOLSet) {
                return ColumnTypes.Bool;
            }
            if (value.IsMSet) {
                return ColumnTypes.Map;
            }
            if (value.NS != null && value.NS.Any()) {
                return ColumnTypes.NumberList;
            }
            if(value.SS != null && value.SS.Any()) {
                return ColumnTypes.StringList;
            }
            if(value.BS != null && value.BS.Any()) {
                return ColumnTypes.BinaryList;
            }
            if(!string.IsNullOrEmpty(value.S)) {
                return ColumnTypes.String;
            }
            if(value.N != null) {
                return ColumnTypes.Number;
            }
            return ColumnTypes.Null;
        }
    }
}
