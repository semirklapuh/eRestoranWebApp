using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MailKit.Net.Smtp;
using WebApplication1.Helper;
using WebApplication1.EF;
using WebApplication1.Areas.ModulKorisnik.ViewModels.Kontakt;
using WebApplication1.Models;

namespace WebApplication1.Areas.ModulKorisnik.Controllers
{
    [Autorizacija(zaposlenik: true, korisnik: true)]

    [Area("ModulKorisnik")]
    [Route("/ModulKorisnik/Kontakt")]
    public class KontaktController : Controller
    {
        private const string LogiraniKorisnik = "logirani_korisnik";
        private MyContext _db;
        public KontaktController(MyContext db)
        {
            _db = db;
        }

        [Route("/ModulKorisnik/Kontakt/Index")]
        public IActionResult Index()
        {
            KorisnickiNalog korisnik = HttpContext.GetLogiraniKorisnik();
            Narucilac n = _db.Narucilac.Where(q => q.KorisnickiNalogId == korisnik.Id).FirstOrDefault();

            KontaktIndexVM model = new KontaktIndexVM();
            
            model.Narucilac = n.ImePrezime;
            model.Email = n.Email;
           
            return View(model);
        }

        [Route("/ModulKorisnik/Kontakt/Posalji")]
        public IActionResult Posalji(KontaktIndexVM model)
        {
           
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Semir", "eRestoran2020@gmail.com"));
            message.To.Add(new MailboxAddress("Semir", "eRestoran2020@gmail.com"));
            message.Subject = "Poruka od korisnika";

            message.Body = new TextPart("plain")
            {
                Text = model.Poruka + " Poruka poslana od: " + model.Narucilac
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("eRestoran2020@gmail.com", "restoran123");

                client.Send(message);

                client.Disconnect(true);
            }

            return Redirect("/ModulKorisnik/Home/Index");
        }
    }
}