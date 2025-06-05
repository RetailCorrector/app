﻿using System.Windows.Input;

namespace RetailCorrector.Wizard.UserControls
{
    public class Command(Action<object?> execute, Predicate<object?>? canExecute = null) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) =>
            canExecute == null || canExecute(parameter);

        public void Execute(object? parameter) => execute(parameter);
    }
}
