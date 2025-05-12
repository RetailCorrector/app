using RetailCorrector.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Report : UserControl
    {
        public string Url { get; set; }
        public HttpMethod Method { get; set; } = HttpMethod.GET;
        public ReportContentType ContentType { get; set; } = ReportContentType.JSON;
        public ObservableCollection<Tuple<string, string>> Body { get; set; }
        public ObservableCollection<Tuple<string, string>> Headers { get; set; }

        public Report()
        {
            InitializeComponent();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var headers = new Dictionary<string, List<string>>();
            foreach (var head in Headers)
            {
                if(headers.ContainsKey(head.Item1))
                    headers[head.Item1].Add(head.Item2);
                else
                    headers.Add(head.Item1, [head.Item2]);
            }
            var body = new Dictionary<string, object>();
            foreach (var _b in Body)
                if (!body.ContainsKey(_b.Item1))
                    body.Add(_b.Item1, _b.Item2);

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
    }
}
