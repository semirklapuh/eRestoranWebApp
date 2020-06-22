using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.EF;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class NarucilacController : Controller
    {
        private MyContext _db;
        public NarucilacController(MyContext db)
        {
            _db = db;
        }

        public IActionResult Dodaj()
        {
            var model = new NarucilacDodajVM();

            model.gradLista = _db.Grad.Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.GradId.ToString()
            }).ToList();

            return View(model);
        }

        public IActionResult DodajSave(NarucilacDodajVM model)
        {
           
                Narucilac n = _db.Narucilac.Find(model.NarucilacId);
                KorisnickiNalog kn = new KorisnickiNalog();

                if (n == null)
                {
                    n = new Narucilac();

                    _db.Add(n);
                    _db.Add(kn);

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
             
           
            return Redirect("~/Autentifikacija");
        }


    }
}