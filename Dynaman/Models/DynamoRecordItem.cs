using Amazon.DynamoDBv2.Model;
using Livet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman.Models {

    public class DynamoRecordItem : NotificationObject {

        public DynamoRecordItem(DynamoRecord parent, string columnName, AttributeValue value) {
            this.Parent = parent;
            this.ColumnName = columnName;
            this.RecordType = value.GetRecordTypes();
            switch (this.RecordType) {
                case RecordTypes.Binary:
                    this.BinaryValue = value.B.ToArray();
                    break;
                case RecordTypes.BinaryList:
                    this.BinaryListValue = new ObservableCollection<byte[]>();
                    this.BinaryListValue.CollectionChanged += (sender, e) => {
                        RaisePropertyChanged(nameof(BinaryListValue));
                    };
                    foreach (var v in value.BS) {
                        this.BinaryListValue.Add(v.ToArray());
                    }
                    break;
                case RecordTypes.Bool:
                    this.BoolValue = value.BOOL;
                    break;
                case RecordTypes.Number:
                    this.NumberValue = value.N.ToDoubleN();
                    break;
                case RecordTypes.NumberList:
                    this.ListValueText = JsonConvert.SerializeObject(value.NS);
                    break;
                case RecordTypes.String:
                    this.StringValue = value.S;
                    break;
                case RecordTypes.StringList:
                    this.ListValueText = JsonConvert.SerializeObject(value.SS);
                    break;
                case RecordTypes.Map:
                    this.IsMap = true;
                    this.RecordType = RecordTypes.Map;
                    break;
                case RecordTypes.Null:
                    throw new ApplicationException("Null,Map");
            }
        }

        private RecordTypes _RecordType;

        public RecordTypes RecordType {
            get {
                return _RecordType;
            }
            set { 
                if (_RecordType == value) {
                    return;
                }
                _RecordType = value;
                RaisePropertyChanged();
            }
        }


        private bool _IsMap = false;

        public bool IsMap {
            get {
                return _IsMap;
            }
            set { 
                if (_IsMap == value) {
                    return;
                }
                _IsMap = value;
                RaisePropertyChanged();
            }
        }

        public AttributeValue ToAttributeValue()
        {
            var result = new AttributeValue();
            switch (this.RecordType)
            {
                case RecordTypes.Binary:
                    result.B = new System.IO.MemoryStream(this.BinaryValue);
                    break;
                case RecordTypes.BinaryList:
                    result.BS.Clear();
                    result.BS.AddRange(this.BinaryListValue.Select(x => new System.IO.MemoryStream(x)));
                    break;
                case RecordTypes.Bool:
                    result.BOOL = this.BoolValue;
                    break;
                case RecordTypes.Number:
                    result.N = this.NumberValue?.ToString();
                    break;
                case RecordTypes.NumberList:
                    result.NS.Clear();
                    result.NS.AddRange((IEnumerable<string>)JsonConvert.DeserializeObject(this.ListValueText));
                    break;
                case RecordTypes.String:
                    result.S = this.StringValue;
                    break;
                case RecordTypes.StringList:
                    result.SS.Clear();
                    result.SS.AddRange((IEnumerable<string>)JsonConvert.DeserializeObject(this.ListValueText));
                    break;
            }
            return result;
        }
        public object Value
        {
            get
            {
                switch (this.RecordType)
                {
                    case RecordTypes.Binary:
                        return Convert.ToBase64String(this.BinaryValue);
                    case RecordTypes.BinaryList:
                        return JsonConvert.SerializeObject(this.BinaryListValue.Select(x => Convert.ToBase64String(x)));
                    case RecordTypes.Bool:
                        return this.BoolValue;
                    case RecordTypes.Number:
                        return this.NumberValue;
                    case RecordTypes.NumberList:
                        return this.ListValueText;
                    case RecordTypes.String:
                        return this.StringValue;
                    case RecordTypes.StringList:
                        return this.ListValueText;
                    default:
                        return null;
                }
            }
            set
            {
                switch (this.RecordType)
                {
                    case RecordTypes.Binary:
                        this.BinaryValue = Convert.FromBase64String(value?.ToString());
                        break;
                    case RecordTypes.BinaryList:
                        this.BinaryListValue = new ObservableCollection<byte[]>(((IEnumerable<string>)(JsonConvert.DeserializeObject(value?.ToString()))).Select(x => Convert.FromBase64String(x)));
                        break;
                    case RecordTypes.Bool:
                        this.BoolValue = value.ToBoolN() ?? false;
                        break;
                    case RecordTypes.Number:
                        this.NumberValue = value.ToDoubleN();
                        break;
                    case RecordTypes.NumberList:
                        this.ListValueText = value?.ToString();
                        break;
                    case RecordTypes.String:
                        this.StringValue = value?.ToString();
                        break;
                    case RecordTypes.StringList:
                        this.ListValueText = value?.ToString();
                        break;
                }
            }
        }
        public DynamoRecord Parent {
            get;
        }


        private string _ColumnName;

        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            { 
                if (_ColumnName == value)
                {
                    return;
                }
                _ColumnName = value;
                RaisePropertyChanged();
            }
        }

        private bool _BoolValue;

        public bool BoolValue {
            get {
                return _BoolValue;
            }
            set { 
                if (_BoolValue == value) {
                    return;
                }
                _BoolValue = value;
                RaisePropertyChanged();
            }
        }


        private string _StringValue;

        public string StringValue {
            get {
                return _StringValue;
            }
            set {
                if (_StringValue == value) {
                    return;
                }
                _StringValue = value;
                RaisePropertyChanged();
            }
        }

        private string _ListValueText;

        public string ListValueText {
            get {
                return _ListValueText;
            }
            set { 
                if (_ListValueText == value) {
                    return;
                }
                _ListValueText = value;
                RaisePropertyChanged();
            }
        }

        private double? _NumberValue;

        public double? NumberValue {
            get {
                return _NumberValue;
            }
            set { 
                if (_NumberValue == value) {
                    return;
                }
                _NumberValue = value;
                RaisePropertyChanged();
            }
        }

        private byte[] _BinaryValue;

        public byte[] BinaryValue {
            get {
                return _BinaryValue;
            }
            set { 
                if (_BinaryValue == value) {
                    return;
                }
                _BinaryValue = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<byte[]> _BinaryListValue;

        public ObservableCollection<byte[]> BinaryListValue {
            get {
                return _BinaryListValue;
            }
            set { 
                if (_BinaryListValue == value) {
                    return;
                }
                _BinaryListValue = value;
                RaisePropertyChanged();
            }
        }

    }
}