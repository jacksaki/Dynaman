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
using Dynaman2.Models;

namespace Dynaman2.ViewModels {
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
                _Record = value;
                RaisePropertyChanged();
            }
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

        public void AddItem()
        {

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
            return true;
        }

        public void RemoveChecked()
        {
        }

        private ViewModelCommand _RegisterCommand;

        public ViewModelCommand RegisterCommand
        {
            get
            {
                if (_RegisterCommand == null)
                {
                    _RegisterCommand = new ViewModelCommand(Register, CanRegister);
                }
                return _RegisterCommand;
            }
        }

        public bool CanRegister()
        {
            return true;
        }

        public void Register()
        {

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
            return this.Table != null;
        }

        public void InitializeItems()
        {

        }

    }
}
