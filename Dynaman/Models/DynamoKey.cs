using Amazon.DynamoDBv2.Model;
using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dynaman.Models {
    public class DynamoKey : NotificationObject {
        public DynamoKey(string name, string type) : base() {
            this.Name = name;
            this.Type = type;
        }
        public string Name {
            get;
        }
        public string Type {
            get;
        }
        public AttributeValue GetAttributeValue() {
            switch (this.Type) {
                case "N":
                    return new AttributeValue() { N = this.QueryString };
                case "S":
                case "HASH":
                    return new AttributeValue() { S = this.QueryString };
                default:
                    throw new ApplicationException($"Type {this.Type} is not supported.");
            }
        }

        private string _QueryString = "";

        public string QueryString {
            get {
                return _QueryString;
            }
            set { 
                if (_QueryString == value) {
                    return;
                }
                _QueryString = value;
                RaisePropertyChanged();
            }
        }

    }
}
