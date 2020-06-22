using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.EF;
using WebApplication1.Models;
using WebApplication1.Helper;
using WebApplication1.Areas.ModulKorisnik.ViewModels;
using WebApplication1.Areas.ModulKorisnik.ViewModels.Obavijest;

namespace WebApplication1.Areas.ModulKorisnik.Controllers
{

    [Autorizacija(zaposlenik: true, korisnik: true)]

    [Area("ModulKorisnik")]
    [Route("/ModulKorisnik/Obavijest")]

    public class ObavijestController : Controller
    {

        private MyContext _db;
        public ObavijestController(MyContext baza)
        {
            _db = baza;
        }


        [Route("/ModulKorisnik/Obavijest/")]
        [Route("/ModulKorisnik/Obavijest/Index")]
        public IActionResult Index()
        {
            ObavijestIndexVM model = new ObavijestIndexVM
            {
                Rows = _db.Obavijest.Select(s => new ObavijestIndexVM.Row
                {
                    ObavijestId = s.Id,
                    Naziv = s.Naziv,
                    Sadrzaj = s.Sadrzaj,
                    Datum = s.Datum,
                    ImePrezime = s.Zaposlenik.Ime + " " + s.Zaposlenik.Prezime
                }).ToList()
            };
            return View(model);
        }
    }
}