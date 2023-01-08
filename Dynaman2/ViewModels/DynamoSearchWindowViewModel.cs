using Dynaman.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace Dynaman.ViewModels
{
    public class DynamoSearchWindowViewModel : ViewModel
    {
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).

        // This method would be called from View, when ContentRendered event was raised.
        public void Initialize()
        {
        }
        protected override void Dispose(bool disposing)
        {
            foreach (var key in this.Keys)
            {
                key.PropertyChanged -= Key_PropertyChanged;
            }
            base.Dispose(disposing);
        }

        public DynamoSearchWindowViewModel(DynamoTable table) : base()
        {
            this.Table = table;
            this.Keys.Clear();
            foreach (var key in this.Table.Keys)
            {
                key.PropertyChanged += Key_PropertyChanged;
                this.Keys.Add(key);
            }
        }

        private ListenerCommand<int> _SelectedRecordChangedCommand;

        public ListenerCommand<int> SelectedRecordChangedCommand
        {
            get
            {
                if (_SelectedRecordChangedCommand == null)
                {
                    _SelectedRecordChangedCommand = new ListenerCommand<int>(SelectedRecordChanged, CanSelectedRecordChanged);
                }
                return _SelectedRecordChangedCommand;
            }
        }

        public bool CanSelectedRecordChanged()
        {
            return this.ScannedRecords?.Any() == true;
        }

        public void SelectedRecordChanged(int parameter)
        {
            this.Record = this.ScannedRecords[parameter];
        }


        private void Key_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Keys));
            SearchCommand.RaiseCanExecuteChanged();
        }

        public string Title
        {
            get
            {
                return $"検索：{this.Table.Name}";
            }
        }

        public DynamoTable Table
        {
            get;
        }

        public ObservableCollection<DynamoKey> Keys
        {
            get;
        } = new ObservableCollection<DynamoKey>();

        private ViewModelCommand _SearchCommand;

        public ViewModelCommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new ViewModelCommand(Search, CanSearch);
                }
                return _SearchCommand;
            }
        }

        public bool CanSearch()
        {
            return this.Keys.Where(x => !string.IsNullOrEmpty(x.QueryString)).Any();
        }

        public async void Search()
        {
            this.ScannedRecords = await this.Table.ScanAsync(this.Keys);
            RaisePropertyChanged(nameof(ScannedRecords));
        }

        public DynamoRecordCollection ScannedRecords
        {
            get;
            private set;
        } 

        public DynamoRecord Record
        {
            get;
            private set;
        }

        private ListenerCommand<int> _SelectRecordCommand;

        public ListenerCommand<int> SelectRecordCommand
        {
            get
            {
                if (_SelectRecordCommand == null)
                {
                    _SelectRecordCommand = new ListenerCommand<int>(SelectRecord);
                }
                return _SelectRecordCommand;
            }
        }

        public void SelectRecord(int parameter)
        {
            if (parameter < 0)
            {
                return;
            }
            this.Record = this.ScannedRecords[parameter];
            this.DialogResult = true;
        }


        private bool? _DialogResult;

        public bool? DialogResult
        {
            get
            {
                return _DialogResult;
            }
            set
            { 
                if (_DialogResult == value)
                {
                    return;
                }
                _DialogResult = value;
                RaisePropertyChanged();
            }
        }

    }
}
