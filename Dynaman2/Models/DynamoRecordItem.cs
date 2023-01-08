using Amazon.DynamoDBv2.Model;
using Livet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Dynaman.Models
{
    public class DynamoRecordItem : NotificationObject
    {
        public DynamoRecordItem(DynamoRecord parent, string columnName, AttributeValue value)
        {
            this.Parent = parent;
            this.ColumnName = columnName;
            this.ColumnType = value.GetColumnTypes();
            switch (this.ColumnType)
            {
                case ColumnTypes.Binary:
                    this.BinaryValue = value.B.ToArray();
                    break;
                case ColumnTypes.BinaryList:
                    this.BinaryListValue = new ObservableCollection<byte[]>();
                    this.BinaryListValue.CollectionChanged += (sender, e) => {
                        RaisePropertyChanged(nameof(BinaryListValue));
                    };
                    foreach (var v in value.BS)
                    {
                        this.BinaryListValue.Add(v.ToArray());
                    }
                    break;
                case ColumnTypes.Bool:
                    this.BoolValue = value.BOOL;
                    break;
                case ColumnTypes.Number:
                    this.NumberValue = value.N.ToDoubleN();
                    break;
                case ColumnTypes.NumberList:
                    this.ListValueText = JsonConvert.SerializeObject(value.NS);
                    break;
                case ColumnTypes.String:
                    this.StringValue = value.S;
                    break;
                case ColumnTypes.StringList:
                    this.ListValueText = JsonConvert.SerializeObject(value.SS);
                    break;
                case ColumnTypes.Map:
                    this.IsMap = true;
                    this.ColumnType = ColumnTypes.Map;
                    break;
                case ColumnTypes.Null:
                    throw new ApplicationException("Null,Map");
            }
        }
        public DynamoRecord Parent
        {
            get;
            private set;
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

        private ColumnTypes _ColumnType;

        public ColumnTypes ColumnType
        {
            get
            {
                return _ColumnType;
            }
            set
            { 
                if (_ColumnType == value)
                {
                    return;
                }
                _ColumnType = value;
                RaisePropertyChanged();
            }
        }


        private bool _IsMap = false;

        public bool IsMap
        {
            get
            {
                return _IsMap;
            }
            set
            {
                if (_IsMap == value)
                {
                    return;
                }
                _IsMap = value;
                RaisePropertyChanged();
            }
        }

        public object Value
        {
            get
            {
                switch (this.ColumnType)
                {
                    case ColumnTypes.Binary:
                        return Convert.ToBase64String(this.BinaryValue);
                    case ColumnTypes.BinaryList:
                        return JsonConvert.SerializeObject(this.BinaryListValue.Select(x => Convert.ToBase64String(x)));
                    case ColumnTypes.Bool:
                        return this.BoolValue;
                    case ColumnTypes.Number:
                        return this.NumberValue;
                    case ColumnTypes.NumberList:
                        return this.ListValueText;
                    case ColumnTypes.String:
                        return this.StringValue;
                    case ColumnTypes.StringList:
                        return this.ListValueText;
                    default:
                        return null;
                }
            }
            set
            {
                switch (this.ColumnType)
                {
                    case ColumnTypes.Binary:
                        this.BinaryValue = Convert.FromBase64String(value?.ToString());
                        break;
                    case ColumnTypes.BinaryList:
                        this.BinaryListValue = new ObservableCollection<byte[]>(((IEnumerable<string>)(JsonConvert.DeserializeObject(value?.ToString()))).Select(x => Convert.FromBase64String(x)));
                        break;
                    case ColumnTypes.Bool:
                        this.BoolValue = value.ToBoolN() ?? false;
                        break;
                    case ColumnTypes.Number:
                        this.NumberValue = value.ToDoubleN();
                        break;
                    case ColumnTypes.NumberList:
                        this.ListValueText = value?.ToString();
                        break;
                    case ColumnTypes.String:
                        this.StringValue = value?.ToString();
                        break;
                    case ColumnTypes.StringList:
                        this.ListValueText = value?.ToString();
                        break;
                }
            }
        }

        private bool _BoolValue;

        public bool BoolValue
        {
            get
            {
                return _BoolValue;
            }
            set
            {
                if (_BoolValue == value)
                {
                    return;
                }
                _BoolValue = value;
                RaisePropertyChanged();
            }
        }


        private string _StringValue;

        public string StringValue
        {
            get
            {
                return _StringValue;
            }
            set
            {
                if (_StringValue == value)
                {
                    return;
                }
                _StringValue = value;
                RaisePropertyChanged();
            }
        }

        private string _ListValueText;

        public string ListValueText
        {
            get
            {
                return _ListValueText;
            }
            set
            {
                if (_ListValueText == value)
                {
                    return;
                }
                _ListValueText = value;
                RaisePropertyChanged();
            }
        }

        private double? _NumberValue;

        public double? NumberValue
        {
            get
            {
                return _NumberValue;
            }
            set
            {
                if (_NumberValue == value)
                {
                    return;
                }
                _NumberValue = value;
                RaisePropertyChanged();
            }
        }

        private byte[] _BinaryValue;

        public byte[] BinaryValue
        {
            get
            {
                return _BinaryValue;
            }
            set
            {
                if (_BinaryValue == value)
                {
                    return;
                }
                _BinaryValue = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<byte[]> _BinaryListValue;

        public ObservableCollection<byte[]> BinaryListValue
        {
            get
            {
                return _BinaryListValue;
            }
            set
            {
                if (_BinaryListValue == value)
                {
                    return;
                }
                _BinaryListValue = value;
                RaisePropertyChanged();
            }
        }

    }
}
