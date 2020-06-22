using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.EF;
using WebApplication1.Models;
using WebApplication1.Helper;
using WebApplication1.Areas.ModulKorisnik.ViewModels.Rezervacija;

namespace WebApplication1.Areas.ModulKorisnik.Controllers
{
    [Autorizacija(zaposlenik: true, korisnik: true)]

    [Area("ModulKorisnik")]
    [Route("/ModulKorisnik/Rezervacija")]
    public class RezervacijaController : Controller
    {
        private MyContext _db;
        public RezervacijaController(MyContext db)
        {
            _db = db;
        }

        [Route("/ModulKorisnik/Rezervacija/Index")]
        public IActionResult Index()
        {
            RezervacijaIndexVM model = new RezervacijaIndexVM
            {               
                Rows = _db.Rezervacija.Select(r => new RezervacijaIndexVM.Row()
                {
                    RezervacijaId = r.Id,
                    DatumEvidencije = r.DatumEvidencije,
                    DatumRezervacije = r.DatumRezervacije,
                    ImePrezime = r.Narucilac.ImePrezime,
                    Naziv = r.Naziv,
                    BrojMjesta = _db.RezervacijaStavke.Where(w => w.RezervacijaId == r.Id).Select(g => g.Stol.BrojMjesta).FirstOrDefault(),
                    KoriscnikiNalogId = r.Narucilac.KorisnickiNalogId
                }).ToList()
            };
            return View(model);
        }


        [Route("/ModulKorisnik/Rezervacija/Dodaj")]
        public IActionResult Dodaj(int KorisnikId)
        {
            RezervacijaDodajVM model = new RezervacijaDodajVM();

            Narucilac n = _db.Narucilac.Where(w=> w.KorisnickiNalogId== KorisnikId).FirstOrDefault();
            model.NarucilacId = n.NarucilacId;
            model.NarucilacIme = n.ImePrezime;
            model.DatumEvidencije = DateTime.Now;
            return View(model);
        }


        [Route("/ModulKorisnik/Rezervacija/DodajSave")]
        [HttpPost]
        public IActionResult DodajSave(RezervacijaDodajVM model)
        {
           
                Rezervacija r = _db.Rezervacija.Find(model.RezervacijaId);
                RezervacijaStavke rs = _db.RezervacijaStavke.Where(w => w.RezervacijaId == model.RezervacijaId).FirstOrDefault();
                if (r == null)
                {
                    r = new Rezervacija();
                    rs = new RezervacijaStavke();
                    _db.Add(r);
                    _db.Add(rs);
                }


                r.NarucilacId = model.NarucilacId;
                r.Naziv = model.Naziv;
                r.DatumEvidencije = model.DatumEvidencije;
                r.DatumRezervacije = model.DatumRezervacije;

                rs.RezervacijaId = r.Id;
                rs.StolId = model.BrojStolova;

                _db.SaveChanges();

                return RedirectToAction("Index");
           
        }

        [Route("/ModulKorisnik/Rezervacija/Obrisi")]
        [HttpGet]
        public IActionResult Obrisi(int id)
        {
            Rezervacija obrisati = _db.Rezervacija.SingleOrDefault(r => r.Id == id);
            IEnumerable<RezervacijaStavke> obrisatiStavke = _db.RezervacijaStavke.Where(rs => rs.RezervacijaId == id);
            if (obrisati == null)
                return View("Error");

            foreach (RezervacijaStavke rezervacijaStavke in obrisatiStavke)
            {
                _db.Remove(rezervacijaStavke);
            }
            _db.Remove(obrisati);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("/ModulKorisnik/Rezervacija/Uredi")]
        [HttpGet]
        public IActionResult Uredi(int id)
        {
            Rezervacija rezervacija = _db.Rezervacija.Find(id);
            RezervacijaStavke rs = _db.RezervacijaStavke.Where(w => w.RezervacijaId == id).FirstOrDefault();
            Narucilac n = _db.Narucilac.Where(w => w.NarucilacId == rezervacija.NarucilacId).FirstOrDefault();
            RezervacijaDodajVM model = new RezervacijaDodajVM();

            model.RezervacijaId = id;
            model.Naziv = rezervacija.Naziv;
            model.DatumEvidencije = rezervacija.DatumEvidencije;
            model.DatumRezervacije = rezervacija.DatumRezervacije;
            model.BrojStolova = rs.StolId;
            model.NarucilacIme = n.ImePrezime;
            model.NarucilacId = rezervacija.NarucilacId;


            return View(model);
        }

        [Route("/ModulKorisnik/Rezervacija/UrediSave")]
        [HttpPost]
        public IActionResult UrediSave(RezervacijaDodajVM model)
        {
            
                Rezervacija r = _db.Rezervacija.Find(model.RezervacijaId);
                RezervacijaStavke rs = _db.RezervacijaStavke.Where(w => w.RezervacijaId == model.RezervacijaId).FirstOrDefault();
                if (r == null)
                {
                    r = new Rezervacija();
                    rs = new RezervacijaStavke();
                    _db.Add(r);
                    _db.Add(rs);
                }


                r.NarucilacId = model.NarucilacId;
                r.Naziv = model.Naziv;
                r.DatumEvidencije = model.DatumEvidencije;
                r.DatumRezervacije = model.DatumRezervacije;

                rs.RezervacijaId = r.Id;
                rs.StolId = model.BrojStolova;

                _db.SaveChanges();

                return RedirectToAction("Index");
        }
    }
}