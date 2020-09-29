using System;

namespace MenuSystem
{
    
    public class MenuItem

    {
        public virtual string Label { get; set; }
        public virtual string UserChoice { get; set; }
        public virtual Func<string> MethodToExecute { get; set; }

        public MenuItem(string label, string userChoice, Func<string> methodToExecute)
        {
            Label = label.Trim();
            UserChoice = userChoice.Trim();
            MethodToExecute = methodToExecute;
        }

        public override string ToString()
        {
            return UserChoice + ") " + Label;
        }
    }
}