using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
namespace Dynaman.Models {
    public class DynamoColumn : NotificationObject {
        public DynamoColumn():base() {
            this.Children = new ObservableCollection<DynamoColumn>();
            this.Children.CollectionChanged += (sender, e) => {
                if(e.OldItems != null) {
                    foreach(DynamoColumn item in e.OldItems) {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
                if (e.NewItems != null) {
                    foreach (DynamoColumn item in e.NewItems) {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            };
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            throw new NotImplementedException();
        }

        private string _Name;

        public string Name {
            get {
                return _Name;
            }
            set { 
                if (_Name == value) {
                    return;
                }
                _Name = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<DynamoColumn> _Children;

        public ObservableCollection<DynamoColumn> Children {
            get {
                return _Children;
            }
            set { 
                if (_Children == value) {
                    return;
                }
                _Children = value;
                RaisePropertyChanged();
            }
        }
    }
}
