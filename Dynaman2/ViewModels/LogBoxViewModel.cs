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
    public class LogBoxViewModel : ViewModel
    {

        private ListenerCommand<string> _AddLogCommand;

        public ListenerCommand<string> AddLogCommand
        {
            get
            {
                if (_AddLogCommand == null)
                {
                    _AddLogCommand = new ListenerCommand<string>(AddLog);
                }
                return _AddLogCommand;
            }
        }

        public DispatcherCollection<string> Logs
        {
            get;
        } = new DispatcherCollection<string>(DispatcherHelper.UIDispatcher);

        public void AddLog(string parameter)
        {
            this.Logs.Insert(0, parameter);
            this.SelectedItem = this.Logs.FirstOrDefault();
        }

        private string _SelectedItem;

        public string SelectedItem
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
                RaisePropertyChanged();
            }
        }

    }
}
