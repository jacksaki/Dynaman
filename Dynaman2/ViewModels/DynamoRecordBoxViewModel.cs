using Dynaman.Models;
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

namespace Dynaman.ViewModels
{
    public class DynamoRecordBoxViewModel : ViewModel
    {


        private DynamoRecord _Record;

        public DynamoRecord Record
        {
            get
            {
                return _Record;
            }
            set
            { 
                if (_Record == value)
                {
                    return;
                }
                if (_Record != null)
                {
                    _Record.PropertyChanged -= _Record_PropertyChanged;
                }
                _Record = value;
                if (_Record != null)
                {
                    _Record.PropertyChanged += _Record_PropertyChanged;
                }
                AddItemCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        private void _Record_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

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
                AddItemCommand.RaiseCanExecuteChanged();
                RemoveItemCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
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
                RemoveItemCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }


        private ListenerCommand<DynamoRecordItem> _AddItemCommand;

        public ListenerCommand<DynamoRecordItem> AddItemCommand
        {
            get
            {
                if (_AddItemCommand == null)
                {
                    _AddItemCommand = new ListenerCommand<DynamoRecordItem>(AddItem, CanAddItem);
                }
                return _AddItemCommand;
            }
        }

        public bool CanAddItem()
        {
            return this.Table != null && this.Record != null;
        }

        public void AddItem(DynamoRecordItem parameter)
        {
            var newItem = new DynamoRecordItem(this.Record, "**", new Amazon.DynamoDBv2.Model.AttributeValue() { S = "**" });
            if (parameter == null)
            {
                this.Record.Items.Add(newItem);
            }
            else
            {
                var index = this.Record.Items.IndexOf(parameter);
                this.Record.Items.Insert(index, newItem);
            }
        }

        private ListenerCommand<DynamoRecordItem> _RemoveItemCommand;

        public ListenerCommand<DynamoRecordItem> RemoveItemCommand
        {
            get
            {
                if (_RemoveItemCommand == null)
                {
                    _RemoveItemCommand = new ListenerCommand<DynamoRecordItem>(RemoveItem, CanRemoveItem);
                }
                return _RemoveItemCommand;
            }
        }

        public bool CanRemoveItem()
        {
            return this.Table != null && this.SelectedItem != null;
        }

        public void RemoveItem(DynamoRecordItem parameter)
        {
            this.Record.Items.Remove(parameter);
        }


        private ViewModelCommand _SaveCommand;

        public ViewModelCommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new ViewModelCommand(Save, CanSave);
                }
                return _SaveCommand;
            }
        }

        public bool CanSave()
        {
            return this.Table != null && this.Record != null && this.Record.Items != null;
        }

        public void Save()
        {

        }

    }
}
