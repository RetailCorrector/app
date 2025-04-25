using Serilog;
using Serilog.Core;
using System.Windows;

namespace RetailCorrector.Wizard
{
    public partial class App : Application
    {
        public static ReceiptCollection Receipts { get; } = new ReceiptCollection();
        public static Logger Logger { get; } = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                "logs\\.log", rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromMilliseconds(100))
            .CreateLogger();
    }
}
