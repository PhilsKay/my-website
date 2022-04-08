using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using PhilskayPortfolio.Models;
using System.Diagnostics;

namespace PhilskayPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MailSettings _mailSettings;

        public HomeController(ILogger<HomeController> logger, IOptions<MailSettings> mailsettings)
        {
            _logger = logger;
            _mailSettings = mailsettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Index(ContactForm obj)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var email = new MimeMessage();
                    email.Sender = MailboxAddress.Parse(obj.From);
                    email.To.Add(MailboxAddress.Parse(_mailSettings.Username));
                    email.Subject = obj.Subject;
                    var builder = new BodyBuilder();
                    string msg = $"Name : {obj.Name} \nFrom : {obj.From} \n\n{obj.Message}";
                    builder.HtmlBody = msg;
                    email.Body = builder.ToMessageBody();
                    SmtpClient smtp = new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.None);
                    smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
                    await smtp.SendAsync(email);
                    ViewBag.Message = "Thanks! Received your message, will get in touch.";
                    return View();
                }
                catch (System.Net.Mail.SmtpException) { 
                    ViewBag.Message = "Sorry Message could not be sent.";
                    return View();
                }


            }
            ModelState.AddModelError(obj.From, "Invalid");
            return View(obj);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
