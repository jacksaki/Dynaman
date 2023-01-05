using Amazon.DynamoDBv2.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
namespace Dynaman2.Models
{
    public class DynamoRecordCollection:NotificationObject, IEnumerable<DynamoRecord>
    {
        private List<DynamoRecord> _list = new List<DynamoRecord>();
        public void Add(DynamoRecord record)
        {
            _list.Add(record);
        }

        public DynamoRecord this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        private DynamoRecord _SelectedRecord;

        public DynamoRecord SelectedRecord
        {
            get
            {
                return _SelectedRecord;
            }
            set
            { 
                if (_SelectedRecord == value)
                {
                    return;
                }
                _SelectedRecord = value;
                RaisePropertyChanged();
            }
        }
        public void InitData()
        {
            var dt = new DataTable();
            foreach (var col in _list.SelectMany(x => x.Items).Where(x => !x.IsMap).Select(x => x.ColumnName).Distinct())
            {
                var c = new DataColumn(col.Replace(".","").Replace("_", "__"));
                c.Caption = col.Replace("_","__");
                dt.Columns.Add(c);
            }
            foreach (var record in _list)
            {
                var row = dt.NewRow();
                foreach (var col in record.Items.Where(x => !x.IsMap))
                {
                    row[col.ColumnName.Replace(".", "").Replace("_", "__")] = col.Value;
                }

                dt.Rows.Add(row);
            }
            this.Data = new DataView(dt);
        }

        public DataView Data
        {
            get;
            private set;
        }

        public IEnumerator<DynamoRecord> GetEnumerator()
        {
            return ((IEnumerable<DynamoRecord>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }
    }
}
