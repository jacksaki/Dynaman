using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman2.Models {
    public static class AttributeValueExtension {
        public static ColumnTypes GetRecordTypes(this AttributeValue value) {
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
