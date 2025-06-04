using RetailCorrector.Wizard.Contexts;
using RetailCorrector.Wizard.UserControls;
using Serilog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace RetailCorrector.Wizard.Windows
{
    public partial class Report : Window, INotifyPropertyChanged
    {
        public Command WritePattern { get; } = new Command(LocalWritePattern);
        public RoutedCommand Escape { get; } = new RoutedCommand(nameof(Escape), typeof(Report));

        private static Report? Singleton { get; set; } = null;
        private static void LocalWritePattern(object? pattern)
        {
            var text = $"`{(string)pattern!}`";
            var index = Singleton!.body.CaretIndex;
            Singleton.Body = Singleton.Body.Insert(Singleton.body.CaretIndex, text);
            Singleton.body.CaretIndex = index + text.Length;
        }

        public string Url
        {
            get => _url;
            set
            {
                if (_url == value) return;
                _url = value;
                OnPropertyChanged();
            }
        }
        private string _url = WizardDataContext.Report.Url;

        public HttpMethod Method
        {
            get => _method;
            set
            {
                if (_method == value) return;
                _method = value;
                OnPropertyChanged();
            }
        }
        private HttpMethod _method = WizardDataContext.Report.Method;

        public string Body
        {
            get => _content;
            set
            {
                if (_content == value) return;
                _content = value;
                OnPropertyChanged();
            }
        }
        private string _content = WizardDataContext.Report.Content;

        public string ContentType
        {
            get => _contentType;
            set
            {
                if (_contentType == value) return;
                _contentType = value;
                OnPropertyChanged();
            }
        }
        private string _contentType = WizardDataContext.Report.ContentType;

        public int HeaderIndex { get; set; } = -1;

        public bool IsFreeRequest
        {
            get => _isFreeRequest;
            set {
                if (_isFreeRequest == value) return;
                _isFreeRequest = value;
                OnPropertyChanged();
            }
        }
        private bool _isFreeRequest = true;

        public ObservableCollection<StringsPair> Headers { get; set; } = GenHeaders();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static ObservableCollection<StringsPair> GenHeaders()
        {
            var headers = new ObservableCollection<StringsPair>();
            if (WizardDataContext.Report.Headers is not null)
            {
                foreach (var header in WizardDataContext.Report.Headers)
                    foreach (var value in header.Value)
                        headers.Add(new StringsPair(header.Key, value));
            }
            return headers;
        }

        public Report()
        {
            Singleton = this;
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
                Log.Information($"Тестирование шаблона отчета: {Method:F}");
                Log.Information(Url);
                using var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Parse(Method.ToString()), Url);
                foreach (var header in Headers)
                {
                    if (!string.IsNullOrWhiteSpace(header.Key) && !string.IsNullOrWhiteSpace(header.Value))
                    {
                        request.Headers.Add(header.Key, header.Value);
                        Log.Information($"{header.Key}: {header.Value}");
                    }
                }
                if (!string.IsNullOrWhiteSpace(Body))
                {
                    var body = $"{Body}";
                    body = Regex.Replace(body, "`([a-z:]*?)`", "555");
                    request.Content = new StringContent(body, MediaTypeHeaderValue.Parse(ContentType));
                    Log.Information(body);
                }
                using var resp = await client.SendAsync(request);
                var code = (int)resp.StatusCode;
                Log.Information($"Результат тестирования: {code}");
                var content = await resp.Content.ReadAsStringAsync();
                Log.Information(content);
                var status = code switch
                {
                    (>= 100 and <= 199) or (>= 300 and <= 399) => MessageBoxImage.Asterisk,
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
                ErrorAlert(ex, "Ошибка тестирования запроса отчета");
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
            var report = WizardDataContext.Report;
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
            WizardDataContext.Report = report;
            Singleton = null;
            base.OnClosed(e);
        }
    }
}
