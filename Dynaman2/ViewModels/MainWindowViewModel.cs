using Dynaman2.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Dynaman2.ViewModels {
    public class MainWindowViewModel : ViewModel {
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public async void Initialize() {
            this.Tables.Clear();
            foreach(var table in await DynamoTable.GetAll()) {
                this.Tables.Add(table);
            }
        }
        public MainWindowViewModel() : base() {
            this.DynamoBoxViewModel = new DynamoBoxViewModel();
            this.DynamoBoxViewModel.PropertyChanged += (sender, e) => {
                RaisePropertyChanged(nameof(DynamoBoxViewModel));
            };
            this.Tables.CollectionChanged += (sender, e) => {
                RaisePropertyChanged(nameof(Tables));
            };
            this.Keys.CollectionChanged += (sender, e) => {
                RaisePropertyChanged(nameof(Keys));
                if (e.OldItems != null) {
                    foreach (DynamoKey item in e.OldItems) {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
                if (e.NewItems != null) {
                    foreach (DynamoKey item in e.NewItems) {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            };
        }
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            SearchCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(Keys));
        }

        private DynamoRecord _SelectedRecord;

        public DynamoRecord SelectedRecord {
            get {
                return _SelectedRecord;
            }
            set { 
                if (_SelectedRecord == value) {
                    return;
                }
                _SelectedRecord = value;
                this.DynamoBoxViewModel.Record = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DynamoTable> Tables {
            get;
            private set;
        } = new ObservableCollection<DynamoTable>();

        public ObservableCollection<DynamoKey> Keys {
            get;
        } = new ObservableCollection<DynamoKey>();

        private DynamoTable _SelectedTable;

        public DynamoTable SelectedTable {
            get {
                return _SelectedTable;
            }
            set { 
                if (_SelectedTable == value) {
                    return;
                }
                this.Records = null;
                _SelectedTable = value;
                this.DynamoBoxViewModel.Table = value;
                SelectedRecordChangedCommand.RaiseCanExecuteChanged();
                SearchCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = "") {
            base.RaisePropertyChanged(propertyName);
            if (propertyName == nameof(SelectedTable)) {
                InitializeKeys();
            }
        }

        private void InitializeKeys() {
            this.Keys.Clear();
            if (this.SelectedTable == null) {
                return;
            }
            
            foreach(var key in this.SelectedTable.Keys) {
                this.Keys.Add(key);
            }
        }

        private DynamoBoxViewModel _DynamoBoxViewModel;

        public DynamoBoxViewModel DynamoBoxViewModel {
            get {
                return _DynamoBoxViewModel;
            }
            private set { 
                if (_DynamoBoxViewModel == value) {
                    return;
                }
                _DynamoBoxViewModel = value;
                RaisePropertyChanged();
            }
        }


        private ViewModelCommand _SearchCommand;

        public ViewModelCommand SearchCommand {
            get {
                if (_SearchCommand == null) {
                    _SearchCommand = new ViewModelCommand(Search, CanSearch);
                }
                return _SearchCommand;
            }
        }

        public bool CanSearch() {
            if(this.SelectedTable == null) {
                return false;
            }
            if (!this.Keys.Any()) {
                return false;
            }

            if(!this.Keys.Where(x=>!string.IsNullOrEmpty(x.QueryString)).Any()) {
                return false;
            }
            return true;
        }


        private DynamoRecordCollection _Records;

        public DynamoRecordCollection Records
        {
            get
            {
                return _Records;
            }
            set
            { 
                if (_Records == value)
                {
                    return;
                }
                if (value == null)
                {
                    _Records.PropertyChanged -= Records_PropertyChanged;
                }
                _Records = value;
                if (value != null)
                {
                    _Records.PropertyChanged += Records_PropertyChanged;
                }
                RaisePropertyChanged();
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
            return this.Records?.Any() == true;
        }

        public void SelectedRecordChanged(int parameter)
        {
            this.SelectedRecord = this.Records[parameter];
        }


        private void Records_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public async void Search() {
            this.Records = await this.SelectedTable.ScanAsync(this.Keys);
            SelectedRecordChangedCommand.RaiseCanExecuteChanged();
        }
    }
}
