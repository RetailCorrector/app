using OrnaUtils;
using RetailCorrector.Storage;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace RetailCorrector.Utils
{
    public static class Expoter
    {
        private static DirectoryWatcher path = 
            new DirectoryWatcher(Path.GetTempPath())
            .Add($"RetailCorrector-{Env.WorkspaceId:D}");

        public static void UpdateSpaceId()
        {
            path = new DirectoryWatcher(Path.GetTempPath())
                .Add($"RetailCorrector-{Env.WorkspaceId:D}");
        }

        public static void Local()
        {
            ExportReport((string)path);
            ExportReceipts((string)path);
            Process.Start("explorer", ((string)path).Replace("\\\\", "\\"));
            MessageBox.Show("Чеки и отчет выгружены!");
        }

        private static void ExportReport(string pathDirectory)
        {
            var report = Path.Combine(pathDirectory, "report");
            var json = JsonSerializer.Serialize(Env.Report);
            File.WriteAllText(report, json);
        }

        private static void ExportReceipts(string pathDirectory)
        {
            var receipts = StorageContext.Instance.Receipts.Local.Select(i => (Receipt)i).ToList();
            var chuckes = receipts.Chunk(25).ToArray();
            for (var i = 0; i < chuckes.Length; i++)
            {
                var _path = Path.Combine(pathDirectory, $"receipts_{i}");
                var json = JsonSerializer.Serialize(chuckes[i]);
                File.WriteAllText(_path, json);
            }
        }

    }
}
