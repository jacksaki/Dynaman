using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Util;
using Amazon.SecurityToken.Model;
using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xaml;

namespace Dynaman.Models {
    public class DynamoTable : NotificationObject {
        public string Name {
            get;
            private set;
        }
        public async Task InitScanKeysAsync() {
            var keys = new List<DynamoKey>();
            using (var client = DynamoClient.GetClient()) {
                var response = await client.DescribeTableAsync(this.Name);
                foreach (var key in response.Table.KeySchema) {
                    keys.Add(new DynamoKey(key.AttributeName, key.KeyType.Value));
                }
            }
            this.Keys = keys.AsReadOnly();
        }
        public IReadOnlyList<DynamoKey> Keys {
            get;
            private set;
        }

        public async Task<DynamoRecordCollection> ScanAsync(IEnumerable<DynamoKey> keys) {
            var list = new DynamoRecordCollection();
            Dictionary<string, AttributeValue> lastKeyEvaluated = null;
            do {
                var request = new ScanRequest {
                    TableName = this.Name,
                    Limit = 10,
                    ExclusiveStartKey = lastKeyEvaluated,
                    ExpressionAttributeValues = keys.Where(x => !string.IsNullOrEmpty(x.QueryString)).
                     ToDictionary(x => $":{x.Name}", y => y.GetAttributeValue()),
                    FilterExpression = String.Join(" AND ", keys.Select(x=> $"{x.Name} = :{x.Name}")),
                };

                var response = await DynamoClient.GetClient().ScanAsync(request);
                foreach (var item in response.Items) {
                    list.Add(DynamoRecord.Create(this, item));
                }
                lastKeyEvaluated = response.LastEvaluatedKey;
            } while (lastKeyEvaluated != null && lastKeyEvaluated.Count != 0);
            list.InitData();
            return list;
        }

        public static async Task<IEnumerable<DynamoTable>> GetAll() {
            var result = new List<DynamoTable>();
            using (var client = DynamoClient.GetClient()) {
                string lastEvaluatedTableName = null;
                do {
                    // Create a request object to specify optional parameters.
                    var request = new ListTablesRequest {
                        Limit = 10, // Page size.
                        ExclusiveStartTableName = lastEvaluatedTableName
                    };
                    var response = await client.ListTablesAsync(request);
                    foreach(var name in response.TableNames) {
                        var table = new DynamoTable() {
                            Name = name
                        };
                        await table.InitScanKeysAsync();
                        result.Add(table);
                    }
                    lastEvaluatedTableName = response.LastEvaluatedTableName;
                } while (lastEvaluatedTableName != null);
            }
            return result;
        }

        internal DynamoRecord CreateNewRecord()
        {
            var record = DynamoRecord.Create(this, new Dictionary<string, AttributeValue>());
            foreach(var key in this.Keys)
            {
                record.Items.Add(new DynamoRecordItem(record, key.Name, AttributeValueExtension.CreateNew(key.Type)));
            }
            return record;
        }
    }
}
