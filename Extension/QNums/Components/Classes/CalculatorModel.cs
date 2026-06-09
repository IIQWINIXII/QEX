using System;
using System.Collections.Generic;
using System.Text;

namespace QNums.Components.Classes
{
    public class CalculatorModel
    {
        public string CurrentInput { get; set; } = "";
        public string Formula { get; set; } = "";
        public string LastResult { get; set; } = "";
        public bool IsResultDisplayed { get; set; }

        public void Clear()
        {
            CurrentInput = "";
            Formula = "";
            LastResult = "";
            IsResultDisplayed = false;
        }

        public void Backspace()
        {
            if (CurrentInput.Length > 0)
                CurrentInput = CurrentInput[..^1];
        }
    }
}
