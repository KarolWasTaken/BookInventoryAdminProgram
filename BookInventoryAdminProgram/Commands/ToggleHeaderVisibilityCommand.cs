using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookInventoryAdminProgram.Commands
{
    public class ToggleHeaderVisibilityCommand : CommandBase
    {
        private readonly Dictionary<string, bool> _headerVisibility;
        private readonly PropertyChangedEventHandler _propertyChangedHandler;
        public Action<string> OnPropertyChanged { get; }
        /*public ToggleHeaderVisibilityCommand(Dictionary<string, bool> headerVisibility, PropertyChangedEventHandler propertyChangedHandler)
        {
            _headerVisibility = headerVisibility;
            _propertyChangedHandler = propertyChangedHandler;
        }*/

        public ToggleHeaderVisibilityCommand(Dictionary<string, bool>? headerVisibility, Action<string> onPropertyChanged)
        {
            _headerVisibility = headerVisibility;
            OnPropertyChanged = onPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is string headerName)
            {
                if (_headerVisibility.ContainsKey(headerName))
                {
                    _headerVisibility[headerName] = !_headerVisibility[headerName];
                    //_propertyChangedHandler.Invoke(_headerVisibility, new PropertyChangedEventArgs(nameof(_headerVisibility)));
                    OnPropertyChanged.Invoke(nameof(_headerVisibility));
                }
            }
        }
    }
}
