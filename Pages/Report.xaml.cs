using RetailCorrector.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Report : UserControl
    {
        public string Url { get; set; }
        public HttpMethod Method { get; set; } = HttpMethod.GET;
        public ReportContentType ContentType { get; set; } = ReportContentType.JSON;
        public ObservableCollection<KVPair> Body { get; } = [];
        public ObservableCollection<KVPair> Headers { get; } = [];

        public Report()
        {
            InitializeComponent();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var headers = new Dictionary<string, List<string>>();
            foreach (var head in Headers)
            {
                if(headers.ContainsKey(head.Key))
                    headers[head.Key].Add(head.Value);
                else
                    headers.Add(head.Key, [head.Value]);
            }
            var body = new Dictionary<string, object>();
            foreach (var _b in Body)
                if (!body.ContainsKey(_b.Key))
                    body.Add(_b.Key, _b.Value);

            App.Receipts.Report = new RetailCorrector.Report
            {
                Url = Url,
                Method = Method,
                Content = new ReportContent
                {
                    Type = ContentType,
                    Values = body
                },
                Headers = headers
            };
        }

        private void AddHeader(object sender, RoutedEventArgs args) => Headers.Add(new KVPair());
        
        private void AddBody(object sender, RoutedEventArgs args) => Body.Add(new KVPair());

        public class KVPair: INotifyPropertyChanged
        {
            public string Key
            {
                get => _key;
                set
                {
                    _key = value;
                    OnPropertyChanged();
                }
            }
            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }

            private string _key = "";
            private string _value = "";

            public event PropertyChangedEventHandler? PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string propName = "") =>
                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propName));
        }
    }
}
