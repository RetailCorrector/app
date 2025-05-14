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
        public ObservableCollection<StringsPair> Body { get; } = [];
        public ObservableCollection<StringsPair> Headers { get; } = [];

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
                {
                    if(long.TryParse(_b.Value, out var @long))
                        body.Add(_b.Key, @long);
                    else if (bool.TryParse(_b.Value, out var @bool))
                        body.Add(_b.Key, @bool);
                    else body.Add(_b.Key, _b.Value);
                }

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

        private void AddHeader(object sender, RoutedEventArgs args) => Headers.Add(new StringsPair());
        
        private void AddBody(object sender, RoutedEventArgs args) => Body.Add(new StringsPair());
    }
}
