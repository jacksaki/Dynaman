using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman2.Models
{
    public interface IDynamoClient
    {
        static AmazonDynamoDBClient GetClient();
    }
}
