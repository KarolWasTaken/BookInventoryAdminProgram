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
        public Dictionary<string, bool> HeaderVisibility { get; }
        public Action<string, bool> SetDictionary { get; }


        public ToggleHeaderVisibilityCommand(Dictionary<string, bool> headerVisibility, Action<string, bool> setDictionary)
        {
            HeaderVisibility = headerVisibility;
            SetDictionary = setDictionary;
        }

        public override void Execute(object? parameter)
        {

            // updates appsettings.json to hold new visibility dict
            Helper.UpdateHeaderVisibilityJson(HeaderVisibility);

            // Hours wasted: 3
            
            // Literally only God knows why this code wont work without the below line
            // it literally does nothing. Everything it does is done by SetDictionary.
            // I have no words.

            HeaderVisibility[parameter.ToString()] = !HeaderVisibility[parameter.ToString()];
            SetDictionary.Invoke(parameter.ToString(), !HeaderVisibility[parameter.ToString()]);
        }
    }
}
