using BookInventoryAdminProgram.Commands;
using BookInventoryAdminProgram.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using static BookInventoryAdminProgram.Model.SalesOperations;
using static BookInventoryAdminProgram.Stores.DatabaseStore;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using static BookInventoryAdminProgram.Model.GraphDataGrabber;
using System.ComponentModel;
using BookInventoryAdminProgram.Model;

namespace BookInventoryAdminProgram.ViewModel
{
    // file too long. Too many properties. refactor this later.
    public class SalesPanelViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        internal List<BookInfo> Database = DatabaseStore.MainDataset; // single points for database to enter incase i need to load async and get a loading screen in here.
        private Dictionary<int, List<SalesQuarter>> AvailableSalesQuarters;
        internal string FileLocation = string.Empty;
        internal Dictionary<string, double> ExpensesDictionary = new Dictionary<string, double>(); 

        public string ComboBoxFieldResetter { get; set; }

        // 0: Year
        // 1: Quarter
        // 2: Date
        private int _selectedInputType;
        public int SelectedInputType
        {
            get { return _selectedInputType; }
            set
            {
                if (_selectedInputType != value)
                {
                    _selectedInputType = value;
                    OnPropertyChanged(nameof(SelectedInputType));
                }
            }
        }
        private List<int> _years;
        public List<int> Years
        {
            get
            {
                return _years;
            }
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }
        private int _selectedYear;
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                //var quarts = GetAvailableSalesQuarters(new DateTime(SelectedYear, 12, 31), new DateTime(SelectedYear, 1, 1))[SelectedYear];
                var quarts = AvailableSalesQuarters[SelectedYear];
                SalesQuarters[SelectedYear] = new List<SalesQuarter>(quarts);
                OnPropertyChanged(nameof(SelectedYear));
                OnPropertyChanged(nameof(SalesQuartersForSelectedYear));
                ResetComboBoxFields();
            }
        }
        
        public Dictionary<int, List<SalesQuarter>> SalesQuarters { get; set; }
        // Derived property returning the list of SalesQuarters for the selected year
        public List<SalesQuarter> SalesQuartersForSelectedYear
        {
            get
            {
                return SalesQuarters.ContainsKey(SelectedYear) ? SalesQuarters[SelectedYear] : new List<SalesQuarter>();
            }
        }
        private string _selectedSalesQuarter;
        public string SelectedSalesQuarter
        {
            get
            {
                return _selectedSalesQuarter;
            }
            set
            {
                _selectedSalesQuarter = value;
                OnPropertyChanged(nameof(SelectedSalesQuarter));
            }
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        // Expenses
        private string _expenseNameTextBoxField;
        public string ExpenseNameTextBoxField
        {
            get
            {
                return _expenseNameTextBoxField;
            }
            set
            {
                _expenseNameTextBoxField = value;
                OnPropertyChanged(nameof(ExpenseNameTextBoxField));
                OnPropertyChanged(nameof(CanAddExpenseCost));
            }
        }
        private string _expenseCostTextBoxField;
        public string ExpenseCostTextBoxField
        {
            get
            {
                return _expenseCostTextBoxField;
            }
            set
            {
                _expenseCostTextBoxField = value;
                _errorsViewModel.RemoveError(nameof(ExpenseCostTextBoxField));
                if(!double.TryParse(_expenseCostTextBoxField, out double doubleValue) && _expenseCostTextBoxField != null)
                {
                    _errorsViewModel.AddError(nameof(ExpenseCostTextBoxField), "expense cost must not contain characters");
                }
                OnPropertyChanged(nameof(ExpenseCostTextBoxField));
            }
        }
        
        
        private ObservableCollection<string> _listOfExpenses = new ObservableCollection<string>();
        public ObservableCollection<string> ListOfExpenses
        {
            get
            {
                return _listOfExpenses;
            }
            set
            {
                _listOfExpenses = value;
                OnPropertyChanged(nameof(ListOfExpenses));
            }
        }

        private string _selectedExpenseFromListOfExpenses;
        public string SelectedExpenseFromListOfExpenses
        {
            get
            {
                return _selectedExpenseFromListOfExpenses;
            }
            set
            {
                _selectedExpenseFromListOfExpenses = value;
                OnPropertyChanged(nameof(SelectedExpenseFromListOfExpenses));
            }
        }

        // this should prolly be in a separate file

        private readonly Dictionary<int, string> PropertyReferenceDictionary = new Dictionary<int, string>()
        {
            {0, "Author" }, {1, "Genre" }, {2, "Book" }
        };

        // 0: Author
        // 1: Genre
        // 2: Book
        private int _selectedInputGraph;
        public int SelectedInputGraph
        {
            get { return _selectedInputGraph; }
            set
            {
                if (_selectedInputGraph != value)
                {
                    _selectedInputGraph = value;
                    OnPropertyChanged(nameof(SelectedInputGraph));
                    GraphTitle = $"{PropertyReferenceDictionary[SelectedInputGraph]} Graph";
                    OnPropertyChanged(nameof(GraphTitle));
                    ChangeGraphAxis(SelectedInputGraph);
                }
            }
        }
        public string GraphTitle { get; set; }
        public ISeries[] Series { get; set; }
        public Axis[] YAxes { get; set; }
        public Axis[] XAxes { get; set; }






        public readonly ErrorsViewModel _errorsViewModel;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public bool CanCreatePDF => !HasErrors && FileLocation != string.Empty;
        public bool CanAddExpenseCost => ExpenseNameTextBoxField != null && ExpenseNameTextBoxField != string.Empty;
        public ICommand SelectFileLocationCommand { get; }
        public ICommand GeneratePDFCommand { get; }
        public ICommand AddExpensesToDictCommand { get; }
        public ICommand RemoveExpenseListItem { get; }
        public DateTime MaximumDate { get; set; }
        public DateTime MinimumDate { get; set; }
        public SalesPanelViewModel()
        {
            // Command init
            SelectFileLocationCommand = new SelectFileLocationCommand(this, OnPropertyChanged);
            GeneratePDFCommand = new GeneratePDFCommand(this, ExpensesDictionary);
            AddExpensesToDictCommand = new AddExpensesToDictCommand(this);
            RemoveExpenseListItem = new RemoveExpenseListItem(this, SelectedExpenseFromListOfExpenses);

            // ViewModel data init
            DateTime oldestSalesDate = GetOldestSaleDate(Database);
            DateTime newestSalesDate = GetNewestSaleDate(Database);
            MinimumDate = oldestSalesDate;
            MaximumDate = newestSalesDate;
            SelectedDate = newestSalesDate; // Avoids an error where datepicker will bind to null and default to 0001/01/01
            AvailableSalesQuarters = GetAvailableSalesQuarters(newestSalesDate, oldestSalesDate);
            // Errors viewmodel
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

            SalesQuarters = new Dictionary<int, List<SalesQuarter>>();
            Years = GetAvailableSalesYears(MaximumDate, MinimumDate);
            foreach (int year in Years)
            {
                // Add the int as a key with an empty string as the value
                SalesQuarters.Add(year, null);
            }

            // Graph init
            SelectedInputGraph = 1;
            
        }
        private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreatePDF));
        }
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }






        // put this in separate file
        private void ResetComboBoxFields()
        {
            ComboBoxFieldResetter = null;
            OnPropertyChanged(nameof(ComboBoxFieldResetter));
        }
        private void ChangeGraphAxis(int propertyIntValue)
        {
            PopularityGraphProperty popularityEnum;
            switch (propertyIntValue)
            {
                case 0:
                    popularityEnum = PopularityGraphProperty.Author;
                    break;
                case 1:
                    popularityEnum = PopularityGraphProperty.Genre;
                    break;
                case 2:
                    popularityEnum = PopularityGraphProperty.Book;
                    break;
                default:
                    throw new Exception("how did you run into this");
            }



            GraphDataGrabber gdb = new GraphDataGrabber(DatabaseStore.JunctionValuesDictionary);
            var dict = gdb.GetGraphData(popularityEnum);
            string[] PropertyNameArray = dict.Keys.ToArray();
            double[] GraphValues = dict.Values.Select(value => (double)value).ToArray();

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = PropertyNameArray,
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.Transparent),
                    ForceStepToMin = true,
                    MinStep = 1
                }
            };
            OnPropertyChanged(nameof(XAxes));
            YAxes = new Axis[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = GraphValues.Max(),
                    Name = "Total Sales",
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.Transparent)
                }
            };
            OnPropertyChanged(nameof(YAxes));
            Series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = GraphValues,
                    Stroke = null,
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                    IgnoresBarPosition = true,
                    // Defines the distance between every bars in the series
                    Padding = 0,
                    MaxBarWidth = double.PositiveInfinity
                }
            };
            OnPropertyChanged(nameof(Series));
        }
    }
}
