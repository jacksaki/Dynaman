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
            switch (t.ToRecordTypes())
            {
                case RecordTypes.String:
                    return new AttributeValue() { S = "*" };
                case RecordTypes.Binary:
                    return new AttributeValue() { B = new System.IO.MemoryStream() };
                case RecordTypes.BinaryList:
                    return new AttributeValue() { BS = new List<System.IO.MemoryStream>() };
                case RecordTypes.Bool:
                    return new AttributeValue() { BOOL = true };
                case RecordTypes.Number:
                    return new AttributeValue() { N = "0" };
                case RecordTypes.NumberList:
                    return new AttributeValue() { NS = new List<string>() };
                case RecordTypes.StringList:
                    return new AttributeValue() { SS = new List<string>() };
                default:
                    return null;
            }
        }

        public static RecordTypes ToRecordTypes(this string t)
        {
            switch (t)
            {
                case "S":
                    return RecordTypes.String;
                case "N":
                    return RecordTypes.Number;
                case "NS":
                    return RecordTypes.NumberList;
                case "SS":
                    return RecordTypes.StringList;
                case "B":
                    return RecordTypes.Binary;
                case "BS":
                    return RecordTypes.BinaryList;
                case "BOOL":
                    return RecordTypes.Bool;
                case "M":
                    return RecordTypes.Map;
                default:
                    return RecordTypes.String;
            }
        }

        public static RecordTypes GetRecordTypes(this AttributeValue value) {
            if (value.IsBOOLSet) {
                return RecordTypes.Bool;
            }
            if (value.IsMSet) {
                return RecordTypes.Map;
            }
            if (value.NS != null && value.NS.Any()) {
                return RecordTypes.NumberList;
            }
            if(value.SS != null && value.SS.Any()) {
                return RecordTypes.StringList;
            }
            if(value.BS != null && value.BS.Any()) {
                return RecordTypes.BinaryList;
            }
            if(!string.IsNullOrEmpty(value.S)) {
                return RecordTypes.String;
            }
            if(value.N != null) {
                return RecordTypes.Number;
            }
            return RecordTypes.Null;
        }
    }
}
