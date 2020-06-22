using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.EF;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Helper;
using WebApplication1.Areas.ModulKorisnik.ViewModels.Narudzba;

namespace WebApplication1.Areas.ModulKorisnik.Controllers
{
    [Autorizacija(zaposlenik: true, korisnik: true)]

    [Area("ModulKorisnik")]
    [Route("/ModulKorisnik/Narudzba")]
    public class NarudzbaController : Controller
    {

        private MyContext db;
        public NarudzbaController(MyContext baza)
        {
            db = baza;
        }

        [Route("/ModulKorisnik/Narudzba/Index")]
        public IActionResult Index()
        {
            NarudzbaIndexVM model = new NarudzbaIndexVM
            {
                Rows = db.Narudzba.Select(s => new NarudzbaIndexVM.Row
                {
                    NarudzbaId = s.Id,
                    ImePrezime = s.Narucilac.ImePrezime,
                    Adresa = s.Narucilac.Adresa,
                    Telefon = s.Narucilac.Telefon,
                    DatumNarudzbe = s.DatumNarudzbe,
                    VrijemeDostave = db.Dostava.Where(w => w.NarudzbaId == s.Id).Select(x => x.DatumVrijemeSlanja).FirstOrDefault(),
                    StatusDostaveId = db.Dostava.Where(w => w.NarudzbaId == s.Id)
                                                   .Select(sd => sd.StatusDostave.Id).SingleOrDefault(),
                    StatusDostave = db.Dostava.Where(w => w.NarudzbaId == s.Id)
                                              .Select(sd => sd.StatusDostave.Opis).SingleOrDefault(),
                    KorisnickiNalogId = s.Narucilac.KorisnickiNalogId


                }).ToList()
            };

            return View(model);
        }

        [Route("/ModulKorisnik/Narudzba/Obrisi")]
        [HttpGet]
        public IActionResult Obrisi(int id)
        {
            Narudzba obrisi = db.Narudzba.Find(id);
            List<NarudzbaStavke> obrisi2 = db.NarudzbaStavke.Where(w => w.NarudzbaId == id).ToList();
            Dostava obrisi3 = db.Dostava.Where(w => w.NarudzbaId == id).FirstOrDefault();
            if(obrisi3!= null)
            {
                db.Remove(obrisi3);
            }
            
            if(obrisi2 != null)
            {
                foreach (var item in obrisi2)
                {
                    db.Remove(item);
                }
            }
            
            db.Remove(obrisi);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("/ModulKorisnik/Narudzba/Detalji")]
        [HttpGet]
        public IActionResult Detalji(int id)
        {

            NarudzbaDetaljiVM model = new NarudzbaDetaljiVM
            {
                ImePrezime = db.Narudzba.Where(w => w.Id == id).Select(s => s.Narucilac.ImePrezime).SingleOrDefault(),
                Rows = db.NarudzbaStavke.Where(w => w.NarudzbaId == id).Select(s => new NarudzbaDetaljiVM.Row
                {
                    Kolicina = s.Kolicina,
                    Ukupno = db.Jelo.Where(w => w.Id == s.JeloId).Select(g => g.Cijena).FirstOrDefault() * s.Kolicina,
                    Jela = db.Jelo.Where(w => w.Id == s.JeloId).Select(g => new NarudzbaDetaljiVM.JeloVM
                    {
                        Naziv = g.Naziv,
                        Cijena = g.Cijena,
                    }).ToList()
                }).ToList()
            };
            return View(model);
        }

        [Route("/ModulKorisnik/Narudzba/Dodaj")]
        [HttpGet]
        public IActionResult Dodaj()
        {
            KorisnickiNalog korisnik = HttpContext.GetLogiraniKorisnik();
            Narucilac n = db.Narucilac.Where(w => w.KorisnickiNalogId == korisnik.Id).FirstOrDefault();

            NaruzbaDodajVM model = new NaruzbaDodajVM();

            model.NarucilacId = n.NarucilacId;
            model.ImePrezime = n.ImePrezime;
            model.Adresa = n.Adresa;
            model.DatumNarudzbe = DateTime.Now.ToString();
            model.Telefon = n.Telefon;
            model.SatusDostaveId = 2;
            model.StatusDostave = db.StatusDostave.Where(p => p.Id == 2).Select(p => p.Opis).FirstOrDefault();

            return View(model);
        }


        [Route("/ModulKorisnik/Narudzba/DodajSave")]
        [HttpPost]
        public IActionResult DodajSave(NaruzbaDodajVM model)
        {
            Narudzba n = db.Narudzba.Find(model.NarudzbaId);
            Dostava d = new Dostava();

            if (n == null)
            {
                n = new Narudzba();
                d = new Dostava();
                db.Add(n);

            }

            n.NarucilacId = model.NarucilacId;
            n.DatumNarudzbe = DateTime.Now;



            d.NarudzbaId = n.Id;
            d.StatusDostaveId = 2;
            d.DatumVrijemeSlanja = DateTime.Now;
            d.DatumVrijemeDostave = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("DodajStavke", new { id = n.Id });
        }

        [Route("/ModulKorisnik/Narudzba/DodajStavke")]
        [HttpGet]
        public IActionResult DodajStavke(int id)
        {
            NarudzbaDodajStavkeVM model = new NarudzbaDodajStavkeVM();
            model.Jela = db.Jelo.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = s.Naziv + "  " + s.Cijena + " KM",
                Value = s.Id.ToString()
            }).ToList();
            model.Kolicina = 1;
            model.NarudzbaId = id;
            return View(model);
        }

        [Route("/ModulKorisnik/Narudzba/DodajStavkeSave")]
        [HttpPost]
        public IActionResult DodajStavkeSave(NarudzbaDodajStavkeVM model)
        {
            
            NarudzbaStavke ns2 = db.NarudzbaStavke.Where(q => q.NarudzbaId == model.NarudzbaId).FirstOrDefault();
            if(ns2 == null)
            {
                
                    NarudzbaStavke ns = new NarudzbaStavke();
                    db.Add(ns);

                    ns.NarudzbaId = model.NarudzbaId;
                    ns.JeloId = model.JelaId;
                    ns.Kolicina = model.Kolicina;
                    ns.Cijena = model.Kolicina * db.Jelo.Where(q => q.Id == model.JelaId).Select(q => q.Cijena).FirstOrDefault();

                    db.SaveChanges();
                    return RedirectToAction("DetaljiRacuna", new { id = model.NarudzbaId });
                
            }
            else
            {
                if (ns2.JeloId != model.JelaId)
                {
                    NarudzbaStavke ns = new NarudzbaStavke();
                    db.Add(ns);

                    ns.NarudzbaId = model.NarudzbaId;
                    ns.JeloId = model.JelaId;
                    ns.Kolicina = model.Kolicina;
                    ns.Cijena = model.Kolicina * db.Jelo.Where(q => q.Id == model.JelaId).Select(q => q.Cijena).FirstOrDefault();

                    db.SaveChanges();
                    return RedirectToAction("DetaljiRacuna", new { id = model.NarudzbaId });
                }
            }
           

            return RedirectToAction("DetaljiRacuna", new { id = model.NarudzbaId });

        }

        [Route("/ModulKorisnik/Narudzba/DetaljiRacuna")]
        [HttpGet]
        public IActionResult DetaljiRacuna(int id)
        {

            NarudzbaDetaljiRacunVM model = new NarudzbaDetaljiRacunVM
            {
                NarudzbaId = id,
                ImePrezime = db.Narudzba.Where(w => w.Id == id).Select(s => s.Narucilac.ImePrezime).SingleOrDefault(),
                Rows = db.NarudzbaStavke.Where(w => w.NarudzbaId == id).Select(s => new NarudzbaDetaljiRacunVM.Row
                {
                    Kolicina = s.Kolicina,
                    Ukupno = db.Jelo.Where(w => w.Id == s.JeloId).Select(g => g.Cijena).FirstOrDefault() * s.Kolicina,
                    Jela = db.Jelo.Where(w => w.Id == s.JeloId).Select(g => new NarudzbaDetaljiRacunVM.JeloVM
                    {
                        Naziv = g.Naziv,
                        Cijena = g.Cijena,
                    }).ToList()
                }).ToList()
            };
            return View(model);
        }
    }
}