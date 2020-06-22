using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.EF;
using WebApplication1.Models;
using WebApplication1.Helper;
using WebApplication1.Areas.ModulKorisnik.ViewModels.Narucilac;

namespace WebApplication1.Areas.ModulKorisnik.Controllers
{
    [Autorizacija(zaposlenik: true, korisnik: true)]

    [Area("ModulKorisnik")]
    [Route("/ModulKorisnik/Narucilac")]
    public class NarucilacController : Controller
    {
        private MyContext _db;
        public NarucilacController(MyContext db)
        {
            _db = db;
        }

        [Route("/ModulKorisnik/Narucilac/Index")]
        public IActionResult Index(int KorisnikId)
        {
            Narucilac n = _db.Narucilac.Where(s => s.KorisnickiNalogId == KorisnikId).FirstOrDefault();

            KorisnickiNalog kn = _db.KorisnickiNalog.Where(w => w.Id == KorisnikId).SingleOrDefault();

            NarucilacIndexVM model = new NarucilacIndexVM();
            
            model.NarucilacId = n.NarucilacId;
            model.ImePrezime = n.ImePrezime;
            model.GradId = n.GradId;
            model.GradNaziv = _db.Grad.Where(q=> q.GradId == n.GradId).Select(q=> q.Naziv).FirstOrDefault();
            model.KorisnickoIme = n.KorisnickiNalog.KorisnickoIme;
            model.Lozinka = n.KorisnickiNalog.Lozinka;

            model.Telefon = n.Telefon;
            model.Email = n.Email;
            model.Adresa = n.Adresa;

            
            return View(model);
        }

        [Route("/ModulKorisnik/Narucilac/Uredi")]
        public IActionResult Uredi(int NarucilacId, int KorisnickiNalogId)
        {
            Narucilac n = _db.Narucilac.Find(NarucilacId);
            KorisnickiNalog kn = _db.KorisnickiNalog.Where(w => w.Id == n.KorisnickiNalogId).SingleOrDefault();

            NarucilacUrediVM model = new NarucilacUrediVM();

            model.gradLista = _db.Grad.Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.GradId.ToString()
            }).ToList();

            model.NarucilacId = NarucilacId;
            model.ImePrezime = n.ImePrezime;
            model.GradId = n.GradId;
            model.KorisnickoIme = n.KorisnickiNalog.KorisnickoIme;
            model.Lozinka = n.KorisnickiNalog.Lozinka;

            model.Telefon = n.Telefon;
            model.Email = n.Email;
            model.Adresa = n.Adresa;

            model.gradLista = _db.Grad.Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.GradId.ToString()
            }).ToList();

            return View(model);
        }

        [Route("/ModulKorisnik/Narucilac/UrediSave")]
        [HttpPost]
        public IActionResult UrediSave(NarucilacUrediVM model)
        {
            
                Narucilac n = _db.Narucilac.Find(model.NarucilacId);
                KorisnickiNalog kn = _db.KorisnickiNalog.Where(w => w.Id == n.KorisnickiNalogId).FirstOrDefault();

                if (n == null)
                {
                    n = new Narucilac();
                    kn = new KorisnickiNalog();
                    _db.Add(kn);
                    _db.Add(n);

                }
                kn.KorisnickoIme = model.KorisnickoIme;
                kn.Lozinka = model.Lozinka;
                n.ImePrezime = model.ImePrezime;
                n.KorisnickiNalogId = kn.Id;
                n.Email = model.Email;
                n.Telefon = model.Telefon;
                n.Adresa = model.Adresa;
                n.GradId = model.GradId;

                _db.SaveChanges();
                return Redirect("/ModulKorisnik/Narucilac/Index?KorisnikId=" + kn.Id);            
        }
    }
    
}