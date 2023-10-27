using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.ViewModel
{
    public class SettingsPanelViewModel : ViewModelBase, INotifyDataErrorInfo
    {

        private string _minBookCountWarning;
        public string MinBookCountWarning
        {
            get
            {
                return _minBookCountWarning;
            }
            set
            {
                _errorsViewModel.RemoveError(nameof(MinBookCountWarning));
                _minBookCountWarning = value;
                if(int.TryParse(_minBookCountWarning, out int result) && result >= 0)
                {
                    OnPropertyChanged(nameof(MinBookCountWarning));
                    Helper.UpdateMinBookStockNotificationJson(int.Parse(_minBookCountWarning));
                }
                else
                {
                    _errorsViewModel.AddError(nameof(MinBookCountWarning), "Count must be an integer");
                    OnPropertyChanged(nameof(MinBookCountWarning));
                }
            }
        }



        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        private readonly ErrorsViewModel _errorsViewModel;
        public SettingsPanelViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += _errorsViewModel_ErrorsChanged;

            MinBookCountWarning = Helper.ReturnSettings().LowBookStockWarningCount.ToString();
        }

        private void _errorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
    }
}
