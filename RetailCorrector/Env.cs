using System.Collections.ObjectModel;

namespace RetailCorrector;

public static class Env
{
    public static Report Report { get; set; } = new Report();
    public static ObservableCollection<Receipt> Receipts { get; } = [];
}
