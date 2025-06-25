using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using RetailCorrector.Utils;

namespace RetailCorrector.ReportEditor
{
    public partial class Report : Window
    {
        public Command WritePattern { get; } = new(LocalWritePattern);
        public RoutedCommand Escape { get; } = new(nameof(Escape), typeof(Report));

        private static Report? Singleton { get; set; }
        private static void LocalWritePattern(object? pattern)
        {
            var text = $"`{(string)pattern!}`";
            var index = Singleton!.body.CaretIndex;
            Singleton.Body = Singleton.Body.Insert(Singleton.body.CaretIndex, text);
            Singleton.body.CaretIndex = index + text.Length;
        }

        public int HeaderIndex { get; set; } = -1;

        [NotifyUpdated] private string _url = Env.Report.Url;
        [NotifyUpdated] private HttpMethod _method = Env.Report.Method;
        [NotifyUpdated] private string _body = Env.Report.Content;
        [NotifyUpdated] private string _contentType = Env.Report.ContentType;
        [NotifyUpdated] private bool _isFreeRequest = true;

        public ObservableCollection<StringsPair> Headers { get; set; } = GenHeaders();

        private static ObservableCollection<StringsPair> GenHeaders()
        {
            var headers = new ObservableCollection<StringsPair>();
            if (Env.Report.Headers is not null)
            {
                foreach (var header in Env.Report.Headers)
                    foreach (var value in header.Value)
                        headers.Add(new StringsPair(header.Key, value));
            }
            return headers;
        }

        public Report()
        {
            Singleton = this;
            CommandBindings.Add(new CommandBinding(Commands.ExitDialog, (_, _) => Close()));
            Escape.InputGestures.Add(new KeyGesture(Key.Escape));
            CommandBindings.Add(new CommandBinding(Escape, (_, _) => Close()));
            InitializeComponent();
        }

        public async void TestRequest(object? sender, RoutedEventArgs args)
        {
            try
            {
                IsFreeRequest = false;
                using var client = new HttpClient();
                Alert.Debug($"Тестирование шаблона отчета: {Method:F}");
                Alert.Debug(Url);
                using var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Parse(Method.ToString()), Url);
                foreach (var header in Headers)
                {
                    if (!string.IsNullOrWhiteSpace(header.Key) && !string.IsNullOrWhiteSpace(header.Value))
                    {
                        request.Headers.Add(header.Key, header.Value);
                        Alert.Debug($"{header.Key}: {header.Value}");
                    }
                }
                if (!string.IsNullOrWhiteSpace(Body))
                {
                    var body = $"{Body}";
                    body = Regex.Replace(body, "`([a-z:]*?)`", "555");
                    request.Content = new StringContent(body, MediaTypeHeaderValue.Parse(ContentType));
                    Alert.Debug(body);
                }
                using var resp = await client.SendAsync(request);
                var code = (int)resp.StatusCode;
                Alert.Debug($"Результат тестирования: {code}");
                var content = await resp.Content.ReadAsStringAsync();
                Alert.Debug(content);
                var status = code switch
                {
                    >= 100 and <= 199 or >= 300 and <= 399 
                        => MessageBoxImage.Asterisk,
                    >= 200 and <= 299 => MessageBoxImage.None,
                    >= 400 and <= 499 => MessageBoxImage.Error,
                    >= 500 and <= 599 => MessageBoxImage.Stop,
                    _ => MessageBoxImage.Question
                };
                if(resp.Content.Headers.ContentType!.MediaType == "text/html")
                    content = content.Length > 1000 ? content[..1000] + "..." : content;
                MessageBox.Show(content, $"Результат тестирования запроса ({code})", MessageBoxButton.OK, status);
            }
            catch (Exception ex)
            {
                Alert.Error("Ошибка тестирования запроса отчета", ex);
            }
            finally
            {
                IsFreeRequest = true;
            }
        }

        public void AddHeader(object? sender, RoutedEventArgs args) =>
            Headers.Add(new StringsPair());

        public void RemoveHeader(object? sender, RoutedEventArgs args)
        {
            if (HeaderIndex != -1) 
                Headers.RemoveAt(HeaderIndex);
        }

        protected override void OnClosed(EventArgs e)
        {
            var report = Env.Report;
            report.Method = Method;
            report.Url = Url;
            report.Content = Body;
            report.Headers = [];
            foreach (var header in Headers)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    if (!report.Headers.ContainsKey(header.Key))
                        report.Headers.Add(header.Key, []);
                    report.Headers[header.Key].Add(header.Value);
                }
            }
            Env.Report = report;
            Singleton = null;
            base.OnClosed(e);
        }
    }
}
