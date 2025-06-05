using System.Collections.ObjectModel;

namespace RetailCorrector.Wizard.Contexts;

public static class WizardDataContext
{
    public static Report Report { get; set; } = new Report();
    public static ObservableCollection<Receipt> Receipts { get; } = [];
