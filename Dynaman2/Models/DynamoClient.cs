using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman.Models {
    internal class DynamoClient {
        public static AmazonDynamoDBClient GetClient() {
            var conf = new AmazonDynamoDBConfig();
            // Set the endpoint URL
            conf.ServiceURL = "http://localhost:8000";
            return new AmazonDynamoDBClient(conf);
        }
    }
}
