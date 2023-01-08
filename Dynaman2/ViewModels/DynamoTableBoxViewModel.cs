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
using Dynaman.Models;
namespace Dynaman.ViewModels
{
    public class DynamoTableBoxViewModel : ViewModel
    {
        public DynamoTableBoxViewModel() : base()
        {
            this.Tables = new ObservableCollection<DynamoTable>();
        }
        
        public async void Initialize()
        {
            this.Tables.Clear();
            foreach(var table in await DynamoTable.GetAllAsync())
            {
                this.Tables.Add(table);
            }
        }
        private DynamoTable _SelectedTable;

        public DynamoTable SelectedTable
        {
            get
            {
                return _SelectedTable;
            }
            set
            { 
                if (_SelectedTable == value)
                {
                    return;
                }
                _SelectedTable = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<DynamoTable> Tables
        {
            get;
        }
    }
}
