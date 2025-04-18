using Microsoft.Extensions.Options;
using MimeKit;

namespace OnlineShopWebApp.Helpers
{
	public class EmailService
	{
		readonly SmtpSettings _smtpSettings;
		readonly ILogger<EmailService> _logger;
		public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
		{
			_smtpSettings = smtpSettings.Value;
			_logger = logger;
		}

		public void SendEmail(string email, string subject, string message)
		{
			try
			{
				var emailMessage = new MimeMessage();

				emailMessage.From.Add(new MailboxAddress("Администрация сайта Roseto-Shop", _smtpSettings.Username));
				emailMessage.To.Add(new MailboxAddress("", email));
				emailMessage.Subject = subject;
				emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
				{
					Text = message
				};

				using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
				{
					client.Connect(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.EnableSsl);
					client.Authenticate(_smtpSettings.Username, _smtpSettings.Password);
					client.Send(emailMessage);
					client.Disconnect(true);
					_logger.LogInformation("Сообщение на почту отправлено успешно!");
				}
			}
			catch (Exception ex)
			{
				// Логируйте ошибку
				_logger.LogError(ex.Message);
				throw;
			}

		}
	}
}
