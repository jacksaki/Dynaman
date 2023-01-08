using Amazon.DynamoDBv2.Model;
using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman.Models
{
    public class DynamoRecord : NotificationObject
    {
        public static DynamoRecord Create(DynamoTable table, Dictionary<string, AttributeValue> items)
        {
            var record = new DynamoRecord(table);
            record.AttributeValues = items;
            record.Initialize();
            return record;
        }
        public Dictionary<string, AttributeValue> AttributeValues
        {
            get;
            private set;
        }
        public void Initialize()
        {
            foreach (var item in this.AttributeValues.OrderByDescending(x => this.Table.Keys.Where(y => y.Name.Equals(x.Key)).Any()))
            {
                if (item.Value.IsMSet)
                {
                    foreach (var x in GetMapItems(item.Key, item.Value.M))
                    {
                        this.Items.Add(x);
                    }
                }
                else
                {
                    this.Items.Add(new DynamoRecordItem(this, item.Key, item.Value));
                }
            }
        }
        private IEnumerable<DynamoRecordItem> GetMapItems(string parentColumnName, Dictionary<string, AttributeValue> value)
        {
            var result = new List<DynamoRecordItem>();
            foreach (var v in value)
            {
                if (v.Value.IsMSet)
                {
                    result.AddRange(GetMapItems($"{parentColumnName}.{v.Key}", v.Value.M));
                }
                else
                {
                    result.Add(new DynamoRecordItem(this, $"{parentColumnName}.{v.Key}", v.Value));
                }
            }
            return result;
        }

        private DynamoRecord(DynamoTable table)
        {
            this.Table = table;
        }
        public DynamoTable Table
        {
            get;
        }
        public ObservableCollection<DynamoRecordItem> Items
        {
            get;
        } = new ObservableCollection<DynamoRecordItem>();

        public async Task PutItemAsync()
        {
            using(var client = DynamoClient.GetClient())
            {
                var req = new PutItemRequest()
                {
                    TableName = this.Table.Name,
                    Item = GetItem()
                };
                await client.PutItemAsync();
            }
        }
    }
}
