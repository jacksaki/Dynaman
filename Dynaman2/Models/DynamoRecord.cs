using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman2.Models {
    public class DynamoRecord : NotificationObject {

        public static DynamoRecord Create(DynamoTable table, Dictionary<string, AttributeValue> items) {
            var record = new DynamoRecord(table);
            record.AttributeValues = items;
            record.Initialize();
            return record;
        }
        public Dictionary<string, AttributeValue> AttributeValues {
            get;
            private set;
        }

        private DataTable _Data;

        public DataTable Data {
            get {
                return _Data;
            }
            set { 
                if (_Data == value) {
                    return;
                }
                _Data = value;
                RaisePropertyChanged();
            }
        }


        private DynamoRecordItem _SelectedItem;

        public DynamoRecordItem SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            { 
                if (_SelectedItem == value)
                {
                    return;
                }
                _SelectedItem = value;
                RaisePropertyChanged();
            }
        }

        public void Initialize() {
            foreach (var item in this.AttributeValues.OrderByDescending(x=> this.Table.Keys.Where(y=>y.Name.Equals(x.Key)).Any())) {
                if (item.Value.IsMSet) {
                    foreach (var x in GetMapItems(item.Key,item.Value.M)) {
                        this.Items.Add(x);
                    }
                } else {
                    this.Items.Add(new DynamoRecordItem(this, item.Key, item.Value));
                }
            }
        }
        private IEnumerable<DynamoRecordItem> GetMapItems(string parentColumnName, Dictionary<string, AttributeValue> value) {
            var result = new List<DynamoRecordItem>();
            foreach(var v in value) {
                if (v.Value.IsMSet) {
                    result.AddRange(GetMapItems($"{parentColumnName}.{v.Key}", v.Value.M));
                } else {
                    result.Add(new DynamoRecordItem(this, $"{parentColumnName}.{v.Key}",  v.Value));
                }
            }
            return result;
        }

        private DynamoRecord(DynamoTable table):this() {
            this.Table = table;
        }
        private DynamoRecord() {
            this.Items.CollectionChanged += (sender, e) => {
                if (e.OldItems != null)
                {
                    foreach (DynamoRecordItem item in e.OldItems)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (DynamoRecordItem item in e.NewItems)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            };
        }
        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            RaisePropertyChanged(nameof(Items));
        }

        public ObservableCollection<DynamoRecordItem> Items {
            get;
        } = new ObservableCollection<DynamoRecordItem>();
        public DynamoTable Table {
            get;
        }

    }
}
