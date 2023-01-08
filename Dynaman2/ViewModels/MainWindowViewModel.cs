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
using System.Net;
using System.Security.Cryptography.Xml;

namespace Dynaman.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
        public void Initialize()
        {
            this.DynamoTableBoxViewModel.Initialize();
        }

        private void AddLog(string log)
        {
            this.LogBoxViewModel.AddLogCommand.Execute(log);
        }

        public MainWindowViewModel() : base()
        {
            this.DynamoTableBoxViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(DynamoTableBoxViewModel.SelectedTable)))
                {
                    this.DynamoRecordBoxViewModel.Table = this.DynamoTableBoxViewModel.SelectedTable;
                    SearchCommand.RaiseCanExecuteChanged();
                    RaisePropertyChanged(nameof(SelectedTable));
                }
            };
        }
        public DynamoTableBoxViewModel DynamoTableBoxViewModel
        {
            get;
        } = new DynamoTableBoxViewModel();

        public DynamoTable SelectedTable
        {
            get
            {
                return this.DynamoTableBoxViewModel.SelectedTable;
            }
        }

        public LogBoxViewModel LogBoxViewModel
        {
            get;
        } = new LogBoxViewModel();

        private ViewModelCommand _AddLogCommand;

        public ViewModelCommand AddLogCommand
        {
            get
            {
                if (_AddLogCommand == null)
                {
                    _AddLogCommand = new ViewModelCommand(AddLog);
                }
                return _AddLogCommand;
            }
        }

        public DynamoRecordBoxViewModel DynamoRecordBoxViewModel
        {
            get;
        } = new DynamoRecordBoxViewModel();


        public void AddLog()
        {
            AddLog($"{DateTime.UtcNow:yyyy/MM/dd Hh:mm:ss} hoge");
        }


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
            return this.SelectedTable != null;
        }

        public void Search()
        {
            using(var vm  = new DynamoSearchWindowViewModel(this.SelectedTable))
            {
                var message = new TransitionMessage(vm, "ShowSearchWindow");
                Messenger.Raise(message);
                if (vm.DialogResult == true)
                {
                    this.DynamoRecordBoxViewModel.Record = vm.Record;
                }
            }
        }



    }
}
