using BookInventoryAdminProgram.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Model
{
    public class FilteringDatabase
    {
        /// <summary>
        /// Gets all the inputs required for a filter operation in the InventoryViewModel.
        /// Returns them as a Dict(str,str).
        /// 
        /// Keys: PropertyName, FieldName, Condition, FilterValue, FilterBookName.
        /// </summary>
        /// <param name="InvenPanelviewModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception">i should probably handle this in a non fatal way ngl</exception>
        public Dictionary<string, string> GetInputsForFiltering(InventoryPanelViewModel InvenPanelviewModel)
        {
            // this prolly violates mvvm becuase there is a connection between our viewmodel and our model but idc.

            string salesCriteria = InvenPanelviewModel.SelectedItem["Sales"];
            string typeCriteria = InvenPanelviewModel.SelectedItem["Type"];
            string modifierCriteria = InvenPanelviewModel.SelectedItem["Modifier"];

            string propertyName = "";
            switch (typeCriteria)
            {
                case "Daily":
                    propertyName = "DailySales";
                    break;
                case "Monthly":
                    propertyName = "MonthlySales";
                    break;
                case "Yearly":
                    propertyName = "YearlySales";
                    break;
                case "All-Time":
                    propertyName = "AllTimeSales";
                    break;
                case null:
                    propertyName = null;
                    break;
                default: // will never occur in runtime
                    throw new Exception("Input not recognised");
                    
            }
            string fieldName;
            switch (salesCriteria)
            {
                case "Revenue":
                    fieldName = "Revenue";
                    break;
                case "Sales":
                    fieldName = "QuantitySold";
                    break;
                case null: 
                    fieldName = null;
                    break;
                default: // will never occur in runtime
                    throw new Exception("Input not recognised");
            }
            string condition;
            switch (modifierCriteria)
            {
                case "Greater than":
                    condition = ">";
                    break;
                case "Less than":
                    condition = "<";
                    break;
                case "Equal to":
                    condition = "==";
                    break;
                case null:
                    condition = null;
                    break;
                default: // will never occur in runtime
                    throw new Exception("Input not recognised. Ill handle this later probably");
                    
            }
            string filterValue = InvenPanelviewModel.ComboBoxQueryQuantity;
            string filterBookName = InvenPanelviewModel.SearchFieldValue;

            
            return new Dictionary<string, string>
            {
                { "PropertyName", propertyName },
                { "FieldName", fieldName },
                { "Condition", condition },
                { "FilterValue", filterValue },
                { "FilterBookName", filterBookName }
            };
        }
        /// <summary>
        /// Checks for errors in inputs inside the InventoryPanelViewmodel.
        /// If errors exist, return true.
        /// </summary>
        /// <param name="errorsViewModel"></param>
        /// <param name="InvenPanelviewModel"></param>
        /// <returns></returns>
        public bool CheckForErrors(ErrorsViewModel errorsViewModel, InventoryPanelViewModel ip)
        {
            // fucking filthy code
            // may God have mercy on my soul

            bool comboBoxDoesntHaveItems = false;
            errorsViewModel.RemoveError(nameof(ip.ComboBoxQueryQuantity));
            errorsViewModel.RemoveError(nameof(ip.SalesComboBoxOptions));
            errorsViewModel.RemoveError(nameof(ip.TypeComboBoxOptions));
            errorsViewModel.RemoveError(nameof(ip.ModifierComboBoxOptions));

            // this needs some tidying up. Will do before release.
            if (!ip.SalesComboBoxOptions.Contains(ip.SelectedItem["Sales"]) && (ip.ComboBoxTypedText["Sales"] != null && ip.ComboBoxTypedText["Sales"] != ""))
            { errorsViewModel.AddError(nameof(ip.SalesComboBoxOptions), "Not a recognised operation"); comboBoxDoesntHaveItems = true; }
            if (!ip.TypeComboBoxOptions.Contains(ip.SelectedItem["Type"]) && (ip.ComboBoxTypedText["Type"] != null && ip.ComboBoxTypedText["Type"] != ""))
            { errorsViewModel.AddError(nameof(ip.TypeComboBoxOptions), "Not a recognised operation"); comboBoxDoesntHaveItems = true; }
            if (!ip.ModifierComboBoxOptions.Contains(ip.SelectedItem["Modifier"]) && (ip.ComboBoxTypedText["Modifier"] != null && ip.ComboBoxTypedText["Modifier"] != ""))
            { errorsViewModel.AddError(nameof(ip.ModifierComboBoxOptions), "Not a recognised operation"); comboBoxDoesntHaveItems = true; }


            if((ip.ComboBoxTypedText["Modifier"] == null || ip.ComboBoxTypedText["Modifier"] == "")
                || (ip.ComboBoxTypedText["Type"] == null || ip.ComboBoxTypedText["Type"] == "")
                || (ip.ComboBoxTypedText["Sales"] == null || ip.ComboBoxTypedText["Sales"] == ""))
                comboBoxDoesntHaveItems = true;

            if (!int.TryParse(ip.ComboBoxQueryQuantity, out int i) && ip.ComboBoxQueryQuantity != null)
                errorsViewModel.AddError(nameof(ip.ComboBoxQueryQuantity), "Quantity is only a number");
            else if (int.TryParse(ip.ComboBoxQueryQuantity, out int j) && int.Parse(ip.ComboBoxQueryQuantity) < 0)
                errorsViewModel.AddError(nameof(ip.ComboBoxQueryQuantity), "Quantity cant be less than one");
            else if (comboBoxDoesntHaveItems && ip.ComboBoxQueryQuantity != null)
                errorsViewModel.AddError(nameof(ip.ComboBoxQueryQuantity), "Incomplete query");

            if (errorsViewModel.HasErrors)
                return true;
            return false;
        }
        /// <summary>
        /// Performs MergeSort algorithm
        /// </summary>
        /// <param name="unsortedList"></param>
        /// <returns></returns>
        public static List<string> MergeSort(List<string> unsortedList)
        {
            if (unsortedList == null || unsortedList.Count <= 1)
                return unsortedList;

            // split the list up into 2
            int mid = unsortedList.Count / 2;
            List<string> leftHalf = new List<string>(unsortedList.GetRange(0, mid));
            List<string> rightHalf = new List<string>(unsortedList.GetRange(mid, unsortedList.Count - mid));

            // if one of these isn't just 1 element, split it up further.
            // once that has been split up AND sorted, that process finishes
            // and it can resume.
            // if left.count > 1 and right.count <= 1, after left has been sorted
            // resume at line 27.
            leftHalf = MergeSort(leftHalf);
            rightHalf = MergeSort(rightHalf);

            return Merge(leftHalf, rightHalf);
        }

        /// <summary>
        /// Merges two lists alphabetically.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static List<string> Merge(List<string> left, List<string> right)
        {
            List<string> merged = new List<string>();
            int leftIndex = 0;
            int rightIndex = 0;

            // so we dont have an indexOutOfRange error
            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                // using ordinal rules (binary) checks which str goes before which.
                // "< 0" => L goes before R
                // "= 0" => same position (i.e same word)
                // "> 0" => R goes before L
                if (string.Compare(left[leftIndex], right[rightIndex], StringComparison.Ordinal) <= 0)
                {
                    merged.Add(left[leftIndex]); // left goes before right
                    leftIndex++;
                }
                else
                {
                    merged.Add(right[rightIndex]); // right goes before left
                    rightIndex++;
                }
            }

            // This area adds any left overs onto merge
            while (leftIndex < left.Count)
            {
                merged.Add(left[leftIndex]);
                leftIndex++;
            }

            while (rightIndex < right.Count)
            {
                merged.Add(right[rightIndex]);
                rightIndex++;
            }

            return merged;
        }

    }
}
