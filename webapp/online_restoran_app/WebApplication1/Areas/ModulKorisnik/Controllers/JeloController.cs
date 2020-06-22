using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.EF;
using WebApplication1.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Helper;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using WebApplication1.Areas.ModulKorisnik.ViewModels.Jelo;

namespace WebApplication1.Areas.ModulKorisnik.Controllers
{
    [Autorizacija(zaposlenik: true, korisnik: true)]

    [Area("ModulKorisnik")]
    [Route("/ModulKorisnik/Jelo")]
    public class JeloController : Controller
    {
        private MyContext db;
        private readonly IHostingEnvironment hostingEnvironment;
        public JeloController(MyContext baza,
                              IHostingEnvironment hostingEnvironment)
        {

            db = baza;
            this.hostingEnvironment = hostingEnvironment;
        }


        [Route("/ModulKorisnik/Jelo/Index")]
        public IActionResult Index()
        {

            JeloIndexVM model = new JeloIndexVM
            {
                Rows = db.Jelo.Select(j => new JeloIndexVM.Row()
                {
                    Id = j.Id,
                    Naziv = j.Naziv,
                    VrstaJela = j.VrstaJela.Naziv,
                    Opis = j.Opis,
                    Cijena = j.Cijena,
                    Slika = j.Slika,
                    Sastojci = db.ImaSastojke.Where(s => s.JeloId == j.Id).Select(q => new JeloIndexVM.SastojakVM
                    {
                        Naziv = q.Sastojci.Naziv,
                        Opis = q.Sastojci.Opis
                    }).ToList()

                }).ToList()
            };


            return View(model);
        }
        

        
        

    }
}