using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Dynaman.Models;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using System.Net;

namespace Dynaman.ViewModels {
    public class DynamoBoxViewModel : ViewModel {


        private DynamoTable _Table;

        public DynamoTable Table
        {
            get
            {
                return _Table;
            }
            set
            { 
                if (_Table == value)
                {
                    return;
                }
                _Table = value;
                InitializeItemsCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }


        private DynamoRecord _Record;

        public DynamoRecord Record {
            get {
                return _Record;
            }
            set { 
                if (_Record == value) {
                    return;
                }
                if (value == null)
                {
                    value.PropertyChanged -= Value_PropertyChanged;
                }
                _Record = value;
                if (value != null)
                {
                    value.PropertyChanged += Value_PropertyChanged;
                }
                RaisePropertyChanged();
            }
        }

        private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RegisterCommand.RaiseCanExecuteChanged();
            InitializeItemsCommand.RaiseCanExecuteChanged();
        }

        private ViewModelCommand _AddItemCommand;

        public ViewModelCommand AddItemCommand
        {
            get
            {
                if (_AddItemCommand == null)
                {
                    _AddItemCommand = new ViewModelCommand(AddItem);
                }
                return _AddItemCommand;
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
                RemoveCheckedCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }


        public void AddItem()
        {
            var item = new DynamoRecordItem(this.Record, "", new Amazon.DynamoDBv2.Model.AttributeValue() { S = "*" });
            if (this.SelectedItem == null)
            {
                this.Record.Items.Add(item);
            }
            else
            {
                var index = this.Record.Items.IndexOf(this.SelectedItem);
                this.Record.Items.Insert(index, item);
            }
        }

        private ViewModelCommand _RemoveCheckedCommand;

        public ViewModelCommand RemoveCheckedCommand
        {
            get
            {
                if (_RemoveCheckedCommand == null)
                {
                    _RemoveCheckedCommand = new ViewModelCommand(RemoveChecked, CanRemoveChecked);
                }
                return _RemoveCheckedCommand;
            }
        }

        public bool CanRemoveChecked()
        {
            return this.SelectedItem != null;
        }

        public void RemoveChecked()
        {
            this.Record.Items.Remove(this.SelectedItem);
        }

        private ViewModelCommand _RegisterCommand;

        public ViewModelCommand RegisterCommand
        {
            get
            {
                if (_RegisterCommand == null)
                {
                    _RegisterCommand = new ViewModelCommand(RegisterAsync, CanRegister);
                }
                return _RegisterCommand;
            }
        }

        public bool CanRegister()
        {
            return this.Record?.Items?.Any() == true;
        }

        public async void RegisterAsync()
        {
            using(var client = DynamoClient.GetClient())
            {
                var request = new PutItemRequest()
                {
                    TableName = this.Table.Name,
                     Item = this.Record.GetItem()
                };
                await client.PutItemAsync(request);
            }
        }


        private ViewModelCommand _InitializeItemsCommand;

        public ViewModelCommand InitializeItemsCommand
        {
            get
            {
                if (_InitializeItemsCommand == null)
                {
                    _InitializeItemsCommand = new ViewModelCommand(InitializeItems, CanInitializeItems);
                }
                return _InitializeItemsCommand;
            }
        }

        public bool CanInitializeItems()
        {
            return this.Table != null && this.Record.Items.Any();
        }

        public void InitializeItems()
        {
            this.Record.Items.Clear();
        }
    }
}
