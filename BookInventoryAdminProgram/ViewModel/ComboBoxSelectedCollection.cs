using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.ViewModel
{
	/// <summary>
	/// possibly delete later idk
	/// </summary>
    public class ComboBoxSelectedCollection : ViewModelBase
    {
		private string? _salesOptionSelected;
		public string SalesOptionSelected
		{
			get
			{
				return _salesOptionSelected;
			}
			set
			{
				_salesOptionSelected = value;
				OnPropertyChanged(nameof(SalesOptionSelected));
			}
		}

		private string? _typeOptionSelected;
		public string TypeOptionSelected
		{
			get
			{
				return _typeOptionSelected;
			}
			set
			{
				_typeOptionSelected = value;
				OnPropertyChanged(nameof(TypeOptionSelected));
			}
		}
		private string? _modifierOptionSelected;
		public string ModifierOptionSelected
		{
			get
			{
				return _modifierOptionSelected;
			}
			set
			{
				_modifierOptionSelected = value;
				OnPropertyChanged(nameof(ModifierOptionSelected));
			}
		}
	}

}
