using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace MonitorTargetApp.Services.Notification
{
    class EmailNotification : NotificationBase
    {
        public string _sender = "dias@dias.com";
        public string _senderName = "Dias Tech";
        public string _recipient { get; set; }
        public string _recipientName { get; set; }
        public string _subject { get; set; }
        public string _body { get; set; }

        public override void Send()
        { 
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_senderName, _sender));
            message.To.Add(MailboxAddress.Parse("volkancengiz@outlook.com"));
            message.Subject = _subject;
            message.Body = new TextPart("plain")
            {
                Text = _body
            };

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.freesmtpservers.com", 25, false);
            smtpClient.Send(message);
            smtpClient.Disconnect(true);
            smtpClient.Dispose();
        }
    }
}
