using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman.Models
{
    public class DynamoTable
    {
        public string Name
        {
            get;
            private set;
        }
        public async Task InitScanKeysAsync()
        {
            var keys = new List<DynamoKey>();
            using (var client = DynamoClient.GetClient())
            {
                var response = await client.DescribeTableAsync(this.Name);
                foreach (var key in response.Table.KeySchema)
                {
                    keys.Add(new DynamoKey(key.AttributeName, key.KeyType.Value));
                }
            }
            this.Keys = keys.AsReadOnly();
        }

        public IReadOnlyList<DynamoKey> Keys
        {
            get;
            private set;
        }

        public async Task<DynamoRecordCollection> ScanAsync(IEnumerable<DynamoKey> keys)
        {
            var list = new DynamoRecordCollection();
            Dictionary<string, AttributeValue> lastKeyEvaluated = null;
            do
            {
                var request = new ScanRequest
                {
                    TableName = this.Name,
                    Limit = 10,
                    ExclusiveStartKey = lastKeyEvaluated,
                    ExpressionAttributeValues = keys.Where(x => !string.IsNullOrEmpty(x.QueryString)).
                     ToDictionary(x => $":{x.Name}", y => y.GetAttributeValue()),
                    FilterExpression = String.Join(" AND ", keys.Select(x => $"{x.Name} = :{x.Name}")),
                };

                var response = await DynamoClient.GetClient().ScanAsync(request);
                foreach (var item in response.Items)
                {
                    list.Add(DynamoRecord.Create(this, item));
                }
                lastKeyEvaluated = response.LastEvaluatedKey;
            } while (lastKeyEvaluated != null && lastKeyEvaluated.Count != 0);
            list.InitData();
            return list;
        }

        public static async Task<IEnumerable<DynamoTable>> GetAllAsync()
        {
            var result = new List<DynamoTable>();
            using (var client = DynamoClient.GetClient())
            {
                string lastEvaluatedTableName = null;
                do
                {
                    var request = new ListTablesRequest
                    {
                        Limit = 20,
                        ExclusiveStartTableName = lastEvaluatedTableName
                    };
                    var response = await client.ListTablesAsync(request);
                    foreach (var name in response.TableNames)
                    {
                        var table = new DynamoTable()
                        {
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
    }
}
