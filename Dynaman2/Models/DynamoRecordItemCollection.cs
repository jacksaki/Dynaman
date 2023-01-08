using Dynaman.Models;
using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Dynaman.Models
{
    public class DynamoRecordItemCollection : NotificationObject
    {
        public DynamoRecordItemCollection(DynamoRecord parent)
        {
            this.Parent = parent;
        }

        public DynamoRecord Parent
        {
            get;
        }
        public ObservableCollection<DynamoRecordItem> Items
        {
            get;
        } = new ObservableCollection<DynamoRecordItem>();
    }
}
