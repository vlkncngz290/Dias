using MonitorTargetApp.Models;
using MonitorTargetApp.Repositories.TargetApp;
using MonitorTargetApp.Services.Notification;
using System.Net;

namespace MonitorTargetApp.Services.CheckAppHealth
{
    public class CheckAppHealth : IHostedService, IDisposable
    {
        private readonly ITargetAppRepository _targetAppRepository;
        private readonly ILogger<CheckAppHealth> _logger;
        private Timer _timer;
        private int _healthCheckIntervalInSeconds = 20;
        public CheckAppHealth(IServiceScopeFactory factory, ILogger<CheckAppHealth> logger)
        {
            _targetAppRepository = factory.CreateScope().ServiceProvider.GetRequiredService<ITargetAppRepository>();
            _logger = logger;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            _timer = new Timer(o =>
            {
                List<TargetApp> targetApps = new List<TargetApp>();
                targetApps = (List<TargetApp>)_targetAppRepository.Index();
                foreach (TargetApp targetApp in targetApps)
                {
                    var client = new HttpClient();
                    String url = targetApp.Url;
                    Uri uri;
                    if ((Uri.TryCreate(url, UriKind.Absolute, out uri) || Uri.TryCreate("http://" + url, UriKind.Absolute, out uri)) &&
                        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                    {
                        WebRequest request = WebRequest.Create(uri);
                        // If required by the server, set the credentials.
                        request.Credentials = CredentialCache.DefaultCredentials;
                        // Get the response.
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            if (response.StatusDescription == "OK")
                            {
                                targetApp.LastStatus = 1;
                            }
                            else
                            {
                                targetApp.LastStatus = 2;
                                //Send Mail
                                EmailNotification emailNotification = new EmailNotification();
                                emailNotification._recipient = "volkancengiz@outlook.com";
                                emailNotification._recipientName = "Volkan";
                                emailNotification._subject = "Konu";
                                emailNotification._body = "İcerik";
                                try
                                {
                                    emailNotification.Send();
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError("\n\n\nError Message:\n" + ex.Message.ToString() + "\n\n Cant connect or send via SMTP Server",ex);
                                }
                            }
                            response.Close();

                        }
                        catch (Exception e)
                        {
                            _logger.LogError("\n\n\nError Message:\n" + e.Message.ToString() + "\n\n Cant connect Target App URL");
                            targetApp.LastStatus = 2;
                            //Send Mail
                            EmailNotification emailNotification = new EmailNotification();
                            emailNotification._recipient = "volkancengiz@outlook.com";
                            emailNotification._recipientName = "Volkan";
                            emailNotification._subject = "Konu";
                            emailNotification._body = "İcerik";
                            try
                            {
                                emailNotification.Send();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("\n\n\nError Message:\n" + ex.Message + "\n\n Cant connect or send via SMTP Server",ex);
                            }
                        }
                        _targetAppRepository.Edit(targetApp.Id, targetApp);
                    }
                    Thread.Sleep(1000);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(_healthCheckIntervalInSeconds));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
