using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using Dynaman2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dynaman2.ViewModels {
    public class ExceptionWindowViewModel : ViewModel {
        public void Initialize() {
        }
        #region Exception変更通知プロパティ
        private Exception _Exception;

        public Exception Exception {
            get {
                return _Exception;
            }
            set {
                if (_Exception == value) {
                    return;
                }
                _Exception = value;
                RaisePropertyChanged(nameof(Exception));
                RaisePropertyChanged(nameof(ErrorText));
            }
        }
        #endregion



        #region ErrorText変更通知プロパティ

        public string ErrorText {
            get {
                if (this.Exception is AggregateException) {
                    return string.Join("\r\n\r\n", ((AggregateException)this.Exception).InnerExceptions.Select(x => $"{x.Message}\r\n\r\n{x.StackTrace}"));
                } else {
                    return $"{this.Exception.Message}\r\n\r\n{this.Exception.StackTrace}";
                }
            }
        }
        #endregion


        #region ContinueCommand
        private ViewModelCommand _ContinueCommand;

        public ViewModelCommand ContinueCommand {
            get {
                if (_ContinueCommand == null) {
                    _ContinueCommand = new ViewModelCommand(Continue, CanContinue);
                }
                return _ContinueCommand;
            }
        }

        public bool CanContinue() {
            return this.EnableContinue;
        }

        public void Continue() {
            this.DialogResult = true;
        }
        #endregion



        #region QuitCommand
        private ViewModelCommand _QuitCommand;

        public ViewModelCommand QuitCommand {
            get {
                if (_QuitCommand == null) {
                    _QuitCommand = new ViewModelCommand(Quit);
                }
                return _QuitCommand;
            }
        }

        public void Quit() {
            this.DialogResult = false;
        }
        #endregion


        #region DialogResult変更通知プロパティ
        private bool? _DialogResult;

        public bool? DialogResult {
            get {
                return _DialogResult;
            }
            private set {
                if (_DialogResult == value) {
                    return;
                }
                _DialogResult = value;
                RaisePropertyChanged(nameof(DialogResult));
            }
        }
        #endregion



        #region EnableContinue変更通知プロパティ
        private bool _EnableContinue;

        public bool EnableContinue {
            get {
                return _EnableContinue;
            }
            set {
                if (_EnableContinue == value) {
                    return;
                }
                _EnableContinue = value;
                RaisePropertyChanged(nameof(EnableContinue));
            }
        }
        #endregion


    }
}
