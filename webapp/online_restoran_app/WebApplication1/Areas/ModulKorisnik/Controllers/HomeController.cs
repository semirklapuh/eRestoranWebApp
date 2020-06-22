using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.EF;
using WebApplication1.Helper;
using WebApplication1.Models;

namespace WebApplication1.Areas.ModulKorisnik.Controllers
{
    [Autorizacija(zaposlenik: true, korisnik: true)]
    [Area("ModulKorisnik")]
    [Route("/ModulKorisnik/Home")]
    public class HomeController : Controller
    {
        private readonly MyContext _db;

        public HomeController(MyContext db)
        {
            _db = db;
        }
        [Route("/ModulKorisnik/Home/Index")]
        public IActionResult Index()
        {
           
            return View();
        }
    }
}